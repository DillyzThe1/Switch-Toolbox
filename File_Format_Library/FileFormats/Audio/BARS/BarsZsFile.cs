using System;
using System.Collections.Generic;
using System.IO;
using BarsLib.IO;
using Syroot.BinaryData;
using BarsLib;
using NAudio.Wave;
using System.Text;
using Microsoft.VisualBasic.Devices;
using static FirstPlugin.BarsZsFile.AMTAv5;
using System.ComponentModel;
using static FirstPlugin.BarsZsFile.AMTAv5.AMTAv5_Data;
using static FirstPlugin.BarsZsFile.AMTAv5.AMTAv5_Marker;

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

        public class AMTAv5
        {
            public byte vers_min { get; set; }
            public byte vers_maj { get; set; }

            // these can just default to 0
            public uint unk0 { get; set; }
            public uint unk3 { get; set; }
            public uint unk4 { get; set; }

            public uint unk6 { get; set; }

            public byte type { get; set; }
            public byte channels { get; set; }
            public byte unk7 { get; set; }
            public byte flags { get; set; }

            public AMTAv5_Data data { get; set; }
            public AMTAv5_Marker marker { get; set; }
            public AMTAv5_Minf minf { get; set; }

            public bool hasData { get { return data != null; } }
            public bool hasMarker { get { return marker != null; } }
            public bool hasMinf { get { return minf != null; } }

            // DATA
            [TypeConverter(typeof(ExpandableObjectConverter))]
            [EditorBrowsable(EditorBrowsableState.Always)]
            public class AMTAv5_Data
            {
                public uint unk0 { get; set; }
                public float unk1 { get; set; }
                public float unk2 { get; set; }
                public float unk3 { get; set; }
                public float unk4 { get; set; }
                public ushort unk6 { get; set; }

                public List<AMTAv5_Point> points { get; set; }

                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_Point
                {
                    public uint unk0 { get; set; }
                    public float unk1 { get; set; }

                    public AMTAv5_Point(uint unk0, float unk1)
                    {
                        this.unk0 = unk0;
                        this.unk1 = unk1;
                    }

                    public AMTAv5_Point()
                    {
                        this.unk0 = 0;
                        this.unk1 = 0;
                    }
                }
            }

            // MARKER
            [TypeConverter(typeof(ExpandableObjectConverter))]
            [EditorBrowsable(EditorBrowsableState.Always)]
            public class AMTAv5_Marker
            {
                public List<AMTAv5_MarkerIndex> markers { get; set; }

                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_MarkerIndex
                {
                    public uint id { get; set; }
                    public string name { get; set; }
                    public uint start { get; set; }
                    public uint length { get; set; }

                    public AMTAv5_MarkerIndex(uint id, string name, uint start, uint length)
                    {
                        this.id = id;
                        this.name = name;
                        this.start = start;
                        this.length = length;
                    }

                    public AMTAv5_MarkerIndex()
                    {
                        this.id = 0;
                        this.name = "awesomeMarker";
                        this.start = 0;
                        this.length = 0;
                    }
                }
            }

            // MINF
            [TypeConverter(typeof(ExpandableObjectConverter))]
            [EditorBrowsable(EditorBrowsableState.Always)]
            public class AMTAv5_Minf
            {
                public bool comeBackLaterYaNerd { get { return true; } }
            }
        }

        public class AudioEntry_ZS
        {
            public AMTAv5 MetaData;
            public BARSAudioFileZS AudioFile;
            public bool hasAudio { get { return AudioFile != null; } }

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

            Console.WriteLine("bars.zs file here we load 3 " + 0x10);

            // skip the CRC32 file name hashes
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

                /// AMTA DATA 
                AMTAv5 amtav5 = new AMTAv5();

                loader.Position = fi_info + 4;
                loader.ReadUInt16(); // the endianness. we don't need to re-do that, though.

                amtav5.vers_min = loader.ReadByte();
                amtav5.vers_maj = loader.ReadByte();
                string amtaVers = amtav5.vers_maj + "." + amtav5.vers_min;

                Console.WriteLine("AMTA v" + amtaVers + " detected.");
                uint amtaSize = loader.ReadUInt32(); // AMTA file size
                amtav5.unk0 = loader.ReadUInt32(); // unknown 0

                uint amta_dataOffset = loader.ReadUInt32(); // offset to the amta data?
                uint amta_markerOffset = loader.ReadUInt32(); // offset to the.... marker data?
                uint amta_minfOffset = loader.ReadUInt32(); // offset to the.... minf data?!

                long pos_2 = loader.Position;
                if (amta_dataOffset != 0)
                {
                    Console.WriteLine("This AMTA v" + amtaVers + " has the data offset.");

                    loader.Position = fi_info + amta_dataOffset;
                    AMTAv5_Data datav5 = new AMTAv5_Data();
                    datav5.unk0 = loader.ReadUInt32();
                    datav5.unk1 = loader.ReadSingle();
                    datav5.unk2 = loader.ReadSingle();
                    datav5.unk3 = loader.ReadSingle();
                    datav5.unk4 = loader.ReadSingle();
                    ushort point_count = loader.ReadUInt16();
                    datav5.unk6 = loader.ReadUInt16();

                    datav5.points = new List<AMTAv5_Point>();
                    for (int p = 0; p < point_count; p++)
                        datav5.points.Add(new AMTAv5_Point(loader.ReadUInt32(), loader.ReadSingle()));

                    amtav5.data = datav5;
                }
                if (amta_markerOffset != 0)
                {
                    Console.WriteLine("This AMTA v" + amtaVers + " has the marker offset.");
                    loader.Position = fi_info + amta_markerOffset;

                    uint markerCount = loader.ReadUInt32();
                    AMTAv5_Marker markerv5 = new AMTAv5_Marker();
                    markerv5.markers = new List<AMTAv5_MarkerIndex>();
                    for (int m = 0; m < markerCount; m++)
                    {
                        loader.Position = fi_info + amta_markerOffset + 4 + 16*m;

                        AMTAv5_MarkerIndex mi = new AMTAv5_MarkerIndex();
                        mi.id = loader.ReadUInt32();
                        long pos_markerthing = loader.Position;

                        loader.Position = pos_markerthing + loader.ReadUInt32();
                        mi.name = loader.ReadString(BinaryStringFormat.ZeroTerminated).Trim();

                        loader.Position = pos_markerthing;
                        mi.start = loader.ReadUInt32();
                        mi.length = loader.ReadUInt32();

                        markerv5.markers.Add(mi);
                    }
                    amtav5.marker = markerv5;
                }
                if (amta_minfOffset != 0)
                {
                    Console.WriteLine("This AMTA v" + amtaVers + " has the minf offset.");
                    AMTAv5_Minf minfv5 = new AMTAv5_Minf();
                    amtav5.minf = minfv5;
                }
                loader.Position = pos_2;

                amtav5.unk3 = loader.ReadUInt32(); // unknown 3 (always 0?)
                amtav5.unk4 = loader.ReadUInt32(); // unknown 4 (always 0?)

                int amta_pathOffset = (int)loader.Position + (int)loader.ReadUInt32(); // string offset of path
                loader.ReadUInt32(); // path CRC32 hash

                amtav5.unk6 = loader.ReadUInt32(); // unknown 6

                amtav5.type = loader.ReadByte();
                amtav5.channels = loader.ReadByte();
                amtav5.unk7 = loader.ReadByte(); // unknown 7
                amtav5.flags = loader.ReadByte();

                // use amta data we just read
                loader.Position = amta_pathOffset;
                string fi_fileName = loader.ReadString(BinaryStringFormat.ZeroTerminated).Trim();// + ".bwav";

                // BWAV DATA
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
                entry.MetaData = amtav5;

                loader.Position = pos_2 - 0x20;

                if (fi_data > 0)
                {
                    BARSAudioFileZS spittingBars = new BARSAudioFileZS();
                    spittingBars.Load(loader);
                    spittingBars.AudioFileSize = fi_dataSize;

                    if (spittingBars != null)
                    {
                        entry.AudioFile = spittingBars;
                        entry.AudioFile.SetData(loader, entry.AudioFile.AudioFileSize);
                    }
                }

                //loader.Position = fi_info;
                //entry.debugData = loader.ReadBytes((int)amtaSize);

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
