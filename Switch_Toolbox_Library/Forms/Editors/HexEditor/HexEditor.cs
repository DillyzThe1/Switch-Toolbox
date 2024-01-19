﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Windows.Forms;

namespace Toolbox.Library.Forms
{
    public partial class HexEditor : STUserControl
    {
        FindOptions _findOptions = new FindOptions();
        public Action<byte[]> onChanged;
        private byte[] funnyData = { };

        public HexEditor()
        {
            InitializeComponent();

            hexBox1.BackColor = FormThemes.BaseTheme.FormBackColor;
            hexBox1.ForeColor = FormThemes.BaseTheme.FormForeColor;
            hexBox1.SelectionBackColor = FormThemes.BaseTheme.FormContextMenuSelectColor;
            hexBox1.SelectionForeColor = FormThemes.BaseTheme.FormForeColor;
            fixedBytesToolStripMenuItem.Checked = true;
            hexBox1.UseFixedBytesPerLine = true;
        }

        public override void OnControlClosing()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            if (hexBox1.ByteProvider != null && !isStream)
            {
                funnyData = new byte[0]; 
                hexBox1.ByteProvider.DeleteBytes(0, hexBox1.ByteProvider.Length);

                IDisposable byteProvider = hexBox1.ByteProvider as IDisposable;
                if (byteProvider != null)
                    byteProvider.Dispose();
                hexBox1.ByteProvider = null;
            }
        }

        public bool EnableMenuBar
        {
            set
            {
                if (value)
                    stContextMenuStrip1.Show();
                else
                    stContextMenuStrip1.Hide();
            }
            get
            {
                return stContextMenuStrip1.Visible;
            }
        }

        private bool isStream = false;

        public void LoadData(System.IO.Stream data)
        {
            isStream = true;

            IByteProvider provider = new DynamicFileByteProvider(data);
            hexBox1.ByteProvider = provider;
        }

        public void LoadData(byte[] data)
        {
            isStream = false;

            Cleanup();

            funnyData = data;
            IByteProvider provider = new DynamicByteProvider(data);
            hexBox1.ByteProvider = provider;
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchHex searchHex = new SearchHex();
            searchHex.HexBox = hexBox1;
            searchHex.FindOptions = _findOptions;

            if (searchHex.ShowDialog() == DialogResult.OK)
            {
                FindOptions options = new FindOptions();
                options.MatchCase = searchHex.matchCase;
                options.Type = searchHex.findType;

                if (options.Type == FindType.Hex)
                    options.Hex = searchHex.findHex;
                else
                    options.Text = searchHex.findString;
            }
        }

        private void fixedBytesToolStripMenuItem_Click(object sender, EventArgs e) {
            hexBox1.UseFixedBytesPerLine = fixedBytesToolStripMenuItem.Checked;
            hexBox1.Refresh();
        }

        // https://stackoverflow.com/questions/9820165/convert-hexadecimal-string-to-its-numerical-values-in-c-sharp
        private void trimFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int trimLength = Convert.ToInt32("00002070", 16) + 0x10;
            byte[] oldFunnies = funnyData;

            funnyData = new byte[trimLength];
            Array.Copy(oldFunnies, funnyData, trimLength);

            if (onChanged != null)
                onChanged.Invoke(funnyData);

            hexBox1.ByteProvider.DeleteBytes(0, hexBox1.ByteProvider.Length);
            hexBox1.ByteProvider.InsertBytes(0, funnyData);
            hexBox1.Refresh();
        }
    }
}
