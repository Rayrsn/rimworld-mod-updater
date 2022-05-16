namespace RimworldModUpdater
{
    partial class UpdaterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdaterForm));
            this.listObjectsParsed = new BrightIdeasSoftware.ObjectListView();
            this.folderName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.steamName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.createdTime = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.steamUpdateTime = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.downloadSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.updated = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.listMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnQueryFiles = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblObjectsParsed = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.textGamePath = new System.Windows.Forms.TextBox();
            this.btnOpenFileDialog = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnManualUpdate = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabUpdater = new System.Windows.Forms.TabPage();
            this.toggleBackupMods = new System.Windows.Forms.CheckBox();
            this.tabDownloader = new System.Windows.Forms.TabPage();
            this.downloaderTabStatus = new System.Windows.Forms.Label();
            this.downloaderTabProgressBar = new System.Windows.Forms.ProgressBar();
            this.chromiumWebBrowser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.btnViewForward = new System.Windows.Forms.Button();
            this.btnViewBack = new System.Windows.Forms.Button();
            this.btnViewDownloadMod = new System.Windows.Forms.Button();
            this.textViewUrlBar = new System.Windows.Forms.TextBox();
            this.tabDownloadedMods = new System.Windows.Forms.TabPage();
            this.listDownloadedObjects = new BrightIdeasSoftware.ObjectListView();
            this.modName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.downloadedFolderName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.modSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.postedDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.updatedDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.downloadedModMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.listObjectsParsed)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabUpdater.SuspendLayout();
            this.tabDownloader.SuspendLayout();
            this.tabDownloadedMods.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listDownloadedObjects)).BeginInit();
            this.SuspendLayout();
            // 
            // listObjectsParsed
            // 
            this.listObjectsParsed.AllColumns.Add(this.folderName);
            this.listObjectsParsed.AllColumns.Add(this.steamName);
            this.listObjectsParsed.AllColumns.Add(this.createdTime);
            this.listObjectsParsed.AllColumns.Add(this.steamUpdateTime);
            this.listObjectsParsed.AllColumns.Add(this.downloadSize);
            this.listObjectsParsed.AllColumns.Add(this.updated);
            this.listObjectsParsed.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listObjectsParsed.CellEditUseWholeCell = false;
            this.listObjectsParsed.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.folderName,
            this.steamName,
            this.createdTime,
            this.steamUpdateTime,
            this.downloadSize,
            this.updated});
            this.listObjectsParsed.ContextMenuStrip = this.listMenu;
            this.listObjectsParsed.Cursor = System.Windows.Forms.Cursors.Default;
            this.listObjectsParsed.FullRowSelect = true;
            this.listObjectsParsed.HideSelection = false;
            this.listObjectsParsed.Location = new System.Drawing.Point(0, 32);
            this.listObjectsParsed.Name = "listObjectsParsed";
            this.listObjectsParsed.SelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.listObjectsParsed.ShowCommandMenuOnRightClick = true;
            this.listObjectsParsed.ShowGroups = false;
            this.listObjectsParsed.Size = new System.Drawing.Size(999, 441);
            this.listObjectsParsed.TabIndex = 0;
            this.listObjectsParsed.UseCompatibleStateImageBehavior = false;
            this.listObjectsParsed.View = System.Windows.Forms.View.Details;
            // 
            // folderName
            // 
            this.folderName.AspectName = "Folder";
            this.folderName.Tag = "";
            this.folderName.Text = "Folder";
            this.folderName.ToolTipText = "The name of the folder the mod is in.";
            this.folderName.Width = 185;
            // 
            // steamName
            // 
            this.steamName.AspectName = "Title";
            this.steamName.Text = "Mod Name";
            this.steamName.ToolTipText = "The name of this mod on the workshop.";
            this.steamName.Width = 184;
            // 
            // createdTime
            // 
            this.createdTime.AspectName = "LocalCreatedTime";
            this.createdTime.Text = "Local Version";
            this.createdTime.ToolTipText = "The time this mod\'s files were moved into your mods folder.";
            this.createdTime.Width = 118;
            // 
            // steamUpdateTime
            // 
            this.steamUpdateTime.AspectName = "RemoteUpdatedTime";
            this.steamUpdateTime.Text = "Steam Version";
            this.steamUpdateTime.ToolTipText = "The last time this mod was updated on the workshop.";
            this.steamUpdateTime.Width = 132;
            // 
            // downloadSize
            // 
            this.downloadSize.AspectName = "Size";
            this.downloadSize.AspectToStringFormat = "{0} MB";
            this.downloadSize.Text = "Download Size";
            this.downloadSize.ToolTipText = "The size of the mod on the workshop.";
            this.downloadSize.Width = 97;
            // 
            // updated
            // 
            this.updated.AspectName = "Updated";
            this.updated.Text = "Updated";
            this.updated.ToolTipText = "Whether the mod has been updated or not.";
            this.updated.Width = 77;
            // 
            // listMenu
            // 
            this.listMenu.Name = "listMenu";
            this.listMenu.Size = new System.Drawing.Size(61, 4);
            this.listMenu.Opening += new System.ComponentModel.CancelEventHandler(this.listMenu_Opening);
            // 
            // btnQueryFiles
            // 
            this.btnQueryFiles.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnQueryFiles.Location = new System.Drawing.Point(443, 500);
            this.btnQueryFiles.Name = "btnQueryFiles";
            this.btnQueryFiles.Size = new System.Drawing.Size(106, 23);
            this.btnQueryFiles.TabIndex = 2;
            this.btnQueryFiles.Text = "Query Mod Files";
            this.btnQueryFiles.UseVisualStyleBackColor = true;
            this.btnQueryFiles.Click += new System.EventHandler(this.btnQueryFiles_Click);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(3, 529);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(999, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 3;
            // 
            // lblObjectsParsed
            // 
            this.lblObjectsParsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblObjectsParsed.AutoSize = true;
            this.lblObjectsParsed.Location = new System.Drawing.Point(3, 480);
            this.lblObjectsParsed.Name = "lblObjectsParsed";
            this.lblObjectsParsed.Size = new System.Drawing.Size(153, 13);
            this.lblObjectsParsed.TabIndex = 4;
            this.lblObjectsParsed.Text = "Mods with updates available: 0";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(3, 512);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(67, 17);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Waiting...";
            // 
            // textGamePath
            // 
            this.textGamePath.Location = new System.Drawing.Point(0, 6);
            this.textGamePath.Name = "textGamePath";
            this.textGamePath.Size = new System.Drawing.Size(278, 20);
            this.textGamePath.TabIndex = 6;
            this.textGamePath.TextChanged += new System.EventHandler(this.textGamePath_TextChanged);
            // 
            // btnOpenFileDialog
            // 
            this.btnOpenFileDialog.Location = new System.Drawing.Point(284, 3);
            this.btnOpenFileDialog.Name = "btnOpenFileDialog";
            this.btnOpenFileDialog.Size = new System.Drawing.Size(122, 23);
            this.btnOpenFileDialog.TabIndex = 7;
            this.btnOpenFileDialog.Text = "Open Rimworld Folder";
            this.btnOpenFileDialog.UseVisualStyleBackColor = true;
            this.btnOpenFileDialog.Click += new System.EventHandler(this.btnOpenFileDialog_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(924, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnManualUpdate
            // 
            this.btnManualUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManualUpdate.Location = new System.Drawing.Point(829, 3);
            this.btnManualUpdate.Name = "btnManualUpdate";
            this.btnManualUpdate.Size = new System.Drawing.Size(89, 23);
            this.btnManualUpdate.TabIndex = 9;
            this.btnManualUpdate.Text = "Manual Update";
            this.btnManualUpdate.UseVisualStyleBackColor = true;
            this.btnManualUpdate.Click += new System.EventHandler(this.btnManualUpdate_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabUpdater);
            this.tabControl.Controls.Add(this.tabDownloader);
            this.tabControl.Controls.Add(this.tabDownloadedMods);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1013, 581);
            this.tabControl.TabIndex = 10;
            // 
            // tabUpdater
            // 
            this.tabUpdater.Controls.Add(this.toggleBackupMods);
            this.tabUpdater.Controls.Add(this.btnQueryFiles);
            this.tabUpdater.Controls.Add(this.listObjectsParsed);
            this.tabUpdater.Controls.Add(this.lblStatus);
            this.tabUpdater.Controls.Add(this.textGamePath);
            this.tabUpdater.Controls.Add(this.lblObjectsParsed);
            this.tabUpdater.Controls.Add(this.btnReset);
            this.tabUpdater.Controls.Add(this.btnManualUpdate);
            this.tabUpdater.Controls.Add(this.btnOpenFileDialog);
            this.tabUpdater.Controls.Add(this.progressBar);
            this.tabUpdater.Location = new System.Drawing.Point(4, 22);
            this.tabUpdater.Name = "tabUpdater";
            this.tabUpdater.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdater.Size = new System.Drawing.Size(1005, 555);
            this.tabUpdater.TabIndex = 0;
            this.tabUpdater.Text = "Updater";
            this.tabUpdater.UseVisualStyleBackColor = true;
            // 
            // toggleBackupMods
            // 
            this.toggleBackupMods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleBackupMods.AutoSize = true;
            this.toggleBackupMods.Checked = true;
            this.toggleBackupMods.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toggleBackupMods.Location = new System.Drawing.Point(864, 479);
            this.toggleBackupMods.Name = "toggleBackupMods";
            this.toggleBackupMods.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toggleBackupMods.Size = new System.Drawing.Size(133, 17);
            this.toggleBackupMods.TabIndex = 10;
            this.toggleBackupMods.Text = "Backup updated mods";
            this.toggleBackupMods.UseVisualStyleBackColor = true;
            this.toggleBackupMods.CheckedChanged += new System.EventHandler(this.toggleBackupMods_CheckedChanged);
            // 
            // tabDownloader
            // 
            this.tabDownloader.Controls.Add(this.downloaderTabStatus);
            this.tabDownloader.Controls.Add(this.downloaderTabProgressBar);
            this.tabDownloader.Controls.Add(this.chromiumWebBrowser);
            this.tabDownloader.Controls.Add(this.btnViewForward);
            this.tabDownloader.Controls.Add(this.btnViewBack);
            this.tabDownloader.Controls.Add(this.btnViewDownloadMod);
            this.tabDownloader.Controls.Add(this.textViewUrlBar);
            this.tabDownloader.Location = new System.Drawing.Point(4, 22);
            this.tabDownloader.Name = "tabDownloader";
            this.tabDownloader.Padding = new System.Windows.Forms.Padding(3);
            this.tabDownloader.Size = new System.Drawing.Size(1005, 555);
            this.tabDownloader.TabIndex = 1;
            this.tabDownloader.Text = "Downloader";
            this.tabDownloader.UseVisualStyleBackColor = true;
            // 
            // downloaderTabStatus
            // 
            this.downloaderTabStatus.AutoSize = true;
            this.downloaderTabStatus.BackColor = System.Drawing.Color.Transparent;
            this.downloaderTabStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.downloaderTabStatus.Location = new System.Drawing.Point(3, 515);
            this.downloaderTabStatus.Name = "downloaderTabStatus";
            this.downloaderTabStatus.Size = new System.Drawing.Size(52, 13);
            this.downloaderTabStatus.TabIndex = 12;
            this.downloaderTabStatus.Text = "Waiting...";
            this.downloaderTabStatus.DoubleClick += new System.EventHandler(this.downloaderTabStatus_DoubleClick);
            // 
            // downloaderTabProgressBar
            // 
            this.downloaderTabProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.downloaderTabProgressBar.Location = new System.Drawing.Point(3, 528);
            this.downloaderTabProgressBar.MarqueeAnimationSpeed = 50;
            this.downloaderTabProgressBar.Name = "downloaderTabProgressBar";
            this.downloaderTabProgressBar.Size = new System.Drawing.Size(999, 24);
            this.downloaderTabProgressBar.TabIndex = 11;
            // 
            // chromiumWebBrowser
            // 
            this.chromiumWebBrowser.ActivateBrowserOnCreation = false;
            this.chromiumWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chromiumWebBrowser.Location = new System.Drawing.Point(3, 46);
            this.chromiumWebBrowser.Name = "chromiumWebBrowser";
            this.chromiumWebBrowser.Size = new System.Drawing.Size(999, 466);
            this.chromiumWebBrowser.TabIndex = 6;
            this.chromiumWebBrowser.LoadingStateChanged += new System.EventHandler<CefSharp.LoadingStateChangedEventArgs>(this.chromiumWebBrowser_LoadingStateChanged);
            // 
            // btnViewForward
            // 
            this.btnViewForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewForward.Enabled = false;
            this.btnViewForward.Location = new System.Drawing.Point(974, 3);
            this.btnViewForward.Name = "btnViewForward";
            this.btnViewForward.Size = new System.Drawing.Size(28, 20);
            this.btnViewForward.TabIndex = 5;
            this.btnViewForward.Text = "→";
            this.btnViewForward.UseVisualStyleBackColor = true;
            this.btnViewForward.Click += new System.EventHandler(this.btnViewForward_Click);
            // 
            // btnViewBack
            // 
            this.btnViewBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewBack.Enabled = false;
            this.btnViewBack.Location = new System.Drawing.Point(949, 3);
            this.btnViewBack.Name = "btnViewBack";
            this.btnViewBack.Size = new System.Drawing.Size(28, 20);
            this.btnViewBack.TabIndex = 4;
            this.btnViewBack.Text = "←";
            this.btnViewBack.UseVisualStyleBackColor = true;
            this.btnViewBack.Click += new System.EventHandler(this.btnViewBack_Click);
            // 
            // btnViewDownloadMod
            // 
            this.btnViewDownloadMod.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnViewDownloadMod.Location = new System.Drawing.Point(3, 23);
            this.btnViewDownloadMod.Name = "btnViewDownloadMod";
            this.btnViewDownloadMod.Size = new System.Drawing.Size(999, 23);
            this.btnViewDownloadMod.TabIndex = 2;
            this.btnViewDownloadMod.Text = "Download this mod/collection";
            this.btnViewDownloadMod.UseVisualStyleBackColor = true;
            this.btnViewDownloadMod.Click += new System.EventHandler(this.btnViewDownloadMod_Click);
            // 
            // textViewUrlBar
            // 
            this.textViewUrlBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.textViewUrlBar.Location = new System.Drawing.Point(3, 3);
            this.textViewUrlBar.Name = "textViewUrlBar";
            this.textViewUrlBar.Size = new System.Drawing.Size(999, 20);
            this.textViewUrlBar.TabIndex = 1;
            this.textViewUrlBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textViewUrlBar_KeyDown);
            // 
            // tabDownloadedMods
            // 
            this.tabDownloadedMods.Controls.Add(this.listDownloadedObjects);
            this.tabDownloadedMods.Location = new System.Drawing.Point(4, 22);
            this.tabDownloadedMods.Name = "tabDownloadedMods";
            this.tabDownloadedMods.Padding = new System.Windows.Forms.Padding(3);
            this.tabDownloadedMods.Size = new System.Drawing.Size(1005, 555);
            this.tabDownloadedMods.TabIndex = 2;
            this.tabDownloadedMods.Text = "Downloaded Mods";
            this.tabDownloadedMods.UseVisualStyleBackColor = true;
            // 
            // listDownloadedObjects
            // 
            this.listDownloadedObjects.AllColumns.Add(this.modName);
            this.listDownloadedObjects.AllColumns.Add(this.downloadedFolderName);
            this.listDownloadedObjects.AllColumns.Add(this.modSize);
            this.listDownloadedObjects.AllColumns.Add(this.postedDate);
            this.listDownloadedObjects.AllColumns.Add(this.updatedDate);
            this.listDownloadedObjects.CellEditUseWholeCell = false;
            this.listDownloadedObjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.modName,
            this.downloadedFolderName,
            this.modSize,
            this.postedDate,
            this.updatedDate});
            this.listDownloadedObjects.ContextMenuStrip = this.downloadedModMenu;
            this.listDownloadedObjects.Cursor = System.Windows.Forms.Cursors.Default;
            this.listDownloadedObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listDownloadedObjects.FullRowSelect = true;
            this.listDownloadedObjects.HideSelection = false;
            this.listDownloadedObjects.Location = new System.Drawing.Point(3, 3);
            this.listDownloadedObjects.MultiSelect = false;
            this.listDownloadedObjects.Name = "listDownloadedObjects";
            this.listDownloadedObjects.ShowCommandMenuOnRightClick = true;
            this.listDownloadedObjects.ShowGroups = false;
            this.listDownloadedObjects.Size = new System.Drawing.Size(999, 549);
            this.listDownloadedObjects.TabIndex = 0;
            this.listDownloadedObjects.UseCompatibleStateImageBehavior = false;
            this.listDownloadedObjects.View = System.Windows.Forms.View.Details;
            // 
            // modName
            // 
            this.modName.AspectName = "Title";
            this.modName.Text = "Mod Name";
            this.modName.ToolTipText = "The name of the downloaded mod.";
            this.modName.Width = 221;
            // 
            // downloadedFolderName
            // 
            this.downloadedFolderName.AspectName = "Folder";
            this.downloadedFolderName.Text = "Folder";
            this.downloadedFolderName.ToolTipText = "The folder this mod has been downloaded into.";
            this.downloadedFolderName.Width = 250;
            // 
            // modSize
            // 
            this.modSize.AspectName = "Size";
            this.modSize.AspectToStringFormat = "{0} MB";
            this.modSize.Text = "Size";
            this.modSize.ToolTipText = "The size of the downloaded mod.";
            this.modSize.Width = 84;
            // 
            // postedDate
            // 
            this.postedDate.AspectName = "RemoteCreatedTime";
            this.postedDate.Text = "Posted";
            this.postedDate.ToolTipText = "The date this mod was posted on the workshop.";
            this.postedDate.Width = 223;
            // 
            // updatedDate
            // 
            this.updatedDate.AspectName = "RemoteUpdatedTime";
            this.updatedDate.Text = "Updated";
            this.updatedDate.ToolTipText = "The last time this mod was updated on the workshop.";
            this.updatedDate.Width = 218;
            // 
            // downloadedModMenu
            // 
            this.downloadedModMenu.Name = "downloadedModMenu";
            this.downloadedModMenu.Size = new System.Drawing.Size(61, 4);
            this.downloadedModMenu.Opening += new System.ComponentModel.CancelEventHandler(this.downloadedModMenu_Opening);
            // 
            // UpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 581);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "UpdaterForm";
            this.Text = "Rimworld Mod Updater";
            this.Load += new System.EventHandler(this.UpdaterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listObjectsParsed)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabUpdater.ResumeLayout(false);
            this.tabUpdater.PerformLayout();
            this.tabDownloader.ResumeLayout(false);
            this.tabDownloader.PerformLayout();
            this.tabDownloadedMods.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listDownloadedObjects)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public BrightIdeasSoftware.ObjectListView listObjectsParsed;
        private System.Windows.Forms.Button btnQueryFiles;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblObjectsParsed;
        private System.Windows.Forms.Label lblStatus;
        private BrightIdeasSoftware.OLVColumn folderName;
        private BrightIdeasSoftware.OLVColumn steamName;
        private BrightIdeasSoftware.OLVColumn createdTime;
        private BrightIdeasSoftware.OLVColumn steamUpdateTime;
        private System.Windows.Forms.ContextMenuStrip listMenu;
        private BrightIdeasSoftware.OLVColumn updated;
        private System.Windows.Forms.TextBox textGamePath;
        private System.Windows.Forms.Button btnOpenFileDialog;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnManualUpdate;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabUpdater;
        private System.Windows.Forms.TabPage tabDownloader;
        private System.Windows.Forms.TabPage tabDownloadedMods;
        private BrightIdeasSoftware.ObjectListView listDownloadedObjects;
        private BrightIdeasSoftware.OLVColumn modName;
        private BrightIdeasSoftware.OLVColumn postedDate;
        private BrightIdeasSoftware.OLVColumn updatedDate;
        private System.Windows.Forms.TextBox textViewUrlBar;
        private System.Windows.Forms.Button btnViewDownloadMod;
        private System.Windows.Forms.Button btnViewForward;
        private System.Windows.Forms.Button btnViewBack;
        private System.Windows.Forms.CheckBox toggleBackupMods;
        private BrightIdeasSoftware.OLVColumn downloadedFolderName;
        //private Microsoft.Toolkit.Forms.UI.Controls.WebViewCompatible webViewDownloader;
        private CefSharp.WinForms.ChromiumWebBrowser chromiumWebBrowser;
        public System.Windows.Forms.Label downloaderTabStatus;
        public System.Windows.Forms.ProgressBar downloaderTabProgressBar;
        private BrightIdeasSoftware.OLVColumn downloadSize;
        private System.Windows.Forms.ContextMenuStrip downloadedModMenu;
        private BrightIdeasSoftware.OLVColumn modSize;
    }
}

