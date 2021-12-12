namespace Martini
{
    partial class OptionsDialog
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
            this.LayoutPanel = new System.Windows.Forms.Panel();
            this.LblSection = new System.Windows.Forms.Label();
            this.LblKey = new System.Windows.Forms.Label();
            this.TxtValue = new System.Windows.Forms.TextBox();
            this.BtnApply = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOpenFile = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.LayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayoutPanel.AutoScroll = true;
            this.LayoutPanel.Controls.Add(this.LblSection);
            this.LayoutPanel.Controls.Add(this.LblKey);
            this.LayoutPanel.Controls.Add(this.TxtValue);
            this.LayoutPanel.Location = new System.Drawing.Point(29, 28);
            this.LayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.LayoutPanel.Size = new System.Drawing.Size(391, 456);
            this.LayoutPanel.TabIndex = 0;
            // 
            // LblSection
            // 
            this.LblSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSection.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.LblSection.Location = new System.Drawing.Point(0, 12);
            this.LblSection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblSection.Name = "LblSection";
            this.LblSection.Size = new System.Drawing.Size(391, 28);
            this.LblSection.TabIndex = 3;
            this.LblSection.Text = "Section Label";
            // 
            // LblKey
            // 
            this.LblKey.AutoSize = true;
            this.LblKey.Location = new System.Drawing.Point(17, 70);
            this.LblKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblKey.Name = "LblKey";
            this.LblKey.Size = new System.Drawing.Size(67, 16);
            this.LblKey.TabIndex = 4;
            this.LblKey.Text = "Key Label";
            // 
            // TxtValue
            // 
            this.TxtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtValue.Location = new System.Drawing.Point(125, 66);
            this.TxtValue.Margin = new System.Windows.Forms.Padding(4);
            this.TxtValue.Name = "TxtValue";
            this.TxtValue.Size = new System.Drawing.Size(249, 22);
            this.TxtValue.TabIndex = 5;
            this.toolTip1.SetToolTip(this.TxtValue, "Badaboom\r\n");
            this.TxtValue.TextChanged += new System.EventHandler(this.OnControlValueChanged);
            // 
            // BtnApply
            // 
            this.BtnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnApply.Location = new System.Drawing.Point(196, 516);
            this.BtnApply.Margin = new System.Windows.Forms.Padding(4);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(100, 28);
            this.BtnApply.TabIndex = 1;
            this.BtnApply.Text = "Apply";
            this.BtnApply.UseVisualStyleBackColor = true;
            this.BtnApply.Click += new System.EventHandler(this.OnApplyButtonClick);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(304, 516);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(100, 28);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.OnCancelButtonClick);
            // 
            // BtnOpenFile
            // 
            this.BtnOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnOpenFile.AutoSize = true;
            this.BtnOpenFile.Location = new System.Drawing.Point(29, 516);
            this.BtnOpenFile.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOpenFile.Name = "BtnOpenFile";
            this.BtnOpenFile.Size = new System.Drawing.Size(94, 28);
            this.BtnOpenFile.TabIndex = 3;
            this.BtnOpenFile.Text = "Open file...";
            this.BtnOpenFile.UseVisualStyleBackColor = true;
            this.BtnOpenFile.Click += new System.EventHandler(this.BtnOpenFile_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.Yellow;
            this.toolTip1.ShowAlways = true;
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.BtnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(449, 573);
            this.Controls.Add(this.BtnOpenFile);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnApply);
            this.Controls.Add(this.LayoutPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(450, 450);
            this.Name = "OptionsDialog";
            this.Padding = new System.Windows.Forms.Padding(25);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "?";
            this.Load += new System.EventHandler(this.IniDialog_Load);
            this.LayoutPanel.ResumeLayout(false);
            this.LayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel LayoutPanel;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label LblSection;
        private System.Windows.Forms.Label LblKey;
        private System.Windows.Forms.TextBox TxtValue;
        private System.Windows.Forms.Button BtnOpenFile;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}