using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using CheckBox = System.Windows.Forms.CheckBox;
using ComboBox = System.Windows.Forms.ComboBox;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace Martini
{
    public partial class OptionsDialog : Form
    {
        private readonly IniFile _ini;

        public OptionsDialog(IniFile ini)
        {
            _ini = ini;
            InitializeComponent();

            // We cannot remove existing controls from the layout panel because they will
            // lose their inherited properties, but we don't want to see them either.
            foreach (Control control in LayoutPanel.Controls)
                control.Visible = false;
        }

        private void IniDialog_Load(object sender, EventArgs e)
        {
            // Dynamically create the controls to read the ini values.
            // We render them in a panel so a scrollbar can be used for very long files.
            // After all controls are rendered we try to grow the form to avoid having a 
            // scroll bar if possible.
            // We use bold labels for values which are different from their default values.
            // Default values of true or false presume boolean keys and thus are rendered
            // using checkboxes.
            var top = 0;
            var valueLeftPos = LblKey.Left + GetMaxLabelWidth();

            // Window title
            Text = $@"{Path.GetFileNameWithoutExtension(_ini.FileName).ToPascalCase()} settings";



            foreach (var section in _ini.GetSectionNames())
            {
                var isGlobalSection = string.IsNullOrWhiteSpace(section);

                // Section title
                if (!isGlobalSection)
                {
                    LayoutPanel.Controls.Add(CreateSectionLabel(top, section));
                    top += LblSection.Height + 25;
                }

                foreach (var entry in _ini.GetIniEntries(section))
                {
                    if (!string.IsNullOrEmpty(entry.Note) && !entry.Note.EndsWith("."))
                        entry.Note += ".";
                    var tooltip = $"{entry.Note} Default=\"{entry.DefaultValue}\"";

                    // Key
                    var label = CreateKeyLabel(top+3, entry.Key, entry.HasChanged, tooltip);
                    LayoutPanel.Controls.Add(label);

                    // Value (textbox or checkbox)
                    var onClickData = new ValueContext(section, entry.Key, entry.DefaultValue, label);
                    Control control;
                    if (entry.IsBoolean)
                        control = CreateCheckbox(valueLeftPos + 10, top, entry.CurrentOrDefault == "true", onClickData);
                    else if (entry.IsEnumeration)
                        control = CreateComboBox(valueLeftPos + 10, top, entry.CurrentOrDefault, entry.AllowedValues, onClickData);
                    else
                        control = CreateTextBox(valueLeftPos + 10, top, entry.CurrentOrDefault, onClickData);

                    LayoutPanel.Controls.Add(control);
                    top += TxtValue.Height + 5;
                }
            }

            ResizeAndPlace(top);
            ActiveControl = LayoutPanel;
        }

        private void ResizeAndPlace(int top)
        {
            // Try to remove scrollbar without being taller than
            // the screen, center it all vertically.
            var screenHeight = Screen.GetWorkingArea(this).Height;
            if (top < screenHeight - 50)
            {
                Height = top;
                while (LayoutPanel.VerticalScroll.Visible && Height < screenHeight - 50)
                    Height += 10;
            }
            if (Bottom > screenHeight)
                Top = (screenHeight - Height) / 2;
        }

        private ComboBox CreateComboBox(int left, int top, string text, IEnumerable entryAllowedValues, ValueContext context)
        {
            var combobox = new ComboBox();
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.Font = TxtValue.Font;
            combobox.Top = top;
            combobox.Left = left;
            combobox.Tag = context;
            combobox.Width = LayoutPanel.ClientRectangle.Width - left - 20;
            combobox.SelectedIndexChanged += OnControlValueChanged;
            combobox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            combobox.Items.AddRange((object[])entryAllowedValues);
            foreach (var item in combobox.Items)
                if (item.ToString() == text)
                {
                    combobox.SelectedItem = item;
                    break;
                }
            return combobox;
        }

        private TextBox CreateTextBox(int left, int top, string text, ValueContext context)
        {
            var textbox = new TextBox();
            textbox.Font = TxtValue.Font;
            textbox.Top = top;
            textbox.Left = left;
            textbox.Text = text;
            textbox.Tag = context;
            textbox.Width = LayoutPanel.ClientRectangle.Width - left - 20;
            textbox.TextChanged += OnControlValueChanged;
            textbox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            return textbox;
        }

        private CheckBox CreateCheckbox(int left, int top, bool b, ValueContext context)
        {
            var checkbox = new CheckBox();
            checkbox.Top = top;
            checkbox.Left = left;
            checkbox.Checked = b;
            checkbox.Tag = context;
            checkbox.CheckedChanged += OnControlValueChanged;
            return checkbox;
        }

        private Label CreateKeyLabel(int top, string text, bool modified, string tooltip)
        {
            var label = new Label();
            label.Font = modified ? new Font(LblKey.Font, FontStyle.Bold) : LblKey.Font;
            label.Left = LblKey.Left;
            label.Top = top;
            label.Text = text.Replace("_", " ");
            label.ForeColor = LblKey.ForeColor;
            label.BackColor = LblKey.BackColor;
            label.Padding = LblKey.Padding;
            label.AutoSize = true;
            toolTip1.SetToolTip(label, tooltip);
            return label;
        }

        private Label CreateSectionLabel(int top, string text)
        {
            var label = new Label();
            label.Left = 0;
            label.Width = LblSection.Width;
            label.Anchor = LblSection.Anchor;
            label.Top = top + LayoutPanel.Padding.Top;
            label.Text = text.ToUpper();
            label.Font = LblSection.Font;
            label.ForeColor = LblSection.ForeColor;
            label.BackColor = LblSection.BackColor;
            label.Padding = LblSection.Padding;
            label.TextAlign = LblSection.TextAlign;
            return label;
        }

        private void OnApplyButtonClick(object sender, EventArgs e)
        {
            // Only copy control values into the ini object, we don't save on disk. 
            foreach (Control control in LayoutPanel.Controls)
                if (control.Tag != null)
                {
                    var context = (ValueContext)control.Tag;
                    switch (control)
                    {
                        case ComboBox ctrl:
                            _ini.GetIniEntry(context.Section, context.Key).CurrentValue = ctrl.SelectedItem.ToString();
                            break;
                        case CheckBox ctrl:
                            _ini.GetIniEntry(context.Section, context.Key).CurrentValue = ctrl.Checked ? "true" : "false";
                            break;
                        case TextBox ctrl:
                            _ini.GetIniEntry(context.Section, context.Key).CurrentValue = ctrl.Text;
                            break;
                    }
                }
            DialogResult = DialogResult.OK;
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private int GetMaxLabelWidth()
        {
            var max = 0;
            foreach (var section in _ini.GetSectionNames())
                foreach (var entry in _ini.GetIniEntries(section))
                {
                    var m = TextRenderer.MeasureText(entry.Key, LblKey.Font);
                    if (m.Width > max)
                        max = m.Width;
                }
            return max;
        }

        private void OnControlValueChanged(object sender, EventArgs e)
        {
            // Set label to bold if the control value is different than its default.
            var control = (Control)sender;
            var context = (ValueContext)control.Tag;
            var sameAsDefault = true;
            switch (control)
            {
                case CheckBox ctrl:
                    sameAsDefault = ctrl.Checked = ctrl.Checked && context.DefaultValue == "true";
                    break;
                case TextBox ctrl:
                    sameAsDefault = ctrl.Text.Trim() == context.DefaultValue;
                    break;
                case ComboBox ctrl:
                    sameAsDefault = ctrl.Text.Trim() == context.DefaultValue;
                    break;
            }
            context.KeyLabel.Font = new Font(context.KeyLabel.Font, sameAsDefault ? FontStyle.Regular : FontStyle.Bold);
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            // Open ini file using the default editor.
            Process.Start(_ini.FileName);
        }

        private struct ValueContext
        {
            // Attaches to the tag property of a value control.
            public readonly string Section;
            public readonly string Key;
            public readonly string DefaultValue;
            public readonly Label KeyLabel;
            public ValueContext(string section, string key, string defaultValue, Label keyLabel)
            {
                Section = section;
                Key = key;
                DefaultValue = defaultValue;
                KeyLabel = keyLabel;
            }
        }
    }
}
