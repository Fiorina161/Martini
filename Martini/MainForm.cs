using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Martini.Properties;
using Microsoft.VisualBasic;
using ComboBox = System.Windows.Forms.ComboBox;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace Martini
{
    public partial class MainForm : Form
    {
        /**
		 * Add snapshots support.
		 */
        private class InputPayload
        {
            public IniKeyValue IniKeyValue;
            public Label Label;
        }

        private int _maxKeyWidth;
        private FileSystemWatcher _watcher;

        /**********************************************************************
		 * Constructor.
		 *********************************************************************/
        public MainForm()
        {
            InitializeComponent();
            InstallFileSystemWatcher();
        }

        /**********************************************************************
		 * Prevent panel flickering.
		 *********************************************************************/
        protected override CreateParams CreateParams
        {
            get
            {
                var handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000; // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        /**********************************************************************
		 * Initialize.
		 *********************************************************************/
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = $@"Martini configuration editor {Assembly.GetExecutingAssembly().GetName().Version}";
            BuildListView();
            BuildSnapshotMenu();
            editorPanel.VerticalScroll.Visible = true;
            saveFileButton.Enabled = false;
        }

        /**********************************************************************
		 * Creates snapshots menu from zip files found in current directory.
		 *********************************************************************/
        private void BuildSnapshotMenu()
        {
            snapshotsMenu.DropDownItems.Clear();
            var filenames = Directory.EnumerateFiles(Environment.CurrentDirectory, "*.zip", SearchOption.TopDirectoryOnly);

            foreach (var filename in filenames.OrderBy(x => x))
            {
                var btn = new ToolStripButton(Path.GetFileNameWithoutExtension(filename)) { Image = Resources.package_box };
                btn.Tag = filename;
                btn.Click += LoadSnapshot;
                snapshotsMenu.DropDownItems.Add(btn);
            }

            snapshotsMenu.DropDownItems.Add(new ToolStripSeparator());

            var saveSnapshotButton = new ToolStripButton("Save snapshot as...") { Image = Resources.package_box };
            saveSnapshotButton.Click += SaveSnapshot;

            snapshotsMenu.DropDownItems.Add(saveSnapshotButton);

            var openDirectoryButton = new ToolStripButton("Open in Explorer...") { Width = 150, Image = Resources.folder };
            openDirectoryButton.Click += (s, e) => Process.Start(".");
            snapshotsMenu.DropDownItems.Add(openDirectoryButton);
        }

        /**********************************************************************
		 * Saves current ini files into a zip file.
		 *********************************************************************/
        private void SaveSnapshot(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                CreatePrompt = true,
                OverwritePrompt = true,
                DefaultExt = "*.zip",
                AddExtension = true,
                CheckPathExists = true,
                Filter = @"Snapshots|*.zip"
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            var filename = dialog.FileName;
            if (File.Exists(filename))
                File.Delete(filename);

            using var zip = ZipFile.Open(filename, ZipArchiveMode.Update);
            foreach (ListViewItem item in listView.Items)
            {
                var ini = (IniFileContent)item.Tag;
                zip.CreateEntryFromFile(ini.FileName, Path.GetFileName(ini.FileName), CompressionLevel.Optimal);
            }
            BuildSnapshotMenu();
        }

        /**********************************************************************
		 * Loads snapshots zip file and replaces current files with content.
		 *********************************************************************/
        private void LoadSnapshot(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var filename = (string)button.Tag;
            using var zip = ZipFile.OpenRead(filename);
            foreach (var zipEntry in zip.Entries)
                Utils.Try(() => zipEntry.ExtractToFile(zipEntry.FullName, true));
            BuildListView();
            ClearEditor();
        }

        /**********************************************************************
		 * Load supported ini files from disk and populate ListView.
		 *********************************************************************/
        private void BuildListView()
        {
            listView.Items.Clear();
            foreach (var filename in Directory.EnumerateFiles(".", "*.ini"))
            {
                var iniFileData = new IniFileContent();
                iniFileData.Load(filename);
                if (!iniFileData.IsSupported)
                    continue;

                listView.Items.Add(new ListViewItem
                {
                    Text = Path.GetFileNameWithoutExtension(filename),
                    Tag = iniFileData,
                    ImageIndex = 0
                });
            }
        }

        /**********************************************************************
		 * Toggle between Icons and List view.
		 *********************************************************************/
        private void ToggleListViewStyle()
        {
            listView.View = listView.View == View.LargeIcon ? View.List : View.LargeIcon;
        }

        /**********************************************************************
		 * An ini file has been (de)selected in the ListView, update editor
		 * accordingly.
		 *********************************************************************/
        private void OnListViewSelection(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                ClearEditor();
            }
            else
            {
                var listViewSelectedItem = listView.SelectedItems[0];
                var iniFileData = listViewSelectedItem.Tag as IniFileContent;
                var fileName = iniFileData?.FileName;
                currentFileLabel.Text = $@"  {Path.GetFileName(fileName)}  ";
                currentFileLabel.Tag = iniFileData;
                buttonPanel.Visible = true;
                BuildValuesEditor(iniFileData);
            }
        }

        /**********************************************************************
		 * Clears editor (used when no file is selected).
		 *********************************************************************/
        private void ClearEditor()
        {
            buttonPanel.Visible = false;
            currentFileLabel.Text = "";
            editorPanel.Controls.Clear();
        }

        /**********************************************************************
		 * Build editor panel according to given ini file data.
		 *********************************************************************/
        private void BuildValuesEditor(IniFileContent iniFileContent)
        {
            _maxKeyWidth = GetMaxLabelWidth(iniFileContent);
            var y = 10;
            var x = 10;

            editorPanel.Controls.Clear();

            foreach (var sectionName in iniFileContent.GetSectionNames())
            {
                if (!string.IsNullOrEmpty(sectionName))
                {
                    y += 3;
                    var sectionLabel = CreateSectionLabel(x, y, sectionName);
                    editorPanel.Controls.Add(sectionLabel);
                    y += sectionLabel.Height + 10;
                }

                foreach (var iniFileEntry in iniFileContent.GetIniEntries(sectionName))
                {
                    var keyLabel = CreateKeyLabel(x, y + 3, iniFileEntry);
                    editorPanel.Controls.Add(keyLabel);

                    var payload = new InputPayload { IniKeyValue = iniFileEntry, Label = keyLabel };
                    var valueControl = CreateValueControl(x + _maxKeyWidth, y, payload);
                    editorPanel.Controls.Add(valueControl);

                    y += valueControl.Height + 12;
                }
            }
            editorPanel.Controls.Add(new Label { Top = y, Height = 12 }); // Bottom padding

            UpdateValueControlsSize();
        }

        /**********************************************************************
		 * Returns key label for given ini entry.
		 *********************************************************************/
        private Label CreateKeyLabel(int x, int y, IniKeyValue iniKeyValue)
        {
            var label = new Label
            {
                Text = iniKeyValue.Key.Replace("_", " "),
                Left = x,
                Top = y,
                AutoSize = true,
                Font = iniKeyValue.DefaultValue == iniKeyValue.CurrentValue ? Font : new Font(Font, FontStyle.Bold)
            };

            toolTip1.SetToolTip(label, $"{iniKeyValue.Note} [default='{iniKeyValue.DefaultValue}']");
            return label;
        }

        /**********************************************************************
		 * Returns input control for given entry, either one of TextBox,
		 * CheckBox or ComboBox.
		 *********************************************************************/
        private Control CreateValueControl(int x, int y, InputPayload payload)
        {
            var entry = payload.IniKeyValue;

            if (entry.IsBoolean)
            {
                var checkbox = new CheckBox
                {
                    Top = y,
                    Left = x,
                    Checked = entry.CurrentValue == "true",
                    Tag = payload
                };
                checkbox.CheckedChanged += OnControlValueChanged;
                return checkbox;
            }

            if (entry.IsEnumeration)
            {
                var combobox = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Top = y,
                    Left = x,
                    Tag = payload
                };
                foreach (var v in entry.AllowedValues)
                {
                    combobox.Items.Add(v);
                    if (v == entry.CurrentValue)
                        combobox.SelectedItem = v;
                }
                combobox.SelectedIndexChanged += OnControlValueChanged;
                return combobox;
            }

            var textbox = new TextBox
            {
                Top = y,
                Left = x,
                Text = entry.CurrentValue,
                Tag = payload
            };
            textbox.TextChanged += OnControlValueChanged;
            return textbox;
        }

        /**********************************************************************
		 * Sets label as bold if current ini value is
		 * different than its default.
		 *********************************************************************/
        private void OnControlValueChanged(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var payload = (InputPayload)control.Tag;
            var defaultValue = payload.IniKeyValue.DefaultValue;

            var isDefault = control switch
            {
                CheckBox ctrl => (ctrl.Checked ? "true" : "false") == defaultValue,
                TextBox ctrl => ctrl.Text.Trim() == defaultValue,
                ComboBox ctrl => ctrl.Text.Trim() == defaultValue,
                _ => true
            };
            payload.Label.Font = new Font(Font, isDefault ? FontStyle.Regular : FontStyle.Bold);
            saveFileButton.Enabled = true;
        }

        /**********************************************************************
		 * Returns label for given section name.
		 *********************************************************************/
        private Control CreateSectionLabel(int x, int y, string sectionName)
        {
            return new Label
            {
                Text = sectionName.ToUpper(),
                Left = x,
                Top = y,
                AutoSize = true,
                Font = new Font(Font, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 100, 200),
                Padding = new Padding(5, 3, 5, 3)
            };
        }

        /**********************************************************************
		 * Computes maximum lenght of keys in ini file for alignment.		 
		 *********************************************************************/
        private int GetMaxLabelWidth(IniFileContent iniFileContent)
        {
            var max = 0;
            var font = new Font(Font, FontStyle.Bold);
            foreach (var section in iniFileContent.GetSectionNames())
                foreach (var entry in iniFileContent.GetIniEntries(section))
                {
                    var m = TextRenderer.MeasureText(entry.Key, font);
                    if (m.Width > max)
                        max = m.Width;
                }
            return max;
        }

        /**********************************************************************
		 * We can't use anchors to automatically resize input controls because
		 * it doesn't work properly for controls not visible if a scrollbar is
		 * present. Thus, we redraw manually on form resize.
		 *********************************************************************/
        private void UpdateValueControlsSize()
        {
            if (_maxKeyWidth > 0)
            {
                var w = editorPanel.Width - _maxKeyWidth - 30 - (editorPanel.VerticalScroll.Visible ? 20 : 0);
                SuspendLayout();
                foreach (var control in editorPanel.Controls)
                {
                    switch (control)
                    {
                        case TextBox textbox:
                            textbox.Width = w;
                            break;
                        case ComboBox combobox:
                            combobox.Width = w;
                            break;
                    }
                }
                ResumeLayout();
            }
        }

        /**********************************************************************
		 * Open current file in external editor.
		 *********************************************************************/
        private void CurrentFileLabel_Click(object sender, EventArgs e)
        {
            var iniFileData = (IniFileContent)currentFileLabel.Tag;
            Process.Start(iniFileData.FileName);
        }

        /**********************************************************************
		 * Mimic link when mouse hovers the current filename.
		 *********************************************************************/
        private void CurrentFileLabel_MouseEnter(object sender, EventArgs e)
        {
            currentFileLabel.Font = new Font(Font, FontStyle.Underline);
        }

        /**********************************************************************
		 * Remove underline when leaves the current filename.
		 *********************************************************************/
        private void CurrentFileLabel_MouseLeave(object sender, EventArgs e)
        {
            currentFileLabel.Font = new Font(Font, FontStyle.Regular);
        }

        /**********************************************************************
		 * Resets all values to defaults.
		 *********************************************************************/
        private void ResetButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in editorPanel.Controls)
            {
                var payload = (InputPayload)control.Tag;

                switch (control)
                {
                    case TextBox textbox:
                        textbox.Text = payload.IniKeyValue.DefaultValue;
                        break;
                    case ComboBox combobox:
                        combobox.SelectedItem = payload.IniKeyValue.DefaultValue;
                        break;
                    case CheckBox combobox:
                        combobox.Checked = payload.IniKeyValue.DefaultValue == "true";
                        break;
                }
            }
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            var iniFileData = (IniFileContent)currentFileLabel.Tag;

            foreach (Control control in editorPanel.Controls)
                if (control.Tag != null)
                {
                    var payload = (InputPayload)control.Tag;
                    var entry = payload.IniKeyValue;

                    iniFileData.GetIniEntry(entry.Section, entry.Key).CurrentValue = control switch
                    {
                        ComboBox ctrl => ctrl.SelectedItem.ToString(),
                        CheckBox ctrl => ctrl.Checked ? "true" : "false",
                        TextBox ctrl => ctrl.Text,
                        _ => iniFileData.GetIniEntry(entry.Section, entry.Key).CurrentValue
                    };
                }
            Utils.Try(iniFileData.Save);
            saveFileButton.Enabled = false;
        }

        private void EditorPanel_Resize(object sender, EventArgs e)
        {
            UpdateValueControlsSize();
        }

        private void InstallFileSystemWatcher()
        {
            _watcher = new FileSystemWatcher(Environment.CurrentDirectory);
            _watcher.IncludeSubdirectories = false;
            _watcher.Changed += OnFileOnDiskChanged;
            _watcher.Created += OnDirectoryChanged;
            _watcher.Deleted += OnDirectoryChanged;
            _watcher.Renamed += OnDirectoryChanged;
            _watcher.EnableRaisingEvents = true;
            FormClosed += (s, e) => _watcher.Dispose();
        }

        private void OnDirectoryChanged(object sender, FileSystemEventArgs e)
        {
            var currentFilename = currentFileLabel.Text.Trim();
            Invoke(new Action(BuildSnapshotMenu));
            Invoke(new Action(BuildListView));
            Invoke(new Action(() => SelectItem(currentFilename)));

        }

        private void OnFileOnDiskChanged(object sender, FileSystemEventArgs e)
        {
            var currentFilename = currentFileLabel.Text.Trim();
            if (currentFilename == e.Name)
            {
                Invoke(new Action(BuildListView));
                Invoke(new Action(() => SelectItem(currentFilename)));
            }
        }

        private void SelectItem(string currentFilename)
        {
            foreach (ListViewItem item in listView.Items)
            {
                var data = (IniFileContent)item.Tag;
                var filename = Path.GetFileName(data.FileName);
                if (filename == currentFilename)
                {
                    item.Selected = true;
                    return;
                }
            }
            // No selected item... clear the editor panel...
            Invoke(new Action(ClearEditor));
        }

        private void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
                ToggleListViewStyle();
            else if (e.Control && e.KeyCode == Keys.S && saveFileButton.Enabled)
                saveFileButton.PerformClick();
            else if (e.KeyCode == Keys.F8)
                useDefaultValuesButton.PerformClick();

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