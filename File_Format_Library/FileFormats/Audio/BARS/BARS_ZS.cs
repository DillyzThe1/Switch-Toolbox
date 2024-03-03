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
using System.Linq;

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

            Items.Add(new ToolStripMenuItem("Generate from BWAV", null, delegate (object sender, EventArgs args) {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.FileName = Text;
                ofd.DefaultExt = Path.GetExtension(Text);
                ofd.Filter = "BWAV Audio Files(*.bwav)|*.bwav";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    AudioEntry_ZS aezs = new AudioEntry_ZS();
                    aezs.name = Path.GetFileNameWithoutExtension(ofd.FileName);

                    aezs.AudioFile = new BARSAudioFileZS();
                    aezs.AudioFile.data = File.ReadAllBytes(ofd.FileName);
                    aezs.AudioFile.AudioFileSize = (uint)aezs.AudioFile.data.Length;

                    aezs.MetaData = new AMTAv5();
                    aezs.MetaData.vers_min = 0;
                    aezs.MetaData.vers_maj = 5;
                    aezs.MetaData.unk0 = aezs.MetaData.unk3 = aezs.MetaData.unk4 = 0;
                    aezs.MetaData.unk6 = 7;
                    aezs.MetaData.type = 1;
                    aezs.MetaData.channels = 2;
                    aezs.MetaData.unk7 = aezs.MetaData.flags = 0;

                    barsZs.AudioEntries_ZS.Add(aezs);
                    DisplayAmtas();
                }
            }, Keys.Control | Keys.N));

            Items.Add(new ToolStripMenuItem("Batch Export BWAVs", null, delegate (object sender, EventArgs args) {
                FolderSelectDialog fsd = new FolderSelectDialog();

                if (fsd.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < barsZs.AudioEntries_ZS.Count; i++)
                    {
                        AudioEntry_ZS entry = barsZs.AudioEntries_ZS[i];
                        if (!entry.hasAudio)
                            continue;
                        File.WriteAllBytes(fsd.SelectedPath + "\\" + entry.name + ".bwav", entry.AudioFile.data);
                    }
                }
            }, Keys.Control | Keys.B));

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

        private class BARSv5Wrapper : TreeNodeCustom, IContextMenuNode
        {
            public BARSv5Wrapper(AMTAv5 amta, BARS_ZS nest)
            {
                MetaFile = amta;
                this.nest = nest;
                SelectedImageKey = ImageKey = "MetaInfo";
            }

            public AMTAv5 MetaFile { get; set; }
            BARS_ZS nest;

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

            public ToolStripItem[] GetContextMenuItems()
            {
                List<ToolStripItem> Items = new List<ToolStripItem>();
                Items.Add(new ToolStripMenuItem("Duplicate", null, delegate (object sender, EventArgs args)
                {
                    for (int i = 0; i < nest.barsZs.AudioEntries_ZS.Count; i++) {
                        AudioEntry_ZS aezs = nest.barsZs.AudioEntries_ZS[i];
                        if (aezs.MetaData.ID != MetaFile.ID)
                            continue;

                        AudioEntry_ZS aezs_new = new AudioEntry_ZS();
                        aezs_new.name = aezs.name + "_0";

                        aezs_new.AudioFile = new BARSAudioFileZS();
                        aezs_new.AudioFile.data = aezs.AudioFile.data.ToArray();
                        aezs_new.AudioFile.AudioFileSize = aezs.AudioFile.AudioFileSize;

                        aezs_new.MetaData = aezs.MetaData.Clone();

                        nest.barsZs.AudioEntries_ZS.Add(aezs_new);
                        nest.DisplayAmtas();
                    }
                }));
                Items.Add(new ToolStripMenuItem(MetaFile.data != null ? "Delete DATA" : "Create DATA", null, delegate (object sender, EventArgs args)
                {
                    MetaFile.data = (MetaFile.data != null) ? null : new AMTAv5.AMTAv5_Data();
                    
                    STPropertyGrid editor = (STPropertyGrid)LibraryGUI.GetActiveContent(typeof(STPropertyGrid));
                    if (editor != null)
                        editor.Refresh();
                }));
                Items.Add(new ToolStripMenuItem(MetaFile.marker != null ? "Delete MARKER" : "Create MARKER", null, delegate (object sender, EventArgs args)
                {
                    MetaFile.marker = (MetaFile.marker != null) ? null : new AMTAv5.AMTAv5_Marker();

                    STPropertyGrid editor = (STPropertyGrid)LibraryGUI.GetActiveContent(typeof(STPropertyGrid));
                    if (editor != null)
                        editor.Refresh();
                }));
                Items.Add(new ToolStripMenuItem(MetaFile.minf != null ? "Delete MINF" : "Create MINF", null, delegate (object sender, EventArgs args)
                {
                    MetaFile.minf = (MetaFile.minf != null) ? null : new AMTAv5.AMTAv5_Minf();

                    STPropertyGrid editor = (STPropertyGrid)LibraryGUI.GetActiveContent(typeof(STPropertyGrid));
                    if (editor != null)
                        editor.Refresh();
                }));

                // i hated doing this so much, but this was the best way without a context menu.
                if (MetaFile.minf != null)
                {
                    Items.Add(new ToolStripMenuItem(MetaFile.minf.ResMinfTable0 != null ? "MINF: Delete ResMinfTable0" : "MINF: Create ResMinfTable0", null, delegate (object sender, EventArgs args)
                    {
                        MetaFile.minf.ResMinfTable0 = (MetaFile.minf.ResMinfTable0 != null) ? null : new AMTAv5.AMTAv5_Minf.AMTAv5_ResMinfTable0();

                        STPropertyGrid editor = (STPropertyGrid)LibraryGUI.GetActiveContent(typeof(STPropertyGrid));
                        if (editor != null)
                            editor.Refresh();
                    }));
                    Items.Add(new ToolStripMenuItem(MetaFile.minf.ResMinfTable1 != null ? "MINF: Delete ResMinfTable1" : "MINF: Create ResMinfTable1", null, delegate (object sender, EventArgs args)
                    {
                        MetaFile.minf.ResMinfTable1 = (MetaFile.minf.ResMinfTable1 != null) ? null : new AMTAv5.AMTAv5_Minf.AMTAv5_ResMinfTable1();

                        STPropertyGrid editor = (STPropertyGrid)LibraryGUI.GetActiveContent(typeof(STPropertyGrid));
                        if (editor != null)
                            editor.Refresh();
                    }));
                    Items.Add(new ToolStripMenuItem(MetaFile.minf.ResMinfTable2 != null ? "MINF: Delete ResMinfTable2" : "MINF: Create ResMinfTable2", null, delegate (object sender, EventArgs args)
                    {
                        MetaFile.minf.ResMinfTable2 = (MetaFile.minf.ResMinfTable2 != null) ? null : new AMTAv5.AMTAv5_Minf.AMTAv5_ResMinfTable2();

                        STPropertyGrid editor = (STPropertyGrid)LibraryGUI.GetActiveContent(typeof(STPropertyGrid));
                        if (editor != null)
                            editor.Refresh();
                    }));
                    Items.Add(new ToolStripMenuItem(MetaFile.minf.ResMinfPairTable != null ? "MINF: Delete ResMinfPairTable" : "MINF: Create ResMinfPairTable", null, delegate (object sender, EventArgs args)
                    {
                        MetaFile.minf.ResMinfPairTable = (MetaFile.minf.ResMinfPairTable != null) ? null : new AMTAv5.AMTAv5_Minf.AMTAv5_ResMinfPairTable();

                        STPropertyGrid editor = (STPropertyGrid)LibraryGUI.GetActiveContent(typeof(STPropertyGrid));
                        if (editor != null)
                            editor.Refresh();
                    }));
                    Items.Add(new ToolStripMenuItem(MetaFile.minf.ResMinfOffsetTable != null ? "MINF: Delete ResMinfOffsetTable" : "MINF: Create ResMinfOffsetTable", null, delegate (object sender, EventArgs args)
                    {
                        MetaFile.minf.ResMinfOffsetTable = (MetaFile.minf.ResMinfOffsetTable != null) ? null : new AMTAv5.AMTAv5_Minf.AMTAv5_ResMinfOffsetTable();

                        STPropertyGrid editor = (STPropertyGrid)LibraryGUI.GetActiveContent(typeof(STPropertyGrid));
                        if (editor != null)
                            editor.Refresh();
                    }));
                    Items.Add(new ToolStripMenuItem(MetaFile.minf.ResMinfInstrumentInfoTable != null ? "MINF: Delete ResMinfInstrumentInfoTable" : "MINF: Create ResMinfInstrumentInfoTable", null, delegate (object sender, EventArgs args)
                    {
                        MetaFile.minf.ResMinfInstrumentInfoTable = (MetaFile.minf.ResMinfInstrumentInfoTable != null) ? null : new AMTAv5.AMTAv5_Minf.AMTAv5_ResMinfInstrumentInfoTable();

                        STPropertyGrid editor = (STPropertyGrid)LibraryGUI.GetActiveContent(typeof(STPropertyGrid));
                        if (editor != null)
                            editor.Refresh();
                    }));
                }
                return Items.ToArray();
            }

            private void OnPropertyChanged()
            {
            }
        }

        public BarsZsFile barsZs;
        public void Load(Stream stream)
        {
            CanSave = true;

            if (MessageBox.Show("The Bars.ZS writer is NOT complete and will NEVER load in anything!"
                + "\nIt doesn't write any audio or AMTA headers, just the BARS header!\nDo you want to enable saving?",
                    "New Switch Toolbox", MessageBoxButtons.YesNo) == DialogResult.No)
                CanSave = false;

            Text = FileName;

            Console.WriteLine("barzz " + CanSave);
            barsZs = new BarsZsFile(stream);

            DisplayAmtas();
        }

        public void DisplayAmtas() {
            Nodes.Clear();
            Console.WriteLine("read " + barsZs.AudioEntries_ZS.Count + " bars lol");
            for (int i = 0; i < barsZs.AudioEntries_ZS.Count; i++)
            {
                AudioEntry_ZS entry = barsZs.AudioEntries_ZS[i];
                if (entry.MetaData == null)
                {
                    Console.WriteLine("um... what? the metadata of " + barsZs.AudioEntries_ZS[i].name + " is NULL?!?!");
                    continue;
                }

                Console.WriteLine("Show " + barsZs.AudioEntries_ZS[i].name + " on the bars.");
                BARSv5Wrapper bars = new BARSv5Wrapper(entry.MetaData, this);
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
            barsZs.Save(stream);
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
