using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toolbox.Library;
using Toolbox.Library.IO;
using Toolbox.Library.Forms;
using BarsLib;
using VGAudio.Formats;
using VGAudio;
using VGAudio.Containers.NintendoWare;
using VGAudio.Containers.Wave;
using NAudio.Wave;
using static FirstPlugin.BARS;
using DKCTF;
using static FirstPlugin.RenderInfoEnums;
using System.Security.Cryptography;
using static FirstPlugin.BarsZsFile;
using static FirstPlugin.GFPAK;

namespace FirstPlugin
{
    public class BARS_ZS : TreeNodeFile, IFileFormat, IContextMenuNode
    {
        public FileType FileType { get; set; } = FileType.Audio;

        public bool CanSave { get; set; }
        public string[] Description { get; set; } = new string[] { "Sound Archive" };
        public string[] Extension { get; set; } = new string[] { "*.bars.zs" };
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public IFileInfo IFileInfo { get; set; }

        public Type[] Types
        {
            get
            {
                List<Type> types = new List<Type>();
                return types.ToArray();
            }
        }

        public ToolStripItem[] GetContextMenuItems()
        {
            List<ToolStripItem> Items = new List<ToolStripItem>();

            Items.Add(new ToolStripMenuItem("Save", null, delegate(object sender, EventArgs args)
            {
                List<IFileFormat> formats = new List<IFileFormat>();

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = Utils.GetAllFilters(formats);
                sfd.FileName = FileName;
                if (sfd.ShowDialog() == DialogResult.OK)
                    STFileSaver.SaveFileFormat(this, sfd.FileName);
            }, Keys.Control | Keys.S));
            Items[0].Enabled = false;

            Items.Add(new ToolStripMenuItem("cry urself to sleep", null, delegate(object sender, EventArgs args) {
                MessageBox.Show("WAHHHHH WAHHHHHH", "Baby's First Switch Toolbox", MessageBoxButtons.OK);
            }, Keys.Control | Keys.C));

            return Items.ToArray();
        }

        private const int TargetBarsVersion = 258;//0x0201;

        public bool Identify(Stream stream)
        {
            using (var reader = new FileReader(stream, true))
            {
                bool sigCheck = reader.CheckSignature(4, "BARS");

                if (sigCheck)
                {
                    // https://wiki.oatmealdome.me/Aal/BARS_(File_Format)
                    reader.Position = 10;
                    short versionNum = reader.ReadInt16();
                    reader.Position = 0;
                    return versionNum == TargetBarsVersion;
                }

                return false;
            }
        }

        private class BARSv5Wrapper : TreeNodeCustom
        {
            public BARSv5Wrapper(AMTAv5 amta) { 
                MetaFile = amta;
                SelectedImageKey = ImageKey = "MetaInfo";
            }

            public AMTAv5 MetaFile { get; set; }

            public override void OnClick(TreeView treeview)
            {
                STPropertyGrid editor = (STPropertyGrid)LibraryGUI.GetActiveContent(typeof(STPropertyGrid));
                if (editor == null)
                {
                    editor = new STPropertyGrid();
                    LibraryGUI.LoadEditor(editor);
                }
                editor.Text = Text;
                editor.Dock = DockStyle.Fill;
                editor.LoadProperty(MetaFile, OnPropertyChanged);
            }

            private void OnPropertyChanged() { }
        }

        public BarsZsFile barsZs;
        public void Load(Stream stream)
        {
            CanSave = false;

            Text = FileName;

            Console.WriteLine("barzz");
            barsZs = new BarsZsFile(stream);


            //Console.WriteLine("helloo hi hi hiiii!!!!");
            //if (barsZs.HasMetaData)
            //    Nodes.Add("Meta Data");

            //AudioFolder folder = new AudioFolder("Audio");

            //if (barsZs.HasAudioFiles)
            //    Nodes.Add(folder);

            //TreeNode debugNode = null;
            //if (barsZs.HasDebugData)
            //    debugNode = Nodes.Add("Debug Data");

            Console.WriteLine("read " + barsZs.AudioEntries_ZS.Count + " bars lol");
            for (int i = 0; i < barsZs.AudioEntries_ZS.Count; i++)
            {
                AudioEntry_ZS entry = barsZs.AudioEntries_ZS[i];
                if (entry.MetaData == null)
                    continue;

                BARSv5Wrapper bars = new BARSv5Wrapper(entry.MetaData);
                bars.Text = entry.name;
                Nodes.Add(bars);

                if (entry.hasAudio)
                {
                    BARSAudioFileZS audio = entry.AudioFile;

                    AudioEntry node = new AudioEntry();
                    node.isZS = true;
                    node.audioFile_ZS = audio;
                    node.Magic = "BWAV";
                    node.SetupMusic();

                    node.Text = entry.name + ".bwav";

                    bars.Nodes.Add(node);
                }

                if (entry.debugData != null)
                {
                    BinEntry bin = new BinEntry("debug.bin", barsZs.AudioEntries_ZS[i].debugData);
                    bars.Nodes.Add(bin);
                }
            }
        }

        public void Save(Stream stream)
        {
            if (this.FilePath.EndsWith(".zs"))
            {
                this.IFileInfo.FileCompression = new Zstb();
                this.IFileInfo.FileIsCompressed = true;
            }
            //throw new NotImplementedException();
        }

        public void Unload()
        {
        }

        public class BinEntry : TreeNodeCustom, IContextMenuNode
        {
            public byte[] Data;
            public ToolStripItem[] GetContextMenuItems()
            {
                List<ToolStripItem> Items = new List<ToolStripItem>();
                Items.Add(new ToolStripMenuItem("Export", null, delegate (object sender, EventArgs args)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = Text;
                    sfd.DefaultExt = Path.GetExtension(Text);
                    sfd.Filter = "All files(*.*)|*.*";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfd.FileName, Data);
                    }
                }, Keys.Control | Keys.E));
                Items.Add(new ToolStripMenuItem("Replace", null, delegate(object sender, EventArgs args)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.FileName = Text;
                    ofd.DefaultExt = Path.GetExtension(Text);
                    ofd.Filter = "All files(*.*)|*.*";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        Data = File.ReadAllBytes(ofd.FileName);
                        ShowHexView();
                    }
                }, Keys.Control | Keys.R));
                return Items.ToArray();
            }

            public BinEntry(string name, byte[] data) {
                ImageKey = "fileBlank";
                SelectedImageKey = "fileBlank";
                this.Text = name;
                this.Data = data;
            }

            private void ShowHexView()
            {
                HexEditor editor = (HexEditor)LibraryGUI.GetActiveContent(typeof(HexEditor));
                if (editor == null)
                {
                    editor = new HexEditor();
                    LibraryGUI.LoadEditor(editor);
                }
                editor.Text = Text;
                editor.Dock = DockStyle.Fill;
                editor.LoadData(Data);
                editor.onChanged = delegate (byte[] data) {
                    Data = data;
                };
            }

            public override void OnClick(TreeView treeview)
            {
                ShowHexView();
            }
        }
    }
}
