namespace Martini
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.snapshotsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleListViewStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.listView = new System.Windows.Forms.ListView();
            this.largeImageList = new System.Windows.Forms.ImageList(this.components);
            this.smallImageList = new System.Windows.Forms.ImageList(this.components);
            this.editorPanel = new System.Windows.Forms.Panel();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.useDefaultValuesButton = new System.Windows.Forms.Button();
            this.currentFileLabel = new System.Windows.Forms.Label();
            this.saveFileButton = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.snapshotsMenu,
            this.viewToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(10, 10);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(780, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // snapshotsMenu
            // 
            this.snapshotsMenu.Name = "snapshotsMenu";
            this.snapshotsMenu.Size = new System.Drawing.Size(73, 20);
            this.snapshotsMenu.Text = "Snapshots";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(12, 20);
            // 
            // toggleListViewStyle
            // 
            this.toggleListViewStyle.Name = "toggleListViewStyle";
            this.toggleListViewStyle.Size = new System.Drawing.Size(32, 19);
            // 
            // splitter
            // 
            this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitter.Location = new System.Drawing.Point(10, 10);
            this.splitter.Margin = new System.Windows.Forms.Padding(10);
            this.splitter.Name = "splitter";
            // 
            // splitter.Panel1
            // 
            this.splitter.Panel1.Controls.Add(this.listView);
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.AutoScroll = true;
            this.splitter.Panel2.Controls.Add(this.editorPanel);
            this.splitter.Panel2.Controls.Add(this.buttonPanel);
            this.splitter.Size = new System.Drawing.Size(760, 713);
            this.splitter.SplitterDistance = 214;
            this.splitter.TabIndex = 2;
            // 
            // listView
            // 
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listView.HideSelection = false;
            this.listView.LargeImageList = this.largeImageList;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.ShowGroups = false;
            this.listView.Size = new System.Drawing.Size(214, 713);
            this.listView.SmallImageList = this.smallImageList;
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.OnListViewSelection);
            // 
            // largeImageList
            // 
            this.largeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("largeImageList.ImageStream")));
            this.largeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.largeImageList.Images.SetKeyName(0, "ini.ico");
            // 
            // smallImageList
            // 
            this.smallImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallImageList.ImageStream")));
            this.smallImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.smallImageList.Images.SetKeyName(0, "ini.ico");
            // 
            // editorPanel
            // 
            this.editorPanel.AutoScroll = true;
            this.editorPanel.AutoSize = true;
            this.editorPanel.BackColor = System.Drawing.SystemColors.Control;
            this.editorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.editorPanel.Location = new System.Drawing.Point(0, 0);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(542, 680);
            this.editorPanel.TabIndex = 0;
            this.editorPanel.Resize += new System.EventHandler(this.EditorPanel_Resize);
            // 
            // buttonPanel
            // 
            this.buttonPanel.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPanel.Controls.Add(this.useDefaultValuesButton);
            this.buttonPanel.Controls.Add(this.currentFileLabel);
            this.buttonPanel.Controls.Add(this.saveFileButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 680);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(542, 33);
            this.buttonPanel.TabIndex = 0;
            this.buttonPanel.Visible = false;
            // 
            // useDefaultValuesButton
            // 
            this.useDefaultValuesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.useDefaultValuesButton.AutoSize = true;
            this.useDefaultValuesButton.Location = new System.Drawing.Point(332, 6);
            this.useDefaultValuesButton.Name = "useDefaultValuesButton";
            this.useDefaultValuesButton.Size = new System.Drawing.Size(126, 23);
            this.useDefaultValuesButton.TabIndex = 2;
            this.useDefaultValuesButton.Text = "[F8] Use default values";
            this.useDefaultValuesButton.UseVisualStyleBackColor = true;
            this.useDefaultValuesButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // currentFileLabel
            // 
            this.currentFileLabel.AutoSize = true;
            this.currentFileLabel.ForeColor = System.Drawing.Color.Blue;
            this.currentFileLabel.Location = new System.Drawing.Point(15, 11);
            this.currentFileLabel.Name = "currentFileLabel";
            this.currentFileLabel.Size = new System.Drawing.Size(35, 13);
            this.currentFileLabel.TabIndex = 0;
            this.currentFileLabel.Text = "label1";
            this.currentFileLabel.Click += new System.EventHandler(this.CurrentFileLabel_Click);
            this.currentFileLabel.MouseEnter += new System.EventHandler(this.CurrentFileLabel_MouseEnter);
            this.currentFileLabel.MouseLeave += new System.EventHandler(this.CurrentFileLabel_MouseLeave);
            // 
            // saveFileButton
            // 
            this.saveFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFileButton.Enabled = false;
            this.saveFileButton.Location = new System.Drawing.Point(464, 6);
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(75, 23);
            this.saveFileButton.TabIndex = 1;
            this.saveFileButton.Text = "&Save";
            this.saveFileButton.UseVisualStyleBackColor = true;
            this.saveFileButton.Click += new System.EventHandler(this.SaveFileButton_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.splitter);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(10, 34);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(10);
            this.mainPanel.Size = new System.Drawing.Size(780, 733);
            this.mainPanel.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 777);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.menuStrip);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ini File Manager";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnFormKeyDown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            this.splitter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
            this.splitter.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.SplitContainer splitter;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Panel editorPanel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.ImageList smallImageList;
        private System.Windows.Forms.ImageList largeImageList;
        private System.Windows.Forms.ToolStripMenuItem snapshotsMenu;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button saveFileButton;
        private System.Windows.Forms.ToolStripMenuItem toggleListViewStyle;
        private System.Windows.Forms.Label currentFileLabel;
        private System.Windows.Forms.Button useDefaultValuesButton;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}