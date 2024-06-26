﻿using System;
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
using static FirstPlugin.BarsZsFile.AMTAv5.AMTAv5_Minf;
using System.Management.Instrumentation;
using CafeLibrary.M2;
using Toolbox.Library.Security.Cryptography;
using System.Runtime.InteropServices;

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

            public bool reserved { get; set; }

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
                public byte vers_maj { get; set; }
                public byte vers_min { get; set; }
                public string name { get; set; }
                public uint unk0 { get; set; }
                public uint sampleRate { get; set; }
                public uint unk1 { get; set; }
                public uint unk2 { get; set; }
                public uint unk3 { get; set; }
                public ushort unk4 { get; set; }
                public byte unk5 { get; set; }
                public byte unk6 { get; set; }
                public ushort unk7 { get; set; }
                public ushort unk8 { get; set; }
                public uint unk15 { get; set; }
                public uint unk16 { get; set; }

                // TODO: MAKE THEIR DATATYPES REAL
                public AMTAv5_ResMinfTable0 ResMinfTable0 { get; set; }
                public AMTAv5_ResMinfTable1 ResMinfTable1 { get; set; }
                public AMTAv5_ResMinfTable2 ResMinfTable2 { get; set; }
                public AMTAv5_ResMinfPairTable ResMinfPairTable { get; set; }
                public AMTAv5_ResMinfOffsetTable ResMinfOffsetTable { get; set; }
                public AMTAv5_ResMinfInstrumentInfoTable ResMinfInstrumentInfoTable { get; set; }

                #region resminftable0
                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfTable0Entry {
                    public uint unk0 { get; set; }
                    public ushort unk1 { get; set; }
                    public byte unk2 { get; set; }
                    public byte unk3 { get; set; }
                    public ushort unk4 { get; set; }
                    public ushort unk5 { get; set; }

                    public AMTAv5_ResMinfTable0Entry(uint unk0, ushort unk1, byte unk2, byte unk3, ushort unk4, ushort unk5) {
                        this.unk0 = unk0;
                        this.unk1 = unk1;
                        this.unk2 = unk2;
                        this.unk3 = unk3;
                        this.unk4 = unk4;
                        this.unk5 = unk5;
                    }

                    public AMTAv5_ResMinfTable0Entry()
                    {
                        this.unk0 = 0;
                        this.unk1 = 0;
                        this.unk2 = 0;
                        this.unk3 = 0;
                        this.unk4 = 0;
                        this.unk5 = 0;
                    }
                }

                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfTable0
                {
                    public ushort unk0 { get; set; }
                    public List<AMTAv5_ResMinfTable0Entry> entries { get; set; }
                }
                #endregion

                #region resminftable1
                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfTable1Entry
                {
                    public uint unk0 { get; set; }
                    public uint unk1 { get; set; }
                    public uint unk2 { get; set; }
                    public uint unk3 { get; set; }

                    public AMTAv5_ResMinfTable1Entry(uint unk0, uint unk1, uint unk2, uint unk3)
                    {
                        this.unk0 = unk0;
                        this.unk1 = unk1;
                        this.unk2 = unk2;
                        this.unk3 = unk3;
                    }

                    public AMTAv5_ResMinfTable1Entry()
                    {
                        this.unk0 = 0;
                        this.unk1 = 0;
                        this.unk2 = 0;
                        this.unk3 = 0;
                    }
                }

                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfTable1
                {
                    public ushort unk0 { get; set; }
                    public List<AMTAv5_ResMinfTable1Entry> entries { get; set; }
                }
                #endregion

                #region resminftable2
                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfTable2Entry
                {
                    public uint unk0 { get; set; }
                    public uint unk1 { get; set; }
                    public uint unk2 { get; set; }
                    public float unk3 { get; set; }
                    public uint unk4 { get; set; }

                    public AMTAv5_ResMinfTable2Entry(uint unk0, uint unk1, uint unk2, float unk3, uint unk4)
                    {
                        this.unk0 = unk0;
                        this.unk1 = unk1;
                        this.unk2 = unk2;
                        this.unk3 = unk3;
                        this.unk4 = unk4;
                    }

                    public AMTAv5_ResMinfTable2Entry()
                    {
                        this.unk0 = 0;
                        this.unk1 = 0;
                        this.unk2 = 0;
                        this.unk3 = 0;
                        this.unk4 = 0;
                    }
                }

                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfTable2
                {
                    public ushort unk0 { get; set; }
                    public List<AMTAv5_ResMinfTable2Entry> entries { get; set; }
                }
                #endregion

                #region resminfpairtable
                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfPairTableEntry
                {
                    public uint unk0 { get; set; }
                    public uint unk1 { get; set; }

                    public AMTAv5_ResMinfPairTableEntry(uint unk0, uint unk1)
                    {
                        this.unk0 = unk0;
                        this.unk1 = unk1;
                    }

                    public AMTAv5_ResMinfPairTableEntry()
                    {
                        this.unk0 = 0;
                        this.unk1 = 0;
                    }
                }

                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfPairTable
                {
                    public ushort unk0 { get; set; }
                    public List<AMTAv5_ResMinfPairTableEntry> entries { get; set; }
                }
                #endregion

                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfOffsetTable
                {
                    public ushort unk0 { get; set; }
                    public List<uint> entries { get; set; }
                }

                #region resminfinstrumentinfo
                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfInstrument
                {
                    public string name { get; set; }
                    public uint unk0 { get; set; }
                    public uint unk1 { get; set; }

                    public AMTAv5_ResMinfInstrument(string name, uint unk0, uint unk1)
                    {
                        this.name = name;
                        this.unk0 = unk0;
                        this.unk1 = unk1;
                    }

                    public AMTAv5_ResMinfInstrument()
                    {
                        this.name = "instrument";
                        this.unk0 = 0;
                        this.unk1 = 0;
                    }
                }

                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfInstrumentInfo
                {
                    public uint unk0 { get; set; }
                    public AMTAv5_ResMinfInstrument instrument { get; set; }

                    public AMTAv5_ResMinfInstrumentInfo(uint unk0, AMTAv5_ResMinfInstrument instrument)
                    {
                        this.unk0 = unk0;
                        this.instrument = instrument;
                    }

                    public AMTAv5_ResMinfInstrumentInfo()
                    {
                        this.unk0 = 0;
                        this.instrument = null;
                    }
                }

                [TypeConverter(typeof(ExpandableObjectConverter))]
                [EditorBrowsable(EditorBrowsableState.Always)]
                public class AMTAv5_ResMinfInstrumentInfoTable
                {
                    public ushort unk0 { get; set; }
                    public List<AMTAv5_ResMinfInstrumentInfo> entries { get; set; }
                }
                #endregion
            }

            public uint ID;
            public static uint NEXT_ID = 0;
            public AMTAv5() {
                ID = NEXT_ID++;
            }

            public AMTAv5 Clone() {
                AMTAv5 c = new AMTAv5();

                // amta v5 itself
                c.vers_min = this.vers_min;
                c.vers_maj = this.vers_maj;
                c.unk0 = this.unk0;
                c.unk3 = this.unk3;
                c.unk4 = this.unk4;
                c.unk6 = this.unk6;
                c.type = this.type;
                c.channels = this.channels;
                c.unk7 = this.unk7;
                c.flags = this.flags;
                //

                // data
                if (this.data != null) {
                    c.data = new AMTAv5_Data();
                    c.data.unk0 = this.data.unk0;
                    c.data.unk1 = this.data.unk1;
                    c.data.unk2 = this.data.unk2;
                    c.data.unk3 = this.data.unk3;
                    c.data.unk4 = this.data.unk4;
                    c.data.unk6 = this.data.unk6;

                    c.data.points = new List<AMTAv5_Point>();
                    for (int i = 0; i < this.data.points.Count; i++)
                        c.data.points.Add(new AMTAv5_Point(this.data.points[i].unk0, this.data.points[i].unk1));
                }
                //

                // marker
                if (this.marker != null)
                {
                    c.marker = new AMTAv5_Marker();
                    c.marker.markers = new List<AMTAv5_MarkerIndex>();

                    for (int i = 0; i < this.marker.markers.Count; i++)
                        c.marker.markers.Add(new AMTAv5_MarkerIndex(this.marker.markers[i].id, this.marker.markers[i].name, this.marker.markers[i].start, this.marker.markers[i].length));
                }
                //

                // minf
                if (this.minf != null) {
                    c.minf = new AMTAv5_Minf();

                    c.minf.vers_maj = this.minf.vers_maj;
                    c.minf.vers_min = this.minf.vers_min;
                    c.minf.name = this.minf.name;
                    c.minf.unk0 = this.minf.unk0;
                    c.minf.sampleRate = this.minf.sampleRate;
                    c.minf.unk1 = this.minf.unk1;
                    c.minf.unk2 = this.minf.unk2;
                    c.minf.unk3 = this.minf.unk3;
                    c.minf.unk4 = this.minf.unk4;
                    c.minf.unk5 = this.minf.unk5;
                    c.minf.unk6 = this.minf.unk6;
                    c.minf.unk7 = this.minf.unk7;
                    c.minf.unk8 = this.minf.unk8;
                    c.minf.unk15 = this.minf.unk15;
                    c.minf.unk16 = this.minf.unk16;

                    if (this.minf.ResMinfTable0 != null)
                    {
                        c.minf.ResMinfTable0 = new AMTAv5_ResMinfTable0();
                        c.minf.ResMinfTable0.unk0 = this.minf.ResMinfTable0.unk0;

                        c.minf.ResMinfTable0.entries = new List<AMTAv5_ResMinfTable0Entry>();
                        for (int i = 0; i < this.minf.ResMinfTable0.entries.Count; i++)
                        {
                            AMTAv5_ResMinfTable0Entry ent = this.minf.ResMinfTable0.entries[i];
                            c.minf.ResMinfTable0.entries.Add(new AMTAv5_ResMinfTable0Entry(ent.unk0, ent.unk1, ent.unk2, ent.unk3, ent.unk4, ent.unk5));
                        }
                    }

                    if (this.minf.ResMinfTable1 != null)
                    {
                        c.minf.ResMinfTable1 = new AMTAv5_ResMinfTable1();
                        c.minf.ResMinfTable1.unk0 = this.minf.ResMinfTable1.unk0;

                        c.minf.ResMinfTable1.entries = new List<AMTAv5_ResMinfTable1Entry>();
                        for (int i = 0; i < this.minf.ResMinfTable1.entries.Count; i++)
                        {
                            AMTAv5_ResMinfTable1Entry ent = this.minf.ResMinfTable1.entries[i];
                            c.minf.ResMinfTable1.entries.Add(new AMTAv5_ResMinfTable1Entry(ent.unk0, ent.unk1, ent.unk2, ent.unk3));
                        }
                    }

                    if (this.minf.ResMinfTable2 != null)
                    {
                        c.minf.ResMinfTable2 = new AMTAv5_ResMinfTable2();
                        c.minf.ResMinfTable2.unk0 = this.minf.ResMinfTable2.unk0;

                        c.minf.ResMinfTable2.entries = new List<AMTAv5_ResMinfTable2Entry>();
                        for (int i = 0; i < this.minf.ResMinfTable2.entries.Count; i++)
                        {
                            AMTAv5_ResMinfTable2Entry ent = this.minf.ResMinfTable2.entries[i];
                            c.minf.ResMinfTable2.entries.Add(new AMTAv5_ResMinfTable2Entry(ent.unk0, ent.unk1, ent.unk2, ent.unk3, ent.unk4));
                        }
                    }

                    if (this.minf.ResMinfPairTable != null)
                    {
                        c.minf.ResMinfPairTable = new AMTAv5_ResMinfPairTable();
                        c.minf.ResMinfPairTable.unk0 = this.minf.ResMinfPairTable.unk0;

                        c.minf.ResMinfPairTable.entries = new List<AMTAv5_ResMinfPairTableEntry>();
                        for (int i = 0; i < this.minf.ResMinfPairTable.entries.Count; i++)
                        {
                            AMTAv5_ResMinfPairTableEntry ent = this.minf.ResMinfPairTable.entries[i];
                            c.minf.ResMinfPairTable.entries.Add(new AMTAv5_ResMinfPairTableEntry(ent.unk0, ent.unk1));
                        }
                    }

                    if (this.minf.ResMinfOffsetTable != null)
                    {
                        c.minf.ResMinfOffsetTable = new AMTAv5_ResMinfOffsetTable();
                        c.minf.ResMinfOffsetTable.unk0 = this.minf.ResMinfOffsetTable.unk0;

                        c.minf.ResMinfOffsetTable.entries = new List<uint>();
                        for (int i = 0; i < this.minf.ResMinfOffsetTable.entries.Count; i++)
                            c.minf.ResMinfOffsetTable.entries.Add(this.minf.ResMinfOffsetTable.entries[i]);
                    }

                    // ugh i'll hate making this part
                    // UPDATE: wasn't that bad but i still hated it
                    if (this.minf.ResMinfInstrumentInfoTable != null)
                    {
                        c.minf.ResMinfInstrumentInfoTable = new AMTAv5_ResMinfInstrumentInfoTable();
                        c.minf.ResMinfInstrumentInfoTable.unk0 = this.minf.ResMinfInstrumentInfoTable.unk0;

                        c.minf.ResMinfInstrumentInfoTable.entries = new List<AMTAv5_ResMinfInstrumentInfo>();
                        for (int i = 0; i < this.minf.ResMinfInstrumentInfoTable.entries.Count; i++) {
                            AMTAv5_ResMinfInstrumentInfo og = this.minf.ResMinfInstrumentInfoTable.entries[i];
                            c.minf.ResMinfInstrumentInfoTable.entries.Add(new AMTAv5_ResMinfInstrumentInfo(og.unk0,
                                new AMTAv5_ResMinfInstrument(og.instrument.name, og.instrument.unk0, og.instrument.unk1)
                            ));
                        }
                    }
                }
                //

                return c;
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
            uint fileCount = loader.ReadUInt32(); // file count
            Console.WriteLine("zs file here we load 2.5 lp = " + loader.Position);
            //Hashes = loader.ReadUInt32s((int)fileCount);
            long position = loader.Position;

            long posStarter = 16;
            Console.WriteLine("bars.zs file here we load 3 " + fileCount);

            // skip the CRC32 file name hashes
            posStarter = (fileCount * 4) + 16;

            List<uint> reservedCrc32s = new List<uint>();
            loader.Position = posStarter + fileCount * 8;
            uint reservedFiles = loader.ReadUInt32();
            Console.WriteLine("We have " + reservedFiles + " reserved files.");

            for (int i = 0; i < reservedFiles; i++)
                reservedCrc32s.Add(loader.ReadUInt32());

            for (int i = 0; i < fileCount; i++)
            {
                Console.WriteLine("lp " + loader.Position + ", ll " + loader.Length + ", posStarter " + posStarter);
                loader.Position = posStarter;

                // amta position
                long fi_info = loader.ReadUInt32();
                // bwav position
                long fi_data = loader.ReadUInt32();

                if (fi_info > loader.Length || fi_data > loader.Length)
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

                    loader.Position = fi_info + amta_minfOffset + 6;
                    AMTAv5_Minf minfv5 = new AMTAv5_Minf();

                    minfv5.vers_maj = loader.ReadByte();
                    minfv5.vers_min = loader.ReadByte();
                    loader.ReadUInt32(); // file size

                    long minfOff_name = loader.Position + loader.ReadUInt32();

                    minfv5.unk0 = loader.ReadUInt32();
                    minfv5.sampleRate = loader.ReadUInt32();
                    minfv5.unk1 = loader.ReadUInt32();
                    minfv5.unk2 = loader.ReadUInt32();
                    minfv5.unk3 = loader.ReadUInt32();
                    minfv5.unk4 = loader.ReadUInt16();
                    minfv5.unk5 = loader.ReadByte();
                    minfv5.unk6 = loader.ReadByte();
                    minfv5.unk7 = loader.ReadUInt16();
                    minfv5.unk8 = loader.ReadUInt16();

                    long minfOff_table0 = loader.Position + loader.ReadUInt32();
                    long minfOff_table1 = loader.Position + loader.ReadUInt32();
                    long minfOff_table2 = loader.Position + loader.ReadUInt32();
                    long minfOff_pairTable = loader.Position + loader.ReadUInt32();
                    long minfOff_offsetTable = loader.Position + loader.ReadUInt32();
                    long minfOff_instrumentTable = loader.Position + loader.ReadUInt32();

                    minfv5.unk15 = loader.ReadUInt32();
                    minfv5.unk16 = loader.ReadUInt32();

                    // do the offset stuff now
                    long coolPos = loader.Position;

                    loader.Position = minfOff_name;
                    minfv5.name = loader.ReadString(BinaryStringFormat.ZeroTerminated).Trim();

                    // t0
                    loader.Position = minfOff_table0;
                    minfv5.ResMinfTable0 = new AMTAv5_ResMinfTable0();
                    ushort tablecount = loader.ReadUInt16();
                    minfv5.ResMinfTable0.unk0 = loader.ReadUInt16();

                    minfv5.ResMinfTable0.entries = new List<AMTAv5_ResMinfTable0Entry>();
                    for (int t = 0; t < tablecount; t++) {
                        minfv5.ResMinfTable0.entries.Add(new AMTAv5_ResMinfTable0Entry(
                            loader.ReadUInt32(),
                            loader.ReadUInt16(),
                            loader.ReadByte(),
                            loader.ReadByte(),
                            loader.ReadUInt16(),
                            loader.ReadUInt16()
                        ));
                    }

                    // t1
                    loader.Position = minfOff_table1;
                    minfv5.ResMinfTable1 = new AMTAv5_ResMinfTable1();
                    tablecount = loader.ReadUInt16();
                    minfv5.ResMinfTable1.unk0 = loader.ReadUInt16();

                    minfv5.ResMinfTable1.entries = new List<AMTAv5_ResMinfTable1Entry>();
                    for (int t = 0; t < tablecount; t++) {
                        minfv5.ResMinfTable1.entries.Add(new AMTAv5_ResMinfTable1Entry(
                            loader.ReadUInt32(),
                            loader.ReadUInt32(),
                            loader.ReadUInt32(),
                            loader.ReadUInt32()
                        ));
                    }

                    // t2
                    loader.Position = minfOff_table2;
                    minfv5.ResMinfTable2 = new AMTAv5_ResMinfTable2();
                    tablecount = loader.ReadUInt16();
                    minfv5.ResMinfTable2.unk0 = loader.ReadUInt16();

                    minfv5.ResMinfTable2.entries = new List<AMTAv5_ResMinfTable2Entry>();
                    for (int t = 0; t < tablecount; t++) {
                        minfv5.ResMinfTable2.entries.Add(new AMTAv5_ResMinfTable2Entry(
                            loader.ReadUInt32(),
                            loader.ReadUInt32(),
                            loader.ReadUInt32(),
                            loader.ReadSingle(),
                            loader.ReadUInt32()
                        ));
                    }

                    // pt
                    loader.Position = minfOff_pairTable;
                    minfv5.ResMinfPairTable = new AMTAv5_ResMinfPairTable();
                    tablecount = loader.ReadUInt16();
                    minfv5.ResMinfPairTable.unk0 = loader.ReadUInt16();

                    minfv5.ResMinfPairTable.entries = new List<AMTAv5_ResMinfPairTableEntry>();
                    for (int t = 0; t < tablecount; t++)
                    {
                        minfv5.ResMinfPairTable.entries.Add(new AMTAv5_ResMinfPairTableEntry(
                            loader.ReadUInt32(),
                            loader.ReadUInt32()
                        ));
                    }

                    // ot
                    loader.Position = minfOff_offsetTable;
                    minfv5.ResMinfOffsetTable = new AMTAv5_ResMinfOffsetTable();
                    tablecount = loader.ReadUInt16();
                    minfv5.ResMinfOffsetTable.unk0 = loader.ReadUInt16();

                    minfv5.ResMinfOffsetTable.entries = new List<uint>();
                    for (int t = 0; t < tablecount; t++)
                        minfv5.ResMinfOffsetTable.entries.Add(loader.ReadUInt32());

                    // ot
                    loader.Position = minfOff_instrumentTable;
                    minfv5.ResMinfInstrumentInfoTable = new AMTAv5_ResMinfInstrumentInfoTable();
                    tablecount = loader.ReadUInt16();
                    minfv5.ResMinfInstrumentInfoTable.unk0 = loader.ReadUInt16();

                    minfv5.ResMinfInstrumentInfoTable.entries = new List<AMTAv5_ResMinfInstrumentInfo>();
                    for (int t = 0; t < tablecount; t++) {
                        AMTAv5_ResMinfInstrumentInfo instinfo = new AMTAv5_ResMinfInstrumentInfo();
                        instinfo.unk0 = loader.ReadUInt32();
                        long okbuddy = loader.Position;
                        long instOff = loader.Position + loader.ReadUInt32();

                        //
                        loader.Position = instOff;
                        AMTAv5_ResMinfInstrument inst = new AMTAv5_ResMinfInstrument();
                        long instOff_name = loader.Position + loader.ReadUInt32();
                        inst.unk0 = loader.ReadUInt32();
                        inst.unk1 = loader.ReadUInt32();

                        loader.Position = instOff_name;
                        inst.name = loader.ReadString(BinaryStringFormat.ZeroTerminated).Trim();

                        instinfo.instrument = inst;

                        //
                        loader.Position = okbuddy + 4;
                        minfv5.ResMinfInstrumentInfoTable.entries.Add(instinfo);
                    }

                    // ok
                    loader.Position = coolPos;

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
                amtav5.reserved = reservedCrc32s.Contains(Crc32.Compute(fi_fileName));

                // BWAV DATA
                loader.Position = fi_data;

                Console.Write("BWAV HEADER: " + loader.ReadUInt32() + "\n");


                // first, get some bwav header data
                loader.Position = fi_data + 0x0E;
                uint bwavdata_channelCount = loader.ReadUInt16();
                Console.Write("bwav channels: " + bwavdata_channelCount + "\n");

                //uint fi_dataSize = 0x10;// + (0x70 * bwavdata_channelCount);
                long fi_lastByte = 0x10;

                loader.Position = fi_data + 0x10;
                for (uint c = 0; c < bwavdata_channelCount; c++) {
                    loader.Position += 0x0C;
                    uint bwavdata_sampleCount = loader.ReadUInt32();
                    loader.Position += 0x24;
                    uint bwavdata_sampleOffset = loader.ReadUInt32();
                    loader.Position += 0x14;

                    Console.Write("channel #" + c + " sample count: " + bwavdata_sampleCount + " and offset: " + bwavdata_sampleOffset);

                    fi_lastByte = bwavdata_sampleOffset;
                    uint samplecountttttt = (bwavdata_sampleCount / 14) * 8;
                    samplecountttttt += (bwavdata_sampleCount % 14 == 0) ? 0 : ((bwavdata_sampleCount % 14) / 2) + (bwavdata_sampleCount % 2) + 1;
                    Console.Write(" and sample length: " + samplecountttttt + "\n");
                    fi_lastByte += samplecountttttt;
                }

                loader.Position = fi_data;
                //

                Console.WriteLine(fi_fileName + ".bwav discovered. length " + fi_lastByte + ".");

                if (loader.Position + fi_lastByte > loader.Length)
                    Console.WriteLine("ummmmmm " + (loader.Position + fi_lastByte) + " over " + loader.Length + " real 3am?!?!?!?!");

                AudioEntry_ZS entry = new AudioEntry_ZS();
                entry.name = fi_fileName;
                entry.MetaData = amtav5;

                loader.Position = fi_data;

                if (fi_data > 0)
                {
                    BARSAudioFileZS spittingBars = new BARSAudioFileZS();
                    spittingBars.Load(loader);
                    spittingBars.AudioFileSize = (uint)fi_lastByte;

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

        void IFileDataZS.Save(FileSaverBarsZS saver)
        {
            // HEADER

            saver.ByteOrder = ByteOrder.LittleEndian;
            saver.Position = 0;
            saver.Write("BARS", BinaryStringFormat.NoPrefixOrTermination);
            saver.Position = 8;
            // endian
            saver.Write((byte)255);
            saver.Write((byte)254);
            // version
            saver.Write((byte)2);
            saver.Write((byte)1);
            // file count
            saver.Write((int)AudioEntries_ZS.Count);

            for (int i = 0; i < AudioEntries_ZS.Count; i++)
                saver.Write((int)Crc32.Compute(AudioEntries_ZS[i].name));

            //


            // OFFSETS

            FileSaverBarsZS.ItemEntry offs_General = new FileSaverBarsZS.ItemEntry("general");
            saver.items.Add(offs_General);

            for (int i = 0; i < AudioEntries_ZS.Count; i++)
            {
                FileSaverBarsZS.ItemEntry entry = new FileSaverBarsZS.ItemEntry("audio_" + AudioEntries_ZS[i].name);
                entry.newOffset("bamta_offset", saver.Position);
                saver.Write((int)0);
                entry.newOffset("bwav_offset", saver.Position);
                saver.Write((int)0);
                saver.items.Add(entry);
            }


            offs_General.newOffset("reserveCount", saver.Position);
            uint reserves = 0;
            saver.Write((int)0);
            for (int i = 0; i < AudioEntries_ZS.Count; i++)
            {
                if (!AudioEntries_ZS[i].MetaData.reserved)
                    continue;
                saver.Write((int)Crc32.Compute(AudioEntries_ZS[i].name));
                reserves++;
            }
            offs_General.SetOffsetByName("reserveCount", reserves);

            //


            // AMTA

            for (int i = 0; i < AudioEntries_ZS.Count; i++)
            {
                long lastLength = saver.BaseStream.Length;
                AudioEntry_ZS entry = AudioEntries_ZS[i];
                FileSaverBarsZS.ItemEntry itemEntry = saver.getEntry("audio_" + entry.name);
                itemEntry.SetOffsetByName("bamta_offset", (uint)saver.Position);

                saver.Write("AMTA", BinaryStringFormat.NoPrefixOrTermination);
                // endian
                saver.Write((byte)255);
                saver.Write((byte)254);
                // version
                saver.Write((byte)entry.MetaData.vers_min);
                saver.Write((byte)entry.MetaData.vers_maj);
                //
                itemEntry.newOffset("bamta_fileSize", saver.Position);
                saver.Write((int)0);

                saver.Write((int)entry.MetaData.unk0);

                itemEntry.newOffset("bamta_dataOffset", saver.Position);
                saver.Write((int)0);
                itemEntry.newOffset("bamta_markerOffset", saver.Position);
                saver.Write((int)0);
                itemEntry.newOffset("bamta_minfOffset", saver.Position);
                saver.Write((int)0);

                saver.Write((int)entry.MetaData.unk3);
                saver.Write((int)entry.MetaData.unk4);

                itemEntry.newOffset("bamta_pathOffset", saver.Position);
                saver.Write((int)0);
                saver.Write((int)Crc32.Compute(AudioEntries_ZS[i].name)); // path hash

                saver.Write((int)entry.MetaData.unk6);

                saver.Write((byte)entry.MetaData.type);
                saver.Write((byte)entry.MetaData.channels);
                saver.Write((byte)entry.MetaData.unk7);
                saver.Write((byte)entry.MetaData.flags);

                // arrays
                long epicNewPos = saver.Position;

                if (entry.MetaData.data != null)
                {
                    itemEntry.SetOffsetByName("bamta_dataOffset", ((uint)saver.Position - (uint)itemEntry.getOffset("bamta_offset").value));
                    saver.Write((int)entry.MetaData.data.unk0);
                    saver.Write((float)entry.MetaData.data.unk1);
                    saver.Write((float)entry.MetaData.data.unk2);
                    saver.Write((float)entry.MetaData.data.unk3);
                    saver.Write((float)entry.MetaData.data.unk4);
                    saver.Write((ushort)entry.MetaData.data.points.Count);
                    saver.Write((ushort)entry.MetaData.data.unk6);

                    for (int p = 0; p < entry.MetaData.data.points.Count; p++)
                    {
                        saver.Write((int)entry.MetaData.data.points[p].unk0);
                        saver.Write((float)entry.MetaData.data.points[p].unk1);
                    }
                }

                if (entry.MetaData.marker != null)
                {
                    itemEntry.SetOffsetByName("bamta_markerOffset", ((uint)saver.Position - (uint)itemEntry.getOffset("bamta_offset").value));
                    saver.Write((int)entry.MetaData.marker.markers.Count);
                    for (int l = 0; l < entry.MetaData.marker.markers.Count; l++) {
                        saver.Write((int)entry.MetaData.marker.markers[l].id);
                        itemEntry.newOffset("bamta_markers_nameOffset_" + l, saver.Position);
                        saver.Write((int)0);
                        saver.Write((int)entry.MetaData.marker.markers[l].start);
                        saver.Write((int)entry.MetaData.marker.markers[l].length);
                    }
                }

                if (entry.MetaData.minf != null)
                {
                    itemEntry.SetOffsetByName("bamta_minfOffset", ((uint)saver.Position - (uint)itemEntry.getOffset("bamta_offset").value));
                    long oldLength = saver.BaseStream.Length;
                    saver.Write("MINF", BinaryStringFormat.NoPrefixOrTermination);
                    // endian
                    saver.Write((byte)255);
                    saver.Write((byte)254);
                    // version
                    saver.Write((byte)entry.MetaData.minf.vers_maj);
                    saver.Write((byte)entry.MetaData.minf.vers_min);
                    //
                    itemEntry.newOffset("bamta_minf_fileSize", saver.Position);
                    saver.Write((int)0);
                    itemEntry.newOffset("bamta_minf_nameOffset", saver.Position);
                    saver.Write((int)0);
                    //
                    saver.Write((int)entry.MetaData.minf.unk0);
                    saver.Write((int)entry.MetaData.minf.sampleRate);
                    saver.Write((int)entry.MetaData.minf.unk1);
                    saver.Write((int)entry.MetaData.minf.unk2);
                    saver.Write((int)entry.MetaData.minf.unk3);
                    saver.Write((ushort)entry.MetaData.minf.unk4);
                    saver.Write((byte)entry.MetaData.minf.unk5);
                    saver.Write((byte)entry.MetaData.minf.unk6);
                    saver.Write((ushort)entry.MetaData.minf.unk7);
                    saver.Write((ushort)entry.MetaData.minf.unk8);
                    //
                    itemEntry.newOffset("bamta_minf_table_off0", saver.Position);
                    saver.Write((int)0);
                    itemEntry.newOffset("bamta_minf_table_off1", saver.Position);
                    saver.Write((int)0);
                    itemEntry.newOffset("bamta_minf_table_off2", saver.Position);
                    saver.Write((int)0);
                    itemEntry.newOffset("bamta_minf_table_offPair", saver.Position);
                    saver.Write((int)0);
                    itemEntry.newOffset("bamta_minf_table_offOffset", saver.Position);
                    saver.Write((int)0);
                    itemEntry.newOffset("bamta_minf_table_offInstrumentInfo", saver.Position);
                    saver.Write((int)0);

                    //
                    saver.Write((int)entry.MetaData.minf.unk15);
                    saver.Write((int)entry.MetaData.minf.unk16);

                    //
                    #region minfTableWrites_Indexed
                    itemEntry.SetOffsetByName("bamta_minf_table_off0", (uint)saver.Position - (uint)itemEntry.getOffset("bamta_minf_table_off0").at);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfTable0.entries.Count);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfTable0.unk0);
                    for (int a = 0; a < entry.MetaData.minf.ResMinfTable0.entries.Count; a++) {
                        AMTAv5_ResMinfTable0Entry e = entry.MetaData.minf.ResMinfTable0.entries[a];
                        saver.Write((int)e.unk0);
                        saver.Write((ushort)e.unk1);
                        saver.Write((byte)e.unk2);
                        saver.Write((byte)e.unk3);
                        saver.Write((ushort)e.unk4);
                        saver.Write((ushort)e.unk5);
                    }

                    itemEntry.SetOffsetByName("bamta_minf_table_off1", (uint)saver.Position - (uint)itemEntry.getOffset("bamta_minf_table_off1").at);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfTable1.entries.Count);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfTable1.unk0);
                    for (int a = 0; a < entry.MetaData.minf.ResMinfTable1.entries.Count; a++)
                    {
                        AMTAv5_ResMinfTable1Entry e = entry.MetaData.minf.ResMinfTable1.entries[a];
                        saver.Write((int)e.unk0);
                        saver.Write((int)e.unk1);
                        saver.Write((int)e.unk2);
                        saver.Write((int)e.unk3);
                    }

                    itemEntry.SetOffsetByName("bamta_minf_table_off2", (uint)saver.Position - (uint)itemEntry.getOffset("bamta_minf_table_off2").at);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfTable2.entries.Count);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfTable2.unk0);
                    for (int a = 0; a < entry.MetaData.minf.ResMinfTable2.entries.Count; a++)
                    {
                        AMTAv5_ResMinfTable2Entry e = entry.MetaData.minf.ResMinfTable2.entries[a];
                        saver.Write((int)e.unk0);
                        saver.Write((int)e.unk1);
                        saver.Write((int)e.unk2);
                        saver.Write((float)e.unk3);
                        saver.Write((int)e.unk4);
                    }
                    #endregion


                    #region minfTableWrites_PairOffset
                    itemEntry.SetOffsetByName("bamta_minf_table_offPair", (uint)saver.Position - (uint)itemEntry.getOffset("bamta_minf_table_offPair").at);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfPairTable.entries.Count);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfPairTable.unk0);
                    for (int a = 0; a < entry.MetaData.minf.ResMinfPairTable.entries.Count; a++)
                    {
                        AMTAv5_ResMinfPairTableEntry e = entry.MetaData.minf.ResMinfPairTable.entries[a];
                        saver.Write((int)e.unk0);
                        saver.Write((int)e.unk1);
                    }

                    itemEntry.SetOffsetByName("bamta_minf_table_offOffset", (uint)saver.Position - (uint)itemEntry.getOffset("bamta_minf_table_offOffset").at);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfOffsetTable.entries.Count);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfOffsetTable.unk0);
                    for (int a = 0; a < entry.MetaData.minf.ResMinfOffsetTable.entries.Count; a++)
                        saver.Write((int)entry.MetaData.minf.ResMinfOffsetTable.entries[a]);
                    #endregion

                    #region mindTableWrites_Instrument
                    itemEntry.SetOffsetByName("bamta_minf_table_offInstrumentInfo", (uint)saver.Position - (uint)itemEntry.getOffset("bamta_minf_table_offInstrumentInfo").at);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfInstrumentInfoTable.entries.Count);
                    //saver.Write((ushort)0);
                    saver.Write((ushort)entry.MetaData.minf.ResMinfInstrumentInfoTable.unk0);

                    for (int a = 0; a < entry.MetaData.minf.ResMinfInstrumentInfoTable.entries.Count; a++)
                    {
                        AMTAv5_ResMinfInstrumentInfo e = entry.MetaData.minf.ResMinfInstrumentInfoTable.entries[a];
                        saver.Write((int)e.unk0);
                        itemEntry.newOffset("instInfo_" + a, saver.Position);
                        saver.Write((int)0);
                    }

                    for (int a = 0; a < entry.MetaData.minf.ResMinfInstrumentInfoTable.entries.Count; a++)
                    {
                        AMTAv5_ResMinfInstrumentInfo e = entry.MetaData.minf.ResMinfInstrumentInfoTable.entries[a];
                        itemEntry.SetOffsetByName("instInfo_" + a, (uint)saver.Position - (uint)itemEntry.getOffset("instInfo_" + a).at);

                        itemEntry.newOffset("instInfo_" + a + "_name", saver.Position);
                        saver.Write((int)0);
                        saver.Write((int)e.instrument.unk0);
                        saver.Write((int)e.instrument.unk1);
                    }

                    for (int a = 0; a < entry.MetaData.minf.ResMinfInstrumentInfoTable.entries.Count; a++)
                    {
                        AMTAv5_ResMinfInstrumentInfo e = entry.MetaData.minf.ResMinfInstrumentInfoTable.entries[a];
                        itemEntry.SetOffsetByName("instInfo_" + a + "_name", (uint)saver.Position - (uint)itemEntry.getOffset("instInfo_" + a + "_name").at);
                        saver.Write(e.instrument.name, BinaryStringFormat.ZeroTerminated);
                    }
                    #endregion

                    //
                    itemEntry.SetOffsetByName("bamta_minf_fileSize", (uint)(saver.BaseStream.Length - oldLength));

                }

                if (entry.MetaData.minf != null)
                {
                    itemEntry.SetOffsetByName("bamta_minf_nameOffset", (uint)saver.Position - (uint)itemEntry.getOffset("bamta_minf_nameOffset").at);
                    saver.Write(entry.MetaData.minf.name, BinaryStringFormat.ZeroTerminated);
                }

                // name
                itemEntry.SetOffsetByName("bamta_pathOffset", (uint)saver.Position - (uint)itemEntry.getOffset("bamta_pathOffset").at);
                Console.WriteLine("don't ya wanna write the " + entry.name + " at " + (uint)saver.Position);
                saver.Write(entry.name, BinaryStringFormat.ZeroTerminated);

                if (entry.MetaData.marker != null)
                {
                    string last_intendedName = null;
                    for (int l = 0; l < entry.MetaData.marker.markers.Count; l++) {
                        string intendedName = entry.MetaData.marker.markers[l].name;
                        if (last_intendedName == intendedName && l > 0)
                        {
                            itemEntry.SetOffsetByName("bamta_markers_nameOffset_" + l, (uint)itemEntry.getOffset("bamta_markers_nameOffset_" + (l - 1)).value);
                            continue;
                        }
                        itemEntry.SetOffsetByName("bamta_markers_nameOffset_" + l, ((uint)saver.Position - (uint)itemEntry.getOffset("bamta_markers_nameOffset_" + l).at));
                        saver.Write(intendedName, BinaryStringFormat.ZeroTerminated);
                    }
                }

                //
                itemEntry.SetOffsetByName("bamta_fileSize", (uint)(saver.BaseStream.Length - lastLength));
            }

            //


            // BWAV

            for (int i = 0; i < AudioEntries_ZS.Count; i++)
            {
                AudioEntry_ZS entry = AudioEntries_ZS[i];
                FileSaverBarsZS.ItemEntry itemEntry = saver.getEntry("audio_" + entry.name);
                itemEntry.SetOffsetByName("bwav_offset", (uint)saver.Position);
                saver.Write(AudioEntries_ZS[i].AudioFile.data);
            }

            //
         }

        public void Save(Stream stream) {
            FileSaverBarsZS fileLoader = new FileSaverBarsZS(stream, this);
            fileLoader.Execute();
        }
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

        void IFileDataZS.Save(FileSaverBarsZS saver)
        {
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
