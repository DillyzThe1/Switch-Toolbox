namespace Toolbox.Library.Forms {
    partial class TeamColorSelector {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.rainbowColorCheck = new System.Windows.Forms.CheckBox();
            this.teamColorPicker = new Toolbox.Library.Forms.ColorSelector();
            this.SuspendLayout();
            // 
            // rainbowColorCheck
            // 
            this.rainbowColorCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rainbowColorCheck.AutoSize = true;
            this.rainbowColorCheck.Checked = true;
            this.rainbowColorCheck.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.rainbowColorCheck.Enabled = false;
            this.rainbowColorCheck.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.rainbowColorCheck.Location = new System.Drawing.Point(63, 11);
            this.rainbowColorCheck.Name = "rainbowColorCheck";
            this.rainbowColorCheck.Size = new System.Drawing.Size(137, 17);
            this.rainbowColorCheck.TabIndex = 0;
            this.rainbowColorCheck.Text = "Rainbow Color Enabled";
            this.rainbowColorCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rainbowColorCheck.UseVisualStyleBackColor = true;
            this.rainbowColorCheck.CheckedChanged += new System.EventHandler(this.rainbowColorCheck_CheckedChanged);
            // 
            // teamColorPicker
            // 
            this.teamColorPicker.Alpha = 0;
            this.teamColorPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.teamColorPicker.Color = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(255)))), ((int)(((byte)(80)))));
            this.teamColorPicker.DisplayAlpha = true;
            this.teamColorPicker.DisplayColor = true;
            this.teamColorPicker.Location = new System.Drawing.Point(12, 34);
            this.teamColorPicker.Name = "teamColorPicker";
            this.teamColorPicker.Size = new System.Drawing.Size(245, 243);
            this.teamColorPicker.TabIndex = 1;
            this.teamColorPicker.Paint += new System.Windows.Forms.PaintEventHandler(this.teamColorPicker_Paint);
            // 
            // TeamColorSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 286);
            this.Controls.Add(this.teamColorPicker);
            this.Controls.Add(this.rainbowColorCheck);
            this.Name = "TeamColorSelector";
            this.Text = "TeamColorSelector";
            this.Load += new System.EventHandler(this.TeamColorSelector_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox rainbowColorCheck;
        private ColorSelector teamColorPicker;
    }
}