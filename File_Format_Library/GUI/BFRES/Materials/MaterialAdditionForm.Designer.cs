namespace FirstPlugin.GUI.BFRES.Materials
{
    partial class MaterialAdditionForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.texName = new System.Windows.Forms.TextBox();
            this.matName = new System.Windows.Forms.Label();
            this.matSampler = new System.Windows.Forms.Label();
            this.texSampler = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(176, 115);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(256, 115);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // texName
            // 
            this.texName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.texName.ForeColor = System.Drawing.Color.White;
            this.texName.Location = new System.Drawing.Point(12, 33);
            this.texName.Name = "texName";
            this.texName.Size = new System.Drawing.Size(314, 20);
            this.texName.TabIndex = 4;
            this.texName.Text = "NewTexture";
            // 
            // matName
            // 
            this.matName.AutoSize = true;
            this.matName.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.matName.Location = new System.Drawing.Point(12, 17);
            this.matName.Name = "matName";
            this.matName.Size = new System.Drawing.Size(35, 13);
            this.matName.TabIndex = 7;
            this.matName.Text = "Name";
            // 
            // matSampler
            // 
            this.matSampler.AutoSize = true;
            this.matSampler.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.matSampler.Location = new System.Drawing.Point(12, 63);
            this.matSampler.Name = "matSampler";
            this.matSampler.Size = new System.Drawing.Size(35, 13);
            this.matSampler.TabIndex = 9;
            this.matSampler.Text = "Name";
            // 
            // texSampler
            // 
            this.texSampler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.texSampler.ForeColor = System.Drawing.Color.White;
            this.texSampler.Location = new System.Drawing.Point(12, 79);
            this.texSampler.Name = "texSampler";
            this.texSampler.Size = new System.Drawing.Size(314, 20);
            this.texSampler.TabIndex = 8;
            this.texSampler.Text = "_a0";
            // 
            // MaterialAdditionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(338, 150);
            this.Controls.Add(this.matSampler);
            this.Controls.Add(this.texSampler);
            this.Controls.Add(this.matName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.texName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MaterialAdditionForm";
            this.Text = "New Material Texture";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox texName;
        private System.Windows.Forms.Label matName;
        private System.Windows.Forms.Label matSampler;
        public System.Windows.Forms.TextBox texSampler;
    }
}