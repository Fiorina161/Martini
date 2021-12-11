using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CheckBox = System.Windows.Forms.CheckBox;
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
            Text = $@"{Path.GetFileNameWithoutExtension(_ini.Filename).ToPascalCase()} settings";

            foreach (var kvp1 in _ini.Help)
            {
                var sectionName = kvp1.Key;
                var isGlobalSection = string.IsNullOrWhiteSpace(sectionName);

                // Section title
                if (!isGlobalSection)
                {
                    LayoutPanel.Controls.Add(CreateSectionLabel(top, sectionName));
                    top += LblSection.Height + 10;
                }

                foreach (var kvp2 in kvp1.Value)
                {
                    var keyName = kvp2.Key;
                    var currentValue = _ini.GetValue(sectionName, keyName);
                    var defaultValue = kvp2.Value;

                    // Key
                    var label = CreateKeyLabel(top, keyName, currentValue != defaultValue);
                    LayoutPanel.Controls.Add(label);

                    // Value (textbox or checkbox)
                    var valueContext = new ValueContext(sectionName, keyName, defaultValue, label);
                    Control control;
                    if (defaultValue == "true" || defaultValue == "false")
                        control = CreateCheckbox(valueLeftPos + 10, top, currentValue == "true", valueContext);
                    else
                        control = CreateTextBox(valueLeftPos + 10, top, currentValue, valueContext);

                    LayoutPanel.Controls.Add(control);
                    top += TxtValue.Height + 5;
                }

                // Add extra padding between sections...
                if (!isGlobalSection)
                    top += LblSection.Height;
            }

            ResizeAndPlace(top);

            // Get rid of selected text on the first textbox...
            ActiveControl = LayoutPanel;
        }

        private void ResizeAndPlace(int top)
        {
            // Try to remove scrollbar without being taller than
            // the screen, center it all vertically.
            var bounds = Screen.GetWorkingArea(this);
            if (top < bounds.Height - 50)
            {
                Height = top;
                while (LayoutPanel.VerticalScroll.Visible)
                    Height++;
            }
            if (Bottom > bounds.Height)
                Top = (bounds.Height - Height) / 2;
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

        private Label CreateKeyLabel(int top, string text, bool modified)
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
            return label;
        }

        private Label CreateSectionLabel(int top, string text)
        {
            var label = new Label();
            label.Left = 0;
            label.Width = LblSection.Width;
            label.Anchor = LblSection.Anchor;
            label.Top = top + LayoutPanel.Padding.Top;
            label.Text = text.ToPascalCase();
            label.Font = LblSection.Font;
            label.ForeColor = LblSection.ForeColor;
            label.BackColor = LblSection.BackColor;
            label.Padding = LblSection.Padding;
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
                        case CheckBox ctrl:
                            _ini.SetValue(context.Section, context.Key, ctrl.Checked ? "true" : "false");
                            break;
                        case TextBox ctrl:
                            _ini.SetValue(context.Section, context.Key, ctrl.Text);
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
            // Find the largest label to display for a nice alignment.
            var font = new Font(LblKey.Font, FontStyle.Bold);
            var max = 0;
            foreach (var kvp1 in _ini.Help)
            {
                foreach (var kvp2 in kvp1.Value)
                {
                    var m = TextRenderer.MeasureText(kvp2.Key, font);
                    if (m.Width > max)
                        max = m.Width;
                }
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
            }
            context.KeyLabel.Font = new Font(context.KeyLabel.Font, sameAsDefault ? FontStyle.Regular : FontStyle.Bold);
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

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            // Ask OS to open the ini file on disk using the default ini editor program.
            System.Diagnostics.Process.Start(_ini.Filename);
        }
    }
}
