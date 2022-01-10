using System;
using Martini.Properties;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Martini
{
    internal class IniFilesViewWidget : UserControl
    {
        private const string DEFAULT_IMAGE_KEY = "$";

        private readonly ImageList _largeImageList = new ImageList { ImageSize = new Size(32, 32) };
        private readonly ImageList _smallImageList = new ImageList { ImageSize = new Size(16, 16) };

        private readonly string _baseDirectory;
        private readonly ListView _listView;

        public IniFilesViewWidget(Control parent, string baseDirectory, Action<string> onSelect)
        {
	        Parent = parent;
            _baseDirectory = baseDirectory;
            
            // Create image list...
            var iconsDirectory = Path.Combine(_baseDirectory, "icons");
            _largeImageList.Images.Add(DEFAULT_IMAGE_KEY, Resources.inifile);
            _smallImageList.Images.Add(DEFAULT_IMAGE_KEY, Resources.inifile);

            foreach (var filename in Directory.EnumerateFiles(iconsDirectory))
            {
	            var image = Image.FromFile(filename);
	            var key = Path.GetFileNameWithoutExtension(filename);
	            _largeImageList.Images.Add(key, image);
	            _smallImageList.Images.Add(key, image);
            }

            // Create list view...
            SuspendLayout();
            _listView = new ListView
            {
	            Dock = DockStyle.Fill,
	            Parent = this.Parent,
	            Activation = ItemActivation.Standard,
	            Alignment = ListViewAlignment.Default,
	            AutoArrange = true,
	            AutoSize = true,
	            BorderStyle = BorderStyle.Fixed3D,
	            LargeImageList = _largeImageList,
	            Scrollable = true,
	            SmallImageList = _smallImageList,
	            Sorting = SortOrder.Ascending,
	            MultiSelect = false,
	            View = View.LargeIcon
            };
            _listView.SelectedIndexChanged += (s, e) =>
            {
	            var filename = _listView.SelectedItems.Count > 0 ? _listView.SelectedItems[0].Tag.ToString() : null;
	            onSelect(filename);
            };
            ResumeLayout(false);
            Controls.Add(_listView);
            Reload();
        }

        public void Reload()
        {
	        SuspendLayout();
            _listView.Clear();

            foreach (var filename in Directory.EnumerateFiles(_baseDirectory, "*.ini"))
            {
                var ini = new IniFile();
                ini.Load(filename);
                if (ini.IsSupported)
                {
                    var name = Path.GetFileNameWithoutExtension(filename);
                    var icon = _largeImageList.Images.ContainsKey(name) ? name : DEFAULT_IMAGE_KEY;
                    var item = new ListViewItem(name, icon);
                    item.Tag = filename;
                    _listView.Items.Add(item);
                }
            }
            ResumeLayout(false);
        }

        public void ToggleView()
        {
            _listView.View = _listView.View == View.LargeIcon ? View.List : View.LargeIcon;
        }
    }
}