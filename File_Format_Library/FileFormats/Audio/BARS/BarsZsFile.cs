using System;
using System.Collections.Generic;
using System.IO;
using BarsLib.IO;
using Syroot.BinaryData;
using BarsLib;
using NAudio.Wave;
using System.Text;

namespace FirstPlugin
{
    public class BarsZsFile : BarsLib.BARS, IFileData
    {
        public ushort versNum = 0;

        internal ByteOrder ByteOrder;

        public new bool HasMetaData
        {
            get
            {
                for (int i = 0; i < AudioEntries_ZS.Count; i++)
                {
                    if (AudioEntries_ZS[i].MetaData != null)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public new bool HasAudioFiles
        {
            get
            {
                for (int i = 0; i < AudioEntries_ZS.Count; i++)
                {
                    if (AudioEntries_ZS[i].AudioFile != null)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public class AudioEntry_ZS
        {
            public AMTA MetaData;

            public BARSAudioFile AudioFile;

            public string name;
        }

        public IList<AudioEntry_ZS> AudioEntries_ZS = new List<AudioEntry_ZS>();

        public BarsZsFile(Stream stream) : base(stream)
        {
            FileLoader fileLoader = new FileLoader(stream, this);
            fileLoader.Read();
        }


        // stolen from filereader.cs
        public bool CheckSignature_Ext(FileLoader loader, int length, string Identifier, long position = 0)
        {
            if (position < 0)
            {
                if (loader.Position + length  >= loader.BaseStream.Length )
                    return false;

                long oldOld = loader.Position;
                string funnysignature = loader.ReadString(length, Encoding.ASCII);

                //Reset position
                loader.Position = oldOld;

                return funnysignature == Identifier;

            }

            if (loader.Position + length + position >= loader.BaseStream.Length)
                return false;

            loader.Position = position;
            string signature = loader.ReadString(length, Encoding.ASCII);

            //Reset position
            loader.Position = 0;

            return signature == Identifier;
        }

        // i am so sorry for my war crimes against these files
        void IFileData.Load(FileLoader loader)
        {
            Console.WriteLine("zs file here we load 1 lp = " + loader.Position);
            CheckSignature_Ext(loader, 4, "BARS"); // magic number
            loader.Position = 4;
            loader.ReadUInt32(); // file size?
            Console.WriteLine("zs file here we load 1.5 lp = " + loader.Position);
            ByteOrderMark = loader.ReadUInt16(); // big/little endian
            loader.ByteOrder = (ByteOrderMark == 65534) ? ByteOrder.LittleEndian : ByteOrder.BigEndian;
            ByteOrder = loader.ByteOrder;

            versNum = loader.ReadUInt16(); // file vers num (we want 0x0201)

            Console.WriteLine("zs file here we load 2 lp = " + loader.Position);
            byte fileCount = loader.ReadByte(); // file count
            loader.Position += 3;
            Console.WriteLine("zs file here we load 2.5 lp = " + loader.Position);
            //Hashes = loader.ReadUInt32s((int)fileCount);
            long position = loader.Position;

            long tableThing = (fileCount * 4) + 16;

            Console.WriteLine("zs file here we load 3 " + 0x10);
            // List<uint> list = new List<uint>();
            //List<uint> list2 = new List<uint>();
            //for (int i = 0; i < fileCount; i++)
           // {
                //Console.WriteLine("Found an " + loader.ReadString(4, Encoding.ASCII) + " file.");
                //loader.ReadUInt32();
                //loader.CheckSignature_Ext(loader, 4, "BARS");
                /*uint num2 = loader.ReadUInt32();
                if (num2 != uint.MaxValue && num2 != 0)
                {
                    Console.WriteLine(num2);
                    list.Add(num2);
                }*/
            //}

            for (int i = 0; i < fileCount; i++)
            {
                Console.WriteLine("lp " + loader.Position + ", ll " + loader.Length + ", tableThing " + tableThing);
                loader.Position = tableThing;
                long fi_info = loader.ReadInt32();
                long fi_data = loader.ReadInt32();

                if (fi_info > loader.Length || fi_data > loader.Length || fi_info + fi_data > loader.Length)
                {
                    Console.WriteLine("info " + fi_info + ", data " + fi_data);
                    continue;
                }

                loader.Position = fi_info;
                loader.ReadString(0x24); // junk data?

                long pos_2 = loader.Position;
                long fi_nameOff = loader.ReadInt32() + pos_2;
                loader.Position = fi_nameOff;

                string fi_fileName = loader.ReadString(BinaryStringFormat.ZeroTerminated).Trim();// + ".bwav";
                loader.Position = fi_data;

                loader.ReadString(0x1c); // junk data?
                long fi_dataSize = loader.ReadInt32();

                Console.WriteLine("dataSize " + fi_dataSize);

                fi_dataSize = ((fi_dataSize + 13) / 14) * 8 + 0x80; // unstored file size lmao

                Console.WriteLine(fi_fileName + ".bwav discovered. length " + fi_dataSize + ".");

                AudioEntry_ZS entry = new AudioEntry_ZS();
                entry.name = fi_fileName;

                BARSAudioFile spittingBars = new BARSAudioFile();
                spittingBars.Magic = "BWAV";

                if (spittingBars != null) {
                    entry.AudioFile = spittingBars;
                    entry.AudioFile.data = null;
                }

                AudioEntries_ZS.Add(entry);

                tableThing += 8;
            }
            Console.WriteLine("wharr????");

        }
    }
}
