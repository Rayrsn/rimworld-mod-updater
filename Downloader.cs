using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;

namespace RimworldModUpdater
{
    public delegate void ModDownloaded(object sender, BaseMod mod, string downloadFolder);
    public delegate void SteamExited(object sender);
    public delegate void SteamStarted(object sender);

    public class Downloader
    {
        public event ModDownloaded ModDownloaded;
        public event SteamExited SteamExited;
        public event SteamStarted SteamStarted;

        public string SteamCmdPath = Path.Combine(Environment.CurrentDirectory, "steamcmd");

        //private Process steamProcess;
        private bool showWindows;

        public Downloader(string steamCmdPath = null, bool showWindows = false)
        {
            SteamCmdPath = steamCmdPath ?? SteamCmdPath;
            this.showWindows = showWindows;
        }

        public async Task<bool> DownloadMods(List<BaseMod> mods)
        {
            try
            {
                // I'm not 100% sure this causes issues but... pretty sure so that's good enough. We don't need it anyway.
                File.Delete(Path.Combine(SteamCmdPath, "steamapps", "workshop", "appworkshop_294100.acf"));
            }
            catch (Exception ex) { }

            try
            {
                string downloadPath = Path.Combine(SteamCmdPath, "steamapps", "workshop", "content", "294100");

                Log.Information("Downloading {0} workshop mods with SteamCMD", mods.Count);

                StringBuilder script = new StringBuilder();

                // Create steamcmd script
                script.AppendLine("login anonymous");
                foreach (var listObject in mods)
                {
                    script.AppendLine($"workshop_download_item 294100 {listObject.ModId}");
                }
                script.AppendLine("quit");

                File.WriteAllText(Path.Combine(SteamCmdPath, "run.txt"), script.ToString());

                var startInfo = new ProcessStartInfo
                {
                    Arguments = "+runscript run.txt",
                    CreateNoWindow = !showWindows,
                    UseShellExecute = false,
                    FileName = Path.Combine(SteamCmdPath, "steamcmd.exe"),
                    WindowStyle = showWindows ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden
                };

                // Just make sure it's there
                Directory.CreateDirectory(downloadPath);

                Log.Information("Watching folder {0}", downloadPath);

                FileSystemWatcher watcher = new FileSystemWatcher(downloadPath);

                watcher.Created += delegate (object sender, FileSystemEventArgs args)
                {
                    // Check that this path is a directory and not a file just to be sure.
                    if (Directory.Exists(args.FullPath))
                    {
                        string id = args.Name;
                        var mod = mods.FirstOrDefault(x => x.ModId == id);

                        // Make sure this is actually a mod being downloaded by this instance of steamcmd and not another one.
                        // (probably, anyway)
                        if (mod?.Details != null)
                        {
                            mod.Downloaded = true;

                            ModDownloaded?.Invoke(this, mod, args.FullPath);

                            Log.Information("Downloaded mod folder created @ {0}", args.FullPath);
                        }
                    }
                };

                // Start watching folder
                watcher.EnableRaisingEvents = true;

                // Start SteamCMD
                var steamCMD = new Process { StartInfo = startInfo };

                Log.Information("Starting SteamCMD instance for download of {0} mods", mods.Count);
                steamCMD.Start();

                // Wait for SteamCMD to start and login.
                await Task.Delay(3000);

                // Today on unreliable and arbitrary events
                SteamStarted?.Invoke(this);

                // wait for steamcmd to exit
                while (!steamCMD.HasExited)
                {
                    await Task.Delay(100);
                }

                // Stop watching the download folder & get rid of watcher
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();

                await Task.Delay(500);

                SteamExited?.Invoke(this);

                Log.Information($"Finished downloading {mods.Count} mods from workshop. SteamCMD instance closed.");

                return true;
            }
            catch (Exception ex)
            {
                StringBuilder str = new StringBuilder();
                mods.ForEach(x => str.AppendLine(x.ModId + " - " + x.Details.title));
                Log.Error(ex, "Error occurred while downloading {0} mods with SteamCMD. Mods:\n{1}", mods.Count, str);
            }

            return false;
        }
    }
}
