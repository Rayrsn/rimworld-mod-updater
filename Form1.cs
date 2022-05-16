using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CefSharp;
using Microsoft.WindowsAPICodePack.Dialogs;
using Serilog;

namespace RimworldModUpdater
{
    public partial class UpdaterForm : Form
    {
        private static UpdaterForm _instance;
        private Updater currentUpdater;
        private DownloadTabManager downloadTabManager;

        public UpdaterForm()
        {
            InitializeComponent();
        }

        private void UpdaterForm_Load(object s, EventArgs e)
        {
            _instance = this;

            this.Text = "Rimworld Mod Updater " + Utils.GetProgramVersion();

            // It's times like this when I really question myself
            new ToolTip().SetToolTip(btnManualUpdate, "Select a mod in your mods folder to update manually.\nWill replace files regardless of whether it's outdated or not.");
            new ToolTip().SetToolTip(btnReset, "Reset the updater to it's inital state, allowing you to query for outdated mods again.");
            new ToolTip().SetToolTip(btnOpenFileDialog, "Opens a file dialog for you to choose your rimworld installation folder.\nIf you put the updater in your rimworld folder, it should be set for you.");
            new ToolTip().SetToolTip(textGamePath, "The rimworld game path. Shit wont work if this isn't set.");
            new ToolTip().SetToolTip(btnViewDownloadMod, "Start downloading this mod/collection.\nIf it's a collection, it may take a while to query the collection & all of the mods in it depending on the size.");

            // Fixes an issue where context menu isn't opened on first try. Not completely sure why this fixes it.
            listMenu.Items.Add("");
            downloadedModMenu.Items.Add("");

            if (Directory.Exists("./Mods"))
            {
                // Talk about a cluttered rimworld folder if you had this piece of shit in the main folder.
                textGamePath.Text = Path.GetFullPath("./");
            }
            else if (Directory.Exists("../Mods"))
            {
                textGamePath.Text = Path.GetFullPath("../");
            }

            listObjectsParsed.ItemsChanged += delegate (object sender, ItemsChangedEventArgs args)
            {
                lblObjectsParsed.Text = $"Mods with updates available: {listObjectsParsed.GetItemCount()}";
            };

            textViewUrlBar.Text = "Loading browser...";
            textViewUrlBar.Enabled = false;

            var clicker = new WorkshopJSClicker();

            clicker.Clicked += (id) =>
            {
                if (id == null)
                {
                    StartModDownload(null);
                }
                else
                {
                    StartModDownload("https://steamcommunity.com/sharedfiles/filedetails/?id=" + id);
                }
            };

            chromiumWebBrowser.JavascriptObjectRepository.Register("_____updater", clicker, false);
            if (File.Exists("steamHelper.js"))
            {
                chromiumWebBrowser.ExecuteScriptAsyncWhenPageLoaded(File.ReadAllText("steamHelper.js"), false);
            }
            chromiumWebBrowser.Load("https://steamcommunity.com/app/294100/workshop/");

            chromiumWebBrowser.LifeSpanHandler = new LifeSpanHandler();

            currentUpdater = new Updater(textGamePath.Text);
            downloadTabManager = new DownloadTabManager(downloaderTabProgressBar, downloaderTabStatus, currentUpdater);

            Utils.CheckForUpdates();
        }

        public static void SetStatus(string txt)
        {
            if (_instance.lblStatus.InvokeRequired)
            {
                _instance.lblStatus.BeginInvoke((MethodInvoker)delegate { _instance.lblStatus.Text = txt; });
            }
            else
            {
                _instance.lblStatus.Text = txt;
            }
        }

        public static void SetProgressBounds(int max)
        {
            if (_instance.progressBar.InvokeRequired)
            {
                _instance.progressBar.BeginInvoke((MethodInvoker)delegate { _instance.progressBar.Maximum = max; });
            }
            else
            {
                _instance.progressBar.Maximum = max;
            }
        }

        public static void SetProgress(int num, ProgressBarStyle style = ProgressBarStyle.Continuous)
        {
            if (_instance.progressBar.InvokeRequired)
            {
                _instance.progressBar.BeginInvoke((MethodInvoker)delegate
               {
                   _instance.progressBar.Value = num;
                   _instance.progressBar.Style = style;
               });
            }
            else
            {
                _instance.progressBar.Value = num;
                _instance.progressBar.Style = style;
            }
        }

        public static void ResetProgress()
        {
            _instance.progressBar.Value = 0;
            _instance.progressBar.Maximum = 0;
        }

        public static void UpdateProgress()
        {
            if (_instance.progressBar.InvokeRequired)
            {
                _instance.progressBar.BeginInvoke((MethodInvoker)delegate
               {
                   if (_instance.progressBar.Value != _instance.progressBar.Maximum)
                       _instance.progressBar.Value++;
               });
            }
            else
            {
                if (_instance.progressBar.Value != _instance.progressBar.Maximum)
                    _instance.progressBar.Value++;
            }

        }

        public static void AddRow(BaseMod mod)
        {
            _instance.listObjectsParsed.AddObject(mod);
        }

        public static void AddRow(string path, string name, DateTime local, DateTime remote, WorkshopFileDetails file)
        {
            _instance.listObjectsParsed.AddObject(new BaseMod
            {
                ModPath = path,
                Details = file
            });
        }

        public static void UpdateObject(BaseMod obj)
        {
            _instance.listObjectsParsed.UpdateObject(obj);
        }

        private async void btnQueryFiles_Click(object sender, EventArgs e)
        {
            if (btnQueryFiles.Text == "Update All")
            {
                // Bad things happen if you update with rimworld running
                if (Process.GetProcessesByName("RimWorldWin64.exe").Length > 0 || Process.GetProcessesByName("RimWorldWin.exe").Length > 0)
                {
                    MessageBox.Show("You cannot update your mods while rimworld is running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (listObjectsParsed.GetItemCount() == 0) return;

                var allRows = listObjectsParsed.Objects.Cast<BaseMod>().ToList();

                // Don't update any mods that have already been updated (through context menu)
                allRows.RemoveAll(x => x.Updated == true);

                if (allRows.Count == 0) return;

                if (MessageBox.Show($"Are you sure you want to update these {allRows.Count} mods?\nThe total download size will be ~{Utils.GetSizeTextForMods(allRows)}", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    //SetDownloadButtonState(true);
                    return;
                }

                Task.Run(() => { currentUpdater.UpdateMods(allRows); });
            }
            else
            {
                if (currentUpdater.Updating)
                {
                    MessageBox.Show("Already updating mod(s). Please wait until they're finished.",
                        "Look it's a title!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string gamePath = textGamePath.Text;
                if (!Utils.IsValidGamePath(gamePath))
                {
                    MessageBox.Show(
                        "Invalid game path; Select the folder that has your rimworld installation.\nex: C:/Games/Rimworld\n\n The program won't work if the folder you select has no Mods folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnQueryFiles.Enabled = false;
                try
                {
                    await currentUpdater.QueryFiles();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to query mods.");
                    MessageBox.Show("An exception occurred while querying mods. Check log for details.", "Uh Oh", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _instance.lblStatus.Text = "Waiting...";
                    _instance.btnQueryFiles.Enabled = true;
                    return;
                }

                if (listObjectsParsed.GetItemCount() == 0)
                {
                    _instance.lblStatus.Text = "Found no mods with updates available.";
                }

                _instance.btnQueryFiles.Text = "Update All";
                _instance.btnQueryFiles.Enabled = true;
                _instance.btnReset.Enabled = true;
            }
        }

        private void listMenu_Opening(object sender, CancelEventArgs e)
        {
            var selectedRows = listObjectsParsed.SelectedObjects;
            var menu = listMenu;
            listMenu.Items.Clear();

            if (selectedRows.Count > 1)
            {
                listMenu.Items.Add("Update selected mods", null, (o, args) =>
                {
                    if (currentUpdater.Updating)
                    {
                        MessageBox.Show("Already updating mod(s). Please wait until they're finished.",
                            "Look it's a title!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    Log.Information($"Updating {selectedRows.Count} selected mods.");
                    Task.Run(async () =>
                    {
                        currentUpdater.UpdateMods(selectedRows.Cast<BaseMod>().ToList());
                    });
                });
            }
            else if (selectedRows.Count == 1)
            {
                var mod = selectedRows[0] as BaseMod;
                listMenu.Items.Add("Update", null, (o, args) =>
                {
                    if (currentUpdater.Updating)
                    {
                        MessageBox.Show("Already updating mod(s). Please wait until they're finished.\n If you'd like to update multiple specific mods at a time, you can Ctrl+Click on each entry to select multiple at a time.",
                            "Look it's a title!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    Log.Information($"Updating selected mod {mod.ModId} ({mod.Folder}).");
                    Task.Run(() =>
                    {
                        currentUpdater.UpdateMods(selectedRows.Cast<BaseMod>().ToList());
                    });
                });

                var restoreBackupEntry = new ToolStripMenuItem("Restore Backup", null, (o, args) =>
                {
                    mod?.RestoreBackup();
                });

                if (!Directory.Exists(Path.Combine(mod.ModPath, "update_backup")))
                {
                    restoreBackupEntry.Enabled = false;
                }

                listMenu.Items.Add(restoreBackupEntry);
                listMenu.Items.Add(new ToolStripSeparator());
                listMenu.Items.Add("Open mod folder", null, (o, args) => Process.Start(Path.Combine(currentUpdater?.ModsPath ?? textGamePath.Text, mod.Folder)));
                listMenu.Items.Add("Open workshop page", null, (o, args) => Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=" + mod.ModId));
                listMenu.Items.Add("Open changelog page", null, (o, args) => Process.Start("https://steamcommunity.com/sharedfiles/filedetails/changelog/" + mod.ModId));
            }

            if (selectedRows.Count > 0)
            {
                listMenu.Items.Add(new ToolStripSeparator());
                listMenu.Items.Add("Remove selected", null, (o, args) =>
                {
                    foreach (var row in selectedRows)
                    {
                        listObjectsParsed.RemoveObject(row);
                    }

                    lblObjectsParsed.Text = "Mods with updates available: " + listObjectsParsed.GetItemCount();
                });
            }
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = "Select rimworld folder (Where the Mods folder is)";
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = textGamePath.Text;
                Log.Information("Opening folder dialog in {0}", dialog.InitialDirectory);

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    textGamePath.Text = dialog.FileName;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            listObjectsParsed.ClearObjects();
            btnQueryFiles.Text = "Query Mod Files";
            btnQueryFiles.Enabled = true;
            btnReset.Enabled = false;
            progressBar.Value = 0;
            progressBar.Maximum = 0;
            lblObjectsParsed.Text = "Mods with updates available: " + listObjectsParsed.GetItemCount();
            lblStatus.Text = "Waiting...";
        }

        private void btnManualUpdate_Click(object sender, EventArgs e)
        {
            if (currentUpdater.Updating)
            {
                MessageBox.Show("Already updating mod(s). Please wait until they're finished.", "Look it's a title!",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = "Select a mod folder";
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = Path.Combine(textGamePath.Text, "Mods");
                Log.Information("Opening folder dialog in {0}", dialog.InitialDirectory);

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string modPath = dialog.FileName;
                    string fileIdPath = Path.Combine(modPath, "About", "PublishedFileId.txt");
                    if (!File.Exists(fileIdPath))
                    {
                        MessageBox.Show("This is either not a mod folder or the mod does not have a PublishedFileId.txt file in About/", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Task.Run(async () =>
                    {
                        string fileId = File.ReadAllText(fileIdPath);
                        var details = await SteamWorkshop.GetWorkshopFileDetails(fileId);

                        if (details.result == 9)
                        {
                            MessageBox.Show($"Couldn't find file id {details.publishedfileid} on the workshop.", "Failed to update mod.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        BaseMod obj = new BaseMod
                        {
                            Details = details,
                            ModPath = modPath,
                        };

                        await currentUpdater.UpdateMods(new List<BaseMod> { obj }, false);
                        //await Downloader.DownloadWorkshopItem(obj, new Updater(Path.GetFullPath(Path.Combine(modPath, "../"))));
                        MessageBox.Show($"Updated {details.title} ({details.publishedfileid}).", "Updated mod successfully.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnReset.BeginInvoke((MethodInvoker)(() => btnReset.Enabled = true));
                    });

                }
            }
        }

        private void btnViewBack_Click(object sender, EventArgs e)
        {
            if (chromiumWebBrowser.CanGoBack)
                chromiumWebBrowser.Back();
        }

        private void btnViewForward_Click(object sender, EventArgs e)
        {
            if (chromiumWebBrowser.CanGoForward)
                chromiumWebBrowser.Forward();
        }

        private void textViewUrlBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                chromiumWebBrowser.Load(textViewUrlBar.Text);
            }
        }

        public void SetDownloadButtonState(bool state)
        {
            btnViewDownloadMod.Invoke((MethodInvoker)(() =>
           {
               btnViewDownloadMod.Enabled = state;
           }));
        }

        public void btnViewDownloadMod_Click(object sender, EventArgs e)
        {
            StartModDownload(null);
        }

        private void StartModDownload(string url)
        {
            SetDownloadButtonState(false);
            if (!Utils.IsValidGamePath(textGamePath.Text))
            {
                Log.Error("User tried to download a mod but game path was invalid.");
                MessageBox.Show("Invalid game path; Select the folder that has your rimworld installation.\nex: C:/Games/Rimworld\n\n The program won't work if the folder you select has no Mods folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetDownloadButtonState(true);
                return;
            }


            string gamePath = textGamePath.Text;
            string modsPath = Path.Combine(gamePath, "Mods");
            url = url ?? chromiumWebBrowser.Address;
            string id = regex.Match(url).Value;
            if (String.IsNullOrEmpty(id) || !url.Contains("filedetails"))
            {
                Log.Error("User tried to download a mod but no file id was found in " + url);
                MessageBox.Show("Couldn't find a file id for this page. Is this a workshop item/collection?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetDownloadButtonState(true);
                return;
            }

            Task.Run(async () =>
            {
                Log.Information("User wants to download stuff from {0}", url);
                bool isCollection = await SteamWorkshop.IsWorkshopCollection(id);
                if (isCollection)
                {
                    List<WorkshopFileDetails> files = await SteamWorkshop.GetWorkshopFileDetailsFromCollection(id);

                    if (files == null)
                    {
                        MessageBox.Show($"Request to get workshop file {id} details failed. See log for details. Try again?", "Uh oh",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    if (MessageBox.Show($"This is a collection. Are you sure you want to download these {files.Count} mods?\nThe total download size is ~{Utils.GetSizeTextForMods(files)}", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        SetDownloadButtonState(true);
                        return;
                    }

                    SetDownloadButtonState(true);

                    SetProgressBounds(1);
                    SetProgress(0, ProgressBarStyle.Marquee);
                    Log.Information("Querying files in collection " + id);
                    List<BaseMod> mods = new List<BaseMod>();
                    foreach (WorkshopFileDetails file in files)
                    {
                        if (!file.IsValidResult())
                        {
                            Log.Warning($"{file.publishedfileid} returned an invalid result from collection. Ignoring.");
                            continue;
                        }

                        string folder = Utils.SanitizeFilename(file.title);

                        mods.Add(new BaseMod
                        {
                            Details = file,
                            ModPath = folder,
                            ModId = file.publishedfileid
                        });
                    }

                    await downloadTabManager.DownloadMods(mods);
                    mods.ForEach(listDownloadedObjects.UpdateObject);
                    return;
                }

                SetDownloadButtonState(true);

                var details = await SteamWorkshop.GetWorkshopFileDetails(id);

                if (details == null)
                {
                    MessageBox.Show($"Request to get workshop file {id} details failed. See log for details. Try again?", "Uh oh",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                if (details.result != 1)
                {
                    MessageBox.Show("Invalid workshop item; Check log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Warning("User tried to download workshop mod but ID returned code {0}", details.result);
                    return;
                }

                string folderName = Utils.SanitizeFilename(details.title);

                Log.Information($"Trying to download a new mod by the name '{details.title}' with the id '{details.publishedfileid}' into folder '{folderName}'");

                string modPath = Path.Combine(modsPath, folderName);

                BaseMod mod = new BaseMod
                {
                    Details = details,
                    ModPath = modPath,
                    ModId = details.publishedfileid
                };

                await downloadTabManager.DownloadMod(mod);
                //await Downloader.DownloadWorkshopItem(mod, new Updater(gamePath));
                listDownloadedObjects.UpdateObject(mod);
            });
        }

        Regex regex = new Regex(@"[0-9]{2,15}");
        private void toggleBackupMods_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ShouldBackupMods = toggleBackupMods.Checked;
        }

        private void chromiumWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            _instance.Invoke((MethodInvoker)(() =>
           {
               if (!e.IsLoading) // Only change address bar when page actually loads; Otherwise you get a little weird behaviour when pasting links and such.
               {
                   textViewUrlBar.Text = chromiumWebBrowser.Address;

                   if (!textViewUrlBar.Enabled) // Enable URL bar if this is first load.
                   {
                       Log.Information("Browser page loaded. Enabling url bar");
                       textViewUrlBar.Enabled = true;
                   }


                   btnViewBack.Enabled = e.CanGoBack;
                   btnViewForward.Enabled = e.CanGoForward;
               }
           }));
        }

        private void textGamePath_TextChanged(object sender, EventArgs e)
        {
            if (currentUpdater != null)
            {
                Log.Information("Game path textbox changed. Changing currentUpdater path...");
                currentUpdater.SetGamePath(textGamePath.Text);
            }
        }

        private void downloadedModMenu_Opening(object sender, CancelEventArgs e)
        {
            var selectedRows = listDownloadedObjects.SelectedObjects;
            var menu = downloadedModMenu;
            menu.Items.Clear();

            if (selectedRows.Count == 1)
            {
                var mod = selectedRows[0] as BaseMod;

                menu.Items.Add("Open mod folder", null, (o, args) => Process.Start(Path.Combine(currentUpdater?.ModsPath ?? textGamePath.Text, mod.Folder)));
                menu.Items.Add("Open workshop page", null, (o, args) => Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=" + mod.ModId));
                menu.Items.Add("Open changelog page", null, (o, args) => Process.Start("https://steamcommunity.com/sharedfiles/filedetails/changelog/" + mod.ModId));
            }
        }

        private void useModifiedDates_CheckedChanged(object sender, EventArgs e)
        {
            //Settings.UseModifiedDate = useModifiedDates.Checked;
        }

        private void downloaderTabStatus_DoubleClick(object sender, EventArgs e)
        {
            Log.Information("Opening browser devtools");
            chromiumWebBrowser.ShowDevTools();
        }
    }
}
