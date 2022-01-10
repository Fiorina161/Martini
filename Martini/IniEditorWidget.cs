using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;
using CheckBox = System.Windows.Forms.CheckBox;
using ComboBox = System.Windows.Forms.ComboBox;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace Martini
{
    internal class IniEditorWidget : UserControl
    {
        private readonly string _filename;
        private readonly int _parentWidth;
        private readonly IniFile _iniFile;

        public IniEditorWidget(string filename, int parentWidth)
        {
            _filename = filename;
            _parentWidth = parentWidth;
            _iniFile = new IniFile();
            _iniFile.Load(filename);
        }

        protected override void OnLoad(EventArgs e)
        {
	        base.OnLoad(e);
	        CreateEditorPanel();
	        CreateButtonsPanel();
        }

        private void CreateEditorPanel()
        {
            var panel = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill
            };
            Controls.Add(panel);

            var keyLabelWidth = GetMaxLabelWidth();
            var y = 0;
            foreach (var sectionName in _iniFile.GetSectionNames())
                CreateSingleSection(panel, sectionName, keyLabelWidth, ref y);
        }

        private Panel CreateSingleSection(Panel panel, string sectionName, int keyLabelWidth, ref int y)
        {
            var textColor = Color.FromArgb(70, 70, 70);

            y += 5;
            if (!string.IsNullOrEmpty(sectionName))
            {
                var headerLabel = new Label
                {
                    Left = 0,
                    Top = y,
                    AutoSize = true,
                    Text = sectionName.ToUpper(),
                    Font = new Font("", 12, FontStyle.Bold),
                    ForeColor = textColor
                };
                panel.Controls.Add(headerLabel);
                y += headerLabel.Height + 10;
            }

            foreach (var iniEntry in _iniFile.GetIniEntries(sectionName))
            {
                var x = 40;
                var keyLabel = CreateKeyLabel(x, y + 4, iniEntry.Key, keyLabelWidth);
                panel.Controls.Add(keyLabel);

                x += keyLabelWidth + 10;
                var control = CreateValueControl(x, y, iniEntry, keyLabel);
                control.ForeColor = textColor;
                panel.Controls.Add(control);

                y += keyLabel.Height + 3;
            }

            return panel;
        }

        private Control CreateValueControl(int x, int y, IniFileEntry entry, Label keyLabel)
        {
            var tag = new ValueData(entry.Section, entry.Key, entry.DefaultValue, keyLabel);
            var w = _parentWidth/2;

            // Boolean
            if (entry.IsBoolean)
            {
                var checkBox = new CheckBox
                {
                    Top = y,
                    Left = x,
                    Checked = entry.CurrentOrDefault == "true",
                    Tag = tag
                };
                checkBox.CheckedChanged += OnControlValueChanged;

                return checkBox;
            }

            // Selection
            if (entry.IsEnumeration)
            {
                var comboBox = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Top = y,
                    Left = x,
                    Width = w,
                    Anchor = AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Top,
                    Tag = tag,
                };
                comboBox.SelectedIndexChanged += OnControlValueChanged;
                foreach (var value in entry.AllowedValues)
                {
                    comboBox.Items.Add(value);
                    if (value == entry.CurrentOrDefault)
                        comboBox.SelectedItem = value;
                }

                return comboBox;
            }

            // Text
            var textBox = new TextBox
            {
                Left = x,
                Top = y,
                Text = entry.CurrentOrDefault,
                Width = w,
                //Anchor = AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Top,
                AutoSize = true,
                Tag = tag
            };
            textBox.TextChanged += OnControlValueChanged;

            return textBox;
        }

        private void OnControlValueChanged(object sender, EventArgs e) { }

        private static Label CreateKeyLabel(int x, int y, string name, int width)
        {
            return new Label
            {
                Left = x,
                Top = y,
                Text = name,
                Width = width
            };
        }

        private void CreateButtonsPanel()
        {
            var editButton = new Button
            {
                Text = @"Edit file",
                AutoSize = true
            };
            editButton.Click += (s, e) => Process.Start(_filename);

            var saveButton = new Button
            {
                Text = @"Save",
                AutoSize = true
            };
            saveButton.Click += (s, e) => _iniFile.Save();

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Controls = { saveButton, editButton },
                BackColor = SystemColors.ControlLight
            };
            Controls.Add(layout);
        }

        private int GetMaxLabelWidth()
        {
            var max = 0;
            foreach (var section in _iniFile.GetSectionNames())
                foreach (var entry in _iniFile.GetIniEntries(section))
                {
                    var m = TextRenderer.MeasureText(entry.Key, Font);
                    if (m.Width > max)
                        max = m.Width;
                }

            return max;
        }

        private struct ValueData
        {
            public readonly string Section;
            public readonly string Key;
            public readonly string DefaultValue;
            public readonly Label KeyLabel;

            public ValueData(string section, string key, string defaultValue, Label keyLabel)
            {
                Section = section;
                Key = key;
                DefaultValue = defaultValue;
                KeyLabel = keyLabel;
            }
        }
    }
}