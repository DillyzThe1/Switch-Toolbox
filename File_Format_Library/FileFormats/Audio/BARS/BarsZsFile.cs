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
    public class BarsZsFile : IFileDataZS
    {
        public ushort versNum = 0;

        internal ByteOrder ByteOrder;

        public ushort ByteOrderMark;

        public bool HasMetaData
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

        public bool HasAudioFiles
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

        public bool HasDebugData
        {
            get
            {
                for (int i = 0; i < AudioEntries_ZS.Count; i++)
                {
                    if (AudioEntries_ZS[i].debugData != null)
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

            public BARSAudioFileZS AudioFile;

            public byte[] debugData;

            public string name;
        }

        public IList<AudioEntry_ZS> AudioEntries_ZS = new List<AudioEntry_ZS>();

        public BarsZsFile(Stream stream)
        {
            FileLoaderBarsZS fileLoader = new FileLoaderBarsZS(stream, this);
            fileLoader.Execute();
        }


        // stolen from filereader.cs
        public bool CheckSignature_Ext(FileLoaderBarsZS loader, int length, string Identifier, long position = 0)
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
        void IFileDataZS.Load(FileLoaderBarsZS loader)
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


            /*
            AMTA time.
            Let's take some notes, shall we?

            Every file seems to have about 4 bytes of who-knows-what pointers/numbers.
            What these 4 bytes are is what I don't know yet.

            What I do know is that in BgmPlazaFestDay2.Product.600.bars.zs (decompressed), there are 3 AMTA things present.
            The addresses for these are as follows:
                 - 0x00000044
                 - 0x00000E1C
                 - 0x000021B4


            UPDATE UPDATE UPDATE!!!!!!!
            these are crc32 hashes of their names! don't worry about them unless exporting!
            */
            long posStarter = 16;

            Console.WriteLine("zs file here we load 3 " + 0x10);

            posStarter = (fileCount * 4) + 16;
            for (int i = 0; i < fileCount; i++)
            {
                Console.WriteLine("lp " + loader.Position + ", ll " + loader.Length + ", posStarter " + posStarter);
                loader.Position = posStarter;

                // amta position
                long fi_info = loader.ReadInt32();
                // bwav position
                long fi_data = loader.ReadInt32();

                if (fi_info > loader.Length || fi_data > loader.Length || fi_info + fi_data > loader.Length)
                {
                    Console.WriteLine("info " + fi_info + ", data " + fi_data);
                    continue;
                }
                Console.WriteLine("NOT AN ERROR info " + fi_info + ", data " + fi_data);

                loader.Position = fi_info;
                //byte[] amtaDataIThink = loader.ReadBytes(0x24); // junk data?

                // we need to read 36 bytes here... somehow
                loader.ReadBytes(8); // AMTA magic + 0xFFFE endian + 0x0005 AMTA version
                // so... where's the other 28?

                // skip unknown bytes for now
                loader.ReadBytes(28);

                long pos_2 = loader.Position;
                long fi_nameOff = loader.ReadInt32() + pos_2;
                loader.Position = fi_nameOff;

                string fi_fileName = loader.ReadString(BinaryStringFormat.ZeroTerminated).Trim();// + ".bwav";
                loader.Position = fi_data;

                loader.ReadString(0x1c); // junk data?
                uint fi_dataSize = loader.ReadUInt32();
                pos_2 = loader.Position;

                Console.WriteLine("dataSize " + fi_dataSize);

                fi_dataSize = ((fi_dataSize + 13) / 14) * 8 + 0x80; // unstored file size lmao

                Console.WriteLine(fi_fileName + ".bwav discovered. length " + fi_dataSize + ".");

                if (loader.Position + fi_dataSize > loader.Length)
                    Console.WriteLine("ummmmmm " + (loader.Position + fi_dataSize) + " over " + loader.Length + " real 3am?!?!?!?!");

                AudioEntry_ZS entry = new AudioEntry_ZS();
                entry.name = fi_fileName;

                loader.Position = pos_2 - 0x20;
                BARSAudioFileZS spittingBars = new BARSAudioFileZS();
                spittingBars.Load(loader);
                spittingBars.AudioFileSize = fi_dataSize;

                if (spittingBars != null) {
                    entry.AudioFile = spittingBars;
                    entry.AudioFile.SetData(loader, entry.AudioFile.AudioFileSize);
                }

                loader.Position = fi_info;
                entry.debugData = loader.ReadBytes((int)(fi_nameOff - fi_info));

                AudioEntries_ZS.Add(entry);

                posStarter += 8;
            }
            Console.WriteLine("wharr???? " + AudioEntries_ZS.Count);
        }

        //void IFileDataZS.Save(FileSaver saver)
        //{ 
        //}

    }

    public class BARSAudioFileZS : IFileDataZS
    {
        public byte[] data;

        public int Alignment;

        public long audioPos;

        public uint AudioFileSize;

        public void Load(FileLoaderBarsZS loader)
        {
            audioPos = loader.Position;
            loader.ByteOrder = ByteOrder.BigEndian;
            if (loader.ReadUInt16() == 65534)
                loader.ByteOrder = ByteOrder.LittleEndian;
            else
                loader.ByteOrder = ByteOrder.BigEndian;

            loader.ReadUInt16();
            loader.ReadUInt32();
            loader.ReadUInt32();
            loader.ReadUInt16();
            loader.ReadUInt16();
            loader.ReadUInt16();
            loader.ReadUInt16();
            loader.ReadUInt32();
            loader.ReadUInt32();
            loader.ReadUInt16();
            loader.ReadUInt16();
            loader.ReadUInt32();
            loader.ReadUInt32();

            loader.ByteOrder = loader.BARS.ByteOrder;
        }

        internal void SetData(FileLoaderBarsZS reader, uint FileSize)
        {
            long position = reader.Position;
            //reader.Seek(audioPos, SeekOrigin.Begin);

            reader.Position = audioPos;
            Console.WriteLine(audioPos + FileSize);
            data = reader.ReadBytes((int)FileSize);
            reader.Position = position;
            //reader.Seek(position, SeekOrigin.Begin);
        }
    }
}
