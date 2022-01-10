using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Martini.Properties;

namespace Martini
{
    internal class ApplicationForm : Form
    {
        private const string BASE_DIRECTORY = ".";

        private MenuStrip _formMenu;
        private StatusBar _statusBar;
        private SplitContainer _splitter;

        public IniFilesViewWidget IniFilesView => (IniFilesViewWidget)_splitter.Panel1.Controls[0];
        //public IniEditorWidget IniEditor => (IniEditorWidget)_splitter.Panel2.Controls[0];

        public ApplicationForm()
        {
            Width = 800;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;
            Padding = new Padding(2);
            Icon = Resources.inifile;
            KeyPreview = true;
            KeyDown += OnFormKeyDown;
            Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Text = $@"Martini configuration editor {Assembly.GetExecutingAssembly().GetName().Version}";
            Utils.Try(CreateSplitter);
            Utils.Try(CreateFormMenu);
            Utils.Try(CreateStatusBar);
        }

        private void CreateFormMenu()
        {
            _formMenu = new MenuStrip();
            _formMenu.Items.Add("Profiles");
            _formMenu.Items.Add("View");
            Controls.Add(_formMenu);
        }

        private void CreateSplitter()
        {
            _splitter = new SplitContainer
            {
                Parent = this,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                SplitterDistance = 175,
                Panel1MinSize = 100,
                Panel2MinSize = 100,
                SplitterWidth = 3,
                Panel1 = { Controls = { new IniFilesViewWidget(this, BASE_DIRECTORY, OnFileSelected) { Dock = DockStyle.Fill } } }
            };
            Controls.Add(_splitter);
        }



        private void CreateStatusBar()
        {
            _statusBar = new StatusBar
            {
                AutoSize = true,
                Parent = this,
                Dock = DockStyle.Bottom,
                SizingGrip = true
            };
            Controls.Add(_statusBar);
        }

        private void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
                IniFilesView.ToggleView();
        }

        private void OnFileSelected(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                _splitter.Panel2.Controls.Clear();
                _splitter.Panel2.Controls.Add(new IniEditorWidget(filename,_splitter.Panel2.Width) { Dock = DockStyle.Fill });
            }
            _statusBar.Text = Path.GetFileName(filename);
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