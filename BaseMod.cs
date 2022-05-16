using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;

namespace RimworldModUpdater
{
    public class BaseMod
    {
        public string ModPath;
        public string ModId;
        public WorkshopFileDetails Details;
        public bool Updated;
        public bool Downloaded;
        public string Folder => Path.GetFileName(ModPath);
        public DateTimeOffset LocalCreatedTime => Updater.GetModLastUpdatedTime(ModPath);
        public DateTimeOffset RemoteUpdatedTime => Details.TimeUpdated;
        public DateTimeOffset RemoteCreatedTime => Details.TimeCreated;
        public string SizeTag => $"{Math.Round((Details?.file_size / 1024d / 1024d) ?? 0, 3)} MB";
        public double Size => Math.Round((Details?.file_size / 1024d / 1024d) ?? 0, 3);
        public string Title => Details?.title;

        public async Task<WorkshopFileDetails> GetWorkshopDetails()
        {
            if (Details != null) return Details;

            string idPath = GetIdPath();

            string id = ModId;

            if (id == null)
            {
                if (!File.Exists(idPath)) return null;

                id = File.ReadAllText(idPath);
            }
            
            var details = await SteamWorkshop.GetWorkshopFileDetails(id);

            Details = details;
            if (ModId == null) ModId = id;

            return details;
        }

        public bool RestoreBackup()
        {
            string path = ModPath;
            string backupPath = Path.Combine(path, "update_backup");

            if (!Directory.Exists(backupPath))
            {
                Log.Error($"User tried to restore backup for mod with no backup folder {path}"); 
                return false;
            }

            List<String> files = Directory.GetFiles(path, "*").ToList();
            List<String> directories = Directory.GetDirectories(path, "*").ToList();

            foreach (string file in files)
            {
                File.Delete(file);
            }

            foreach (string dir in directories)
            {
                if (Path.GetFileName(dir) == "update_backup")
                    continue;

                Directory.Delete(dir, true);
            }

            Utils.MoveShit(backupPath, path, "update_backup");
            Directory.Delete(backupPath);
            MessageBox.Show($"Restored backup for {Folder}.");
            Log.Information("Restored update backup for {0}.", Folder);

            return true;
        }

        public string GetAboutPath()
        {
            return Path.Combine(ModPath, "About", "About.xml");
        }

        public string GetIdPath()
        {
            return Path.Combine(ModPath, "About", "PublishedFileId.txt");
        }

        public bool FolderExists()
        {
            return Directory.Exists(ModPath);
        }

        public bool AboutFileExists()
        {
            return File.Exists(GetAboutPath());
        }
    }
}
