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

        public BarsZsFile barsZs;
        public void Load(Stream stream)
        {
            CanSave = false;

            Text = FileName;

            Console.WriteLine("barzz");
            barsZs = new BarsZsFile(stream);

            Console.WriteLine("helloo hi hi hiiii!!!!");
            if (barsZs.HasMetaData)
                Nodes.Add("Meta Data");

            if (barsZs.HasAudioFiles)
                Nodes.Add(new AudioFolder("Audio"));

            Console.WriteLine("read bars lol");
            for (int i = 0; i < barsZs.AudioEntries.Count; i++)
            {
                if (barsZs.AudioEntries[i].AudioFile != null)
                {
                    BARSAudioFile audio = barsZs.AudioEntries_ZS[i].AudioFile;

                    AudioEntry node = new AudioEntry();
                    node.audioFile = audio;
                    node.Magic = audio.Magic;
                    node.SetupMusic();

                    if (audio.Magic == "FWAV")
                        node.Text = barsZs.AudioEntries_ZS[i].name + ".bfwav";
                    else if (audio.Magic == "FSTP")
                        node.Text = barsZs.AudioEntries_ZS[i].name + ".bfstp";
                    else if (audio.Magic == "BWAV")
                        node.Text = barsZs.AudioEntries_ZS[i].name + ".bwav";
                    else
                        node.Text = $"{barsZs.AudioEntries_ZS[i].name}.{audio.Magic}";

                    Nodes[1].Nodes.Add(node);
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
    }
}
