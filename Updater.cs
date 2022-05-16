using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace RimworldModUpdater
{
    public class Updater
    {
        public string GamePath;
        public string ModsPath;
        public bool Updating = false;

        public Updater(string gamePath)
        {
            GamePath = gamePath;
            ModsPath = Path.Combine(gamePath, "Mods");
        }

        public bool SetGamePath(string path)
        {
            GamePath = path;
            ModsPath = Path.Combine(path, "Mods");

            if (!Directory.Exists(ModsPath))
            {
                return false;
            }

            return true;
        }

        public static DateTimeOffset GetModLastUpdatedTime(string modPath)
        {
            string aboutPath = Path.Combine(modPath, "About");
            string lastUpdatedPath = Path.Combine(aboutPath, ".lastupdated");

            // Check for .lastupdated timestamp file
            if (File.Exists(lastUpdatedPath))
            {
                string str = File.ReadAllText(lastUpdatedPath);
                if (!String.IsNullOrWhiteSpace(str) && long.TryParse(str, out long time))
                {
                    return DateTimeOffset.FromUnixTimeSeconds(time);
                }
                else
                {
                    Log.Error("Invalid .lastupdated file format at {0}. Deleting file.");
                    File.Delete(lastUpdatedPath);
                }
            }

            // Forgot this didn't work because a mod's files will obviously be modified *before* it's uploaded to the workshop, making the comparison useless.
            if (Settings.UseModifiedDate)
            {
                var info = new DirectoryInfo(modPath);
                var ret = Directory.GetLastWriteTime(modPath); 
                //var dirs = Directory.GetFiles(modPath, "*", SearchOption.AllDirectories);

                info.EnumerateFiles("*.*", SearchOption.AllDirectories).AsParallel().ForAll(x =>
                {
                    ret = x.LastWriteTime > ret ? x.LastWriteTime : ret;
                });

                return DateTimeOffset.FromFileTime(ret.ToFileTime()).UtcDateTime;
            }

            string fileIdPath = Path.Combine(aboutPath, "PublishedFileId.txt");
            return DateTimeOffset.FromFileTime(File.GetCreationTime(fileIdPath).ToFileTime()).UtcDateTime;
        }

        public async Task<string> QueryModId(string modPath)
        {
            string aboutPath = Path.Combine(modPath, "About");
            if (!Directory.Exists(aboutPath))
            {
                Log.Warning($"Mod folder {Path.GetFileName(modPath)} has no About subfolder. Not a mod?");
                return null;
            }

            string fileIdPath = Path.Combine(aboutPath, "PublishedFileId.txt");
            if (!File.Exists(fileIdPath))
            {
                Log.Warning($"Mod folder {Path.GetFileName(modPath)} has no PublishedFileId.txt. Not a workshop mod?");
                return null;
            }

            string fileId = File.ReadAllText(fileIdPath).Trim();
            if (String.IsNullOrWhiteSpace(fileId))
            {
                Log.Warning($"Mod folder {Path.GetFileName(modPath)} PublishedFileId.txt is empty.");
                return null;
            }

            return fileId;
        }

        public async Task QueryModBatch(List<BaseMod> batch, int retries = 0)
        {
            var detailList = await SteamWorkshop.GetWorkshopFileDetails(batch.ToArray());

            if (detailList == null)
            {
                if (retries <= 3)
                {
                    Log.Warning("Failed to query batch of {0} mods. Retry {1}...", batch.Count, retries);
                    retries++;
                    await QueryModBatch(batch, retries);
                }
                else
                {
                    Log.Warning("Failed to query batch of {0} mods after {1} retries.", batch.Count, retries);
                }

                return;
            }
            else
            {
                if (retries > 0)
                {
                    Log.Information("Got batch of {0} mods successfully after {1} retries.", batch.Count, retries);
                }
            }

            foreach (var details in detailList)
            {
                var mod = batch.FindAll(x => x.ModId == details.publishedfileid);
                mod.ForEach(x => x.Details = details);
            }
        }

        public async Task QueryFiles()
        {
            var folders = Directory.GetDirectories(ModsPath);

            int numFolders = folders.Length;
            int count = 0;

            if (folders.Length == 0)
            {
                Log.Error("Tried to query mod folders but found none.");
                MessageBox.Show("You have no mods installed!", "bruh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdaterForm.SetProgressBounds(numFolders);
            UpdaterForm.SetProgress(0);
            UpdaterForm.SetStatus($"Querying {numFolders} mod folders for outdated mods...");

            Log.Information($"Querying {numFolders} mod folders for outdated mods...");

            List<BaseMod> mods = new List<BaseMod>();
            for (int i = 0; i < numFolders; i++)
            {
                string id = await QueryModId(folders[i]);
                if (!String.IsNullOrWhiteSpace(id))
                {
                    Log.Information("Got valid mod folder {0} ({1})", folders[i], id);

                    mods.Add(new BaseMod
                    {
                        ModPath = folders[i],
                        ModId = id
                    });

                    count++;
                }

                UpdaterForm.UpdateProgress();
            }

            UpdaterForm.SetStatus($"Found {count}/{numFolders} valid (steam) mod folders.");
            Log.Information("Found {0}/{1} valid mod folders.", count, numFolders);

            int batchCount = 50;

            int num = 0;
            int num2 = 0;
            int len = mods.Count;

            UpdaterForm.SetStatus($"Querying {len} mods in {(int)Math.Ceiling((double)len / batchCount)} batches of {batchCount}");
            Log.Information("Querying {0} mods in {1} batches of {2}", len, (int)Math.Ceiling((double)len / batchCount), batchCount);

            UpdaterForm.ResetProgress();
            UpdaterForm.SetProgressBounds((int)Math.Ceiling((double)len / batchCount));

            bool retry = false;
            var tasks = new List<Task>();
            for (int i = 0; i < len; i += batchCount)
            {
                var task = Task.Run(async () =>
                {
                    var batch = mods.GetRange(i, Math.Min(batchCount, len - i));

                    await QueryModBatch(batch);

                    num2 += batchCount;

                    Log.Information($"Queried {Math.Min(num2, len)}/{len} mods.");
                    UpdaterForm.SetStatus($"Queried {Math.Min(num2, len)}/{len} mods.");
                    UpdaterForm.UpdateProgress();
                });

                tasks.Add(task);

                await Task.Delay(250);
            }

            await Task.WhenAll(tasks);

            // Let's just send it all at once lol
            //var files = await SteamWorkshop.GetWorkshopFileDetails(fileIds.Keys.ToArray());

            Log.Information($"Got workshop file details for {mods.Count(x => x.Details != null)} mods.");


            UpdaterForm.SetProgressBounds(mods.Count);
            UpdaterForm.ResetProgress();

            int updateCount = 0;
            foreach (var mod in mods)
            {
                UpdaterForm.UpdateProgress();
                var details = mod.Details;
                string folderName = mod.Folder;

                if (details == null)
                {
                    Log.Error($"Couldn't get any file details for mod {mod.ModId} ({folderName}).");
                    continue;
                }

                string id = details.publishedfileid;
                if (details.result == 9)
                {
                    Log.Warning($"Tried to query workshop file {id} ({folderName}) but no file could be found. (Code 9). This could mean the mod has been removed/unlisted");
                    continue;
                }

                if (details.result != 1)
                {
                    Log.Warning($"Tried to query workshop file {id} ({folderName}) but steam returned code {details.result}");
                }

                if (details.visibility != 0)
                {
                    Log.Warning($"Got workshop file {id} ({folderName}) but it's a private file.");
                    continue;
                }

                if (details.banned == 1)
                {
                    Log.Warning($"Got workshop file {id} ({folderName}) but it's a banned file.");
                    continue;
                }

                if (details.creator_app_id != 294100)
                {
                    Log.Warning($"Got workshop file {id} ({folderName}) but it's not a rimworld mod! (Huh?)");
                    continue;
                }

                var modPath = mod.ModPath;

                var remoteDate = details.TimeUpdated;
                var lastUpdatedDate = GetModLastUpdatedTime(modPath);

                //Log.Information("{0} < {1}: {2}", remoteDate.ToUnixTimeSeconds(), lastUpdatedDate.ToUnixTimeSeconds(), (remoteDate < lastUpdatedDate));
                if (remoteDate > lastUpdatedDate)
                {
                    updateCount++;
                    Log.Information($"Mod folder {folderName} ({details.publishedfileid}) has an update available.");

                    UpdaterForm.AddRow(mod);

                    await Task.Delay(1);
                }
            }

            Log.Information("There are {0} mods with updates available.", updateCount);
        }

        public bool UpdateMod(BaseMod mod, string downloadPath, bool forceNoBackup = false)
        {
            var details = mod.Details;
            string downloadFolder = Path.Combine(downloadPath, details.publishedfileid);

            string modPath = Path.Combine(ModsPath, mod.Folder);
            string oldFolder = Path.Combine(modPath, "update_backup");
            Log.Information($"Moving downloaded mod files for {Path.GetFileName(downloadFolder)} ({mod.ModId}) into mod folder \"{Path.GetFileName(modPath)}\"");

            if (!Directory.Exists(downloadFolder))
            {
                Log.Error($"Couldn't find downloaded folder for {details.title} ({details.publishedfileid}). Skipping update.");
                return false;
            }

            if (!File.Exists(Path.Combine(downloadFolder, "About", "About.xml")))
            {
                Log.Error($"Couldn't update mod {details.title} ({details.publishedfileid}). About.xml is missing from downloaded files.");
                return false;
            }

            // Throw an error if only About.xml exists in the download folder AND the amount of files in the original mod folder is larger than 1
            // edit: Huh? Why did I do this again?
            if (Utils.CountShit(downloadFolder) <= 1 && Utils.CountShit(mod.ModPath) > 1)
            {
                Log.Error($"Couldn't update mod {details.title} ({details.publishedfileid}). No files exist other than About.xml???");
                return false;
            }

            if (Settings.ShouldBackupMods && !forceNoBackup)
            {
                if (!Directory.Exists(oldFolder))
                {
                    Directory.CreateDirectory(oldFolder);
                }
                else
                {
                    Directory.Delete(oldFolder, true);
                    Directory.CreateDirectory(oldFolder);
                }

                Utils.MoveShit(modPath, oldFolder, "update_backup");
            }
            else
            {
                Directory.CreateDirectory(modPath);
                Utils.DeleteShit(modPath);
            }

            Utils.MoveShit(downloadFolder, modPath, "update_backup");
            Directory.Delete(downloadFolder, true);

            if (Directory.Exists(Path.Combine(modPath, "About")))
            {
                string path = Path.Combine(modPath, "About", ".lastupdated");
                File.WriteAllText(path, DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
                File.SetAttributes(path, FileAttributes.Hidden);
            }

            return true;
        }

        public async Task<bool> UpdateMods(List<BaseMod> mods, bool forceNoBackup = false)
        {
            this.Updating = true;
            UpdaterForm.SetStatus($"Starting SteamCMD...");

            int statusCount = 0;
            double totalSize = 0;
            mods.ForEach(x => totalSize += x.Details?.file_size ?? 0);

            double totalSizeTemp = totalSize;
            UpdaterForm.SetProgress(0, ProgressBarStyle.Blocks);
            UpdaterForm.SetProgressBounds(mods.Count);

            var downloader = new Downloader();

            downloader.ModDownloaded += delegate (object sender, BaseMod mod, string folder)
            {
                statusCount++;
                totalSizeTemp -= mod.Details.file_size;

                UpdaterForm.SetStatus($"Downloaded {statusCount}/{mods.Count} mods ({Math.Round(totalSizeTemp / 1024d / 1024d, 2)} MB)");
                UpdaterForm.UpdateProgress();
            };

            downloader.SteamStarted += sender =>
            {
                UpdaterForm.SetStatus($"Downloaded {statusCount}/{mods.Count} mods ({Math.Round(totalSizeTemp / 1024d / 1024d, 2)} MB)");
            };

            bool ok = await downloader.DownloadMods(mods);

            if (!ok)
            {
                this.Updating = false;
                MessageBox.Show("An error occurred while downloading workshop mods; Check the log file.", "Uh oh", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            Log.Information("Updating mod files...");
            UpdaterForm.SetStatus("Updating mod files...");
            string modsPath = Path.Combine(ModsPath);
            string toolPath = Path.GetFullPath("./");
            string downloadContentPath = Path.Combine(toolPath, "steamcmd", "steamapps", "workshop", "content", "294100");

            UpdaterForm.SetProgress(0, ProgressBarStyle.Blocks);
            UpdaterForm.SetProgressBounds(mods.Count);

            foreach (var obj in mods)
            {
                if (!UpdateMod(obj, downloadContentPath, forceNoBackup))
                {
                    UpdaterForm.UpdateProgress();
                    continue;
                }

                UpdaterForm.UpdateProgress();
                obj.Updated = true;

                if (!forceNoBackup)
                    UpdaterForm.UpdateObject(obj);
            }

            UpdaterForm.SetStatus($"Finished updating {mods.Count} mods! A total of {Math.Round(totalSize / 1024d / 1024d, 2)} MB was downloaded.");
            Log.Information($"Finished updating {mods.Count} mods.");

            this.Updating = false;
            return true;
        }
    }
}
