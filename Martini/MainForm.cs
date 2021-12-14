using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Martini
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Utils.Try(CreateProfilesMenu);
            Utils.Try(CreateListView);
        }

        private void CreateProfilesMenu()
        {
            profilesMenu.DropDownItems.Clear();
            var filenames = Directory.EnumerateFiles(Environment.CurrentDirectory, "*.zip", SearchOption.TopDirectoryOnly);

            foreach (var filename in filenames)
            {
                var btn = new ToolStripButton(Path.GetFileNameWithoutExtension(filename).ToPascalCase());
                btn.Tag = filename;
                btn.Click += ReadProfileFromDisk;
                profilesMenu.DropDownItems.Add(btn);
            }

            profilesMenu.DropDownItems.Add(new ToolStripSeparator());

            var saveButton = new ToolStripButton("Save as...");
            saveButton.Click += OnSaveProfile;
            saveButton.Width = 100;
            profilesMenu.DropDownItems.Add(saveButton);
        }

        private void CreateListView()
        {
            var iniFiles = LoadMartiniFiles();
            listView.Items.Clear();
            foreach (var ini in iniFiles)
            {
                var item = new ListViewItem();
                item.Text = Path.GetFileNameWithoutExtension(ini.Filename);
                item.ImageIndex = 0;
                item.Tag = ini;
                listView.Items.Add(item);
            }
        }

        private void OnSaveProfile(object sender, EventArgs e)
        {
            var name = Interaction.InputBox("Profile name?", "Save profile as...");
            if (!string.IsNullOrEmpty(name))
                Utils.Try(() => WriteProfileToDisk($"{name}.zip"));
        }

        private void OnIniSelected(object sender, EventArgs e)
        {
            // Although we already have the ini, we reload it in case it has 
            // been manually changed... Otherwise we have to restart the
            // appplication, which is counter-intuitive.
            var filename = ((IniFile)listView.SelectedItems[0].Tag).Filename;
            var ini = new IniFile();
            if (Utils.Try(() => ini.Load(filename)))
            {
                using var w = new OptionsDialog(ini);
                if (w.ShowDialog(this) == DialogResult.OK)
                    Utils.Try(() => ini.Save());
            }
        }

        private static IEnumerable<IniFile> LoadMartiniFiles()
        {
            foreach (var filename in Directory.EnumerateFiles(".", "*.ini"))
            {
                var ini = new IniFile();
                ini.Load(filename);
                if (ini.HasSpec)
                    yield return ini;
            }
        }

        private void WriteProfileToDisk(string filename)
        {
            if (File.Exists(filename))
            {
                if (MessageBox.Show(@"File already exists, replace?", filename, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    File.Delete(filename);
                else
                    return;
            }

            using var zip = ZipFile.Open(filename, ZipArchiveMode.Update);

            foreach (ListViewItem item in listView.Items)
            {
                var ini = (IniFile)item.Tag;
                zip.CreateEntryFromFile(ini.Filename, Path.GetFileName(ini.Filename), CompressionLevel.Optimal);
            }
            CreateProfilesMenu();
        }

        private void ReadProfileFromDisk(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var filename = (string)button.Tag;
            using var zip = ZipFile.OpenRead(filename);
            foreach (var zipEntry in zip.Entries)
                Utils.Try(() => zipEntry.ExtractToFile(zipEntry.FullName, true));
            Utils.Try(CreateListView);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}