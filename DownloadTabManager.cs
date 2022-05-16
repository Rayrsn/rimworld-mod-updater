using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Serilog;
using Timer = System.Timers.Timer;

namespace RimworldModUpdater
{
    public class DownloadTabManager
    {
        public List<BaseMod> downloadingMods = new List<BaseMod>();
        private Downloader downloader;
        private Updater updater;
        private Timer updateTimer;
        private ProgressBar progressBar;
        private Label statusLabel;
        private bool needsUpdate;

        private static string downloadPath = Path.Combine(Environment.CurrentDirectory, "steamcmd", "steamapps",
            "workshop", "content", "294100");

        public DownloadTabManager(ProgressBar progressBar, Label statusLabel, Updater updater)
        {
            this.progressBar = progressBar;
            this.statusLabel = statusLabel;

            progressBar.Value = 0;
            progressBar.Maximum = 1;

            statusLabel.Text = "Waiting...";

            updateTimer = new Timer();
            updateTimer.Interval = 100;
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                if (!needsUpdate) return;

                Log.Information("Updating download tab status & stuff");

                if (downloadingMods.Count > 0)
                {
                    double totalSize = 0;
                    downloadingMods.ForEach(x => totalSize += x.Details?.file_size ?? 0);

                    SetStatus($"Downloading {downloadingMods.Count} mods ({Utils.GetSizeTextForMods(downloadingMods)})");
                    
                    SetProgressStyle(ProgressBarStyle.Marquee);
                }
                else
                {
                    SetStatus("Waiting...");
                    SetProgressStyle(ProgressBarStyle.Blocks);
                }

                needsUpdate = false;
            };

            updateTimer.Start();

            this.updater = updater;

            downloader = new Downloader();

            downloader.ModDownloaded += delegate(object sender, BaseMod mod, string folder)
            {
                // This doesn't work; Download folder still in use by steamcmd by the time this runs because it's when the folder is created.
                // TODO: More testing 
                //Log.Information($"User downloaded mod {mod.ModId}; Moving it into Mods folder.");
                //updater.UpdateMod(mod, Path.Combine(Environment.CurrentDirectory, "steamcmd", "steamapps", "workshop", "content", "294100"), true);

                downloadingMods.RemoveAll(x => x.ModId == mod.ModId);
                needsUpdate = true;
            };
        }

        private void SetStatus(string txt)
        {
            statusLabel.Invoke((MethodInvoker) (() =>
            {
                statusLabel.Text = txt;
            }));
        }
        private void SetProgressStyle(ProgressBarStyle style)
        {
            progressBar.Invoke((MethodInvoker)(() =>
            {
                progressBar.Style = style;
            }));
        }

        public async Task<bool> DownloadMod(BaseMod mod)
        {
            if (downloadingMods.Exists(x => x.ModId == mod.ModId))
            {
                Log.Information("User tried to download an already downloading mod.");
                return false;
            };

            Log.Information($"User started a download for new mod {mod.ModId}");

            downloadingMods.Add(mod);
            needsUpdate = true;

            bool ok = await downloader.DownloadMods(new List<BaseMod>{mod});

            // Just to make sure...
            downloadingMods.RemoveAll(x => x.ModId == mod.ModId);
            needsUpdate = true;

            if (!ok)
            {
                MessageBox.Show($"An error occurred while downloading workshop mod {mod.ModId}; Check the log file.", "Uh oh", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            if (!mod.Downloaded)
            {
                MessageBox.Show($"Failed to download mod {mod.Title} ({mod.ModId}). SteamCMD exited but no download folder for mod was created.\nCheck log.txt & steamcmd/logs/workshop_log.txt", "Download failed", MessageBoxButtons.OK);

                return false;
            }

            Log.Information($"User downloaded new mod {mod.ModId}; Moving it into Mods folder.");

            // TODO: Maybe some kind of setting for file verification? The only thing steam tells us about mod files is the size but we could use that.

            //string folder = Path.Combine(downloadPath, mod.ModId);

            //var info = new DirectoryInfo(folder);

            //Log.Information("Mod size {0}", mod.Details.file_size);

            //info.

            updater.UpdateMod(mod, downloadPath, true);

            return true;
        }

        public async Task<bool> DownloadMods(List<BaseMod> mods)
        {
            Log.Information($"User started a download for {mods.Count} new mods.");
            downloadingMods.AddRange(mods);
            needsUpdate = true;

            bool ok = await downloader.DownloadMods(mods);

            if (!ok)
            {
                MessageBox.Show($"An error occurred while downloading {mods.Count} workshop mods; Check the log file.", "Uh oh", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }



            Log.Information($"User downloaded {mods.Count} new mods; Moving them into Mods folder.");

            mods.ForEach(mod => updater.UpdateMod(mod, downloadPath, true));

            return true;
        }
    }
}
