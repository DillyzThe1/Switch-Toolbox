﻿namespace LayoutBXLYT
{
    partial class LayoutEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutEditor));
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.backColorDisplay = new System.Windows.Forms.PictureBox();
            this.viewportBackColorCB = new Toolbox.Library.Forms.STComboBox();
            this.stToolStrip1 = new Toolbox.Library.Forms.STToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.stMenuStrip1 = new Toolbox.Library.Forms.STMenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textureListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textConverterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.backColorDisplay)).BeginInit();
            this.stToolStrip1.SuspendLayout();
            this.stMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 49);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(549, 349);
            this.dockPanel1.TabIndex = 6;
            this.dockPanel1.ActiveDocumentChanged += new System.EventHandler(this.dockPanel1_ActiveDocumentChanged);
            // 
            // backColorDisplay
            // 
            this.backColorDisplay.Location = new System.Drawing.Point(319, 25);
            this.backColorDisplay.Name = "backColorDisplay";
            this.backColorDisplay.Size = new System.Drawing.Size(21, 21);
            this.backColorDisplay.TabIndex = 10;
            this.backColorDisplay.TabStop = false;
            this.backColorDisplay.Click += new System.EventHandler(this.backColorDisplay_Click);
            // 
            // viewportBackColorCB
            // 
            this.viewportBackColorCB.BorderColor = System.Drawing.Color.Empty;
            this.viewportBackColorCB.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.viewportBackColorCB.ButtonColor = System.Drawing.Color.Empty;
            this.viewportBackColorCB.FormattingEnabled = true;
            this.viewportBackColorCB.IsReadOnly = false;
            this.viewportBackColorCB.Location = new System.Drawing.Point(167, 25);
            this.viewportBackColorCB.Name = "viewportBackColorCB";
            this.viewportBackColorCB.Size = new System.Drawing.Size(146, 21);
            this.viewportBackColorCB.TabIndex = 9;
            this.viewportBackColorCB.SelectedIndexChanged += new System.EventHandler(this.viewportBackColorCB_SelectedIndexChanged);
            this.viewportBackColorCB.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.stComboBox1_MouseDoubleClick);
            // 
            // stToolStrip1
            // 
            this.stToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.stToolStrip1.Location = new System.Drawing.Point(0, 24);
            this.stToolStrip1.Name = "stToolStrip1";
            this.stToolStrip1.Size = new System.Drawing.Size(549, 25);
            this.stToolStrip1.TabIndex = 3;
            this.stToolStrip1.Text = "stToolStrip1";
            this.stToolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.stToolStrip1_ItemClicked);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // stMenuStrip1
            // 
            this.stMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.stMenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.stMenuStrip1.Name = "stMenuStrip1";
            this.stMenuStrip1.Size = new System.Drawing.Size(549, 24);
            this.stMenuStrip1.TabIndex = 0;
            this.stMenuStrip1.Text = "stMenuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.clearWorkspaceToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // clearWorkspaceToolStripMenuItem
            // 
            this.clearWorkspaceToolStripMenuItem.Name = "clearWorkspaceToolStripMenuItem";
            this.clearWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearWorkspaceToolStripMenuItem.Text = "Clear Files";
            this.clearWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.clearWorkspaceToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save As";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textureListToolStripMenuItem,
            this.textConverterToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // textureListToolStripMenuItem
            // 
            this.textureListToolStripMenuItem.Name = "textureListToolStripMenuItem";
            this.textureListToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.textureListToolStripMenuItem.Text = "Texture List";
            this.textureListToolStripMenuItem.Click += new System.EventHandler(this.textureListToolStripMenuItem_Click);
            // 
            // textConverterToolStripMenuItem
            // 
            this.textConverterToolStripMenuItem.Name = "textConverterToolStripMenuItem";
            this.textConverterToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.textConverterToolStripMenuItem.Text = "Text Converter";
            this.textConverterToolStripMenuItem.Click += new System.EventHandler(this.textConverterToolStripMenuItem_Click);
            // 
            // LayoutEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 398);
            this.Controls.Add(this.backColorDisplay);
            this.Controls.Add(this.viewportBackColorCB);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.stToolStrip1);
            this.Controls.Add(this.stMenuStrip1);
            this.IsMdiContainer = true;
            this.Name = "LayoutEditor";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.LayoutEditor_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.LayoutEditor_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LayoutEditor_KeyDown);
            this.ParentChanged += new System.EventHandler(this.LayoutEditor_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.backColorDisplay)).EndInit();
            this.stToolStrip1.ResumeLayout(false);
            this.stToolStrip1.PerformLayout();
            this.stMenuStrip1.ResumeLayout(false);
            this.stMenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Toolbox.Library.Forms.STMenuStrip stMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private Toolbox.Library.Forms.STToolStrip stToolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textureListToolStripMenuItem;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private Toolbox.Library.Forms.STComboBox viewportBackColorCB;
        private System.Windows.Forms.PictureBox backColorDisplay;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textConverterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
    }
}
