using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Syroot.BinaryData;
using BarsLib.IO;
using static LibHac.ExternalKeys;

// STOLEN FROM BarsLib.IO.FileLoader BC IT WOULDN'T WORK AHHHHHHH
namespace FirstPlugin
{
    public class FileLoaderBarsZS : BinaryDataReader
    {
        private IDictionary<uint, IFileDataZS> _dataMap;

        internal BarsZsFile BARS { get; }

        public FileLoaderBarsZS(Stream stream, BarsZsFile bars, bool leaveOpen = false)
            : base(stream, Encoding.ASCII, leaveOpen)
        {
            base.ByteOrder = ByteOrder.BigEndian;
            BARS = bars;
            _dataMap = new Dictionary<uint, IFileDataZS>();
        }

        internal FileLoaderBarsZS(string fileName, BarsZsFile bars)
            : this(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read), bars)
        {
        }

        internal T Load<T>(bool SeekOffset = true, uint offset = 0u) where T : IFileDataZS, new()
        {
            if (SeekOffset)
            {
                offset = ReadUInt32();
                if (offset == 0 || offset == uint.MaxValue)
                {
                    return default(T);
                }

                using (TemporarySeek(offset, SeekOrigin.Begin))
                {
                    return ReadResData<T>();
                }
            }

            if (offset != 0)
            {
                using (TemporarySeek(offset, SeekOrigin.Begin))
                {
                    return ReadResData<T>();
                }
            }

            return ReadResData<T>();
        }

        internal void Execute()
        {
            ((IFileDataZS)BARS).Load(this);
        }

        internal void CheckSignature(string validSignature)
        {
            string text = ReadString(4, Encoding.ASCII);
            if (text != validSignature)
            {
                throw new Exception("Invalid signature, expected '" + validSignature + "' but got '" + text + "'.");
            }
        }

        internal T LoadCustom<T>(Func<T> callback, uint? offset = null)
        {
            offset = offset ?? ReadUInt32();
            if (offset == 0)
            {
                return default(T);
            }

            using (TemporarySeek(offset.Value, SeekOrigin.Begin))
            {
                return callback();
            }
        }

        internal string LoadStringInstance(Encoding encoding = null)
        {
            ReadUInt32();
            return ReadString(BinaryStringFormat.ZeroTerminated);
        }

        internal string GetAudioType()
        {
            return ReadString(4, Encoding.ASCII);
        }

        internal string LoadString(Encoding encoding = null)
        {
            long num = ReadInt64();
            if (num == 0L)
            {
                return null;
            }

            encoding = encoding ?? base.Encoding;
            using (TemporarySeek(num, SeekOrigin.Begin))
            {
                ReadInt16();
                return ReadString(BinaryStringFormat.ZeroTerminated, encoding);
            }
        }

        private T ReadResData<T>() where T : IFileDataZS, new()
        {
            uint key = (uint)base.Position;
            T val = new T();
            val.Load(this);
            if (_dataMap.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            _dataMap.Add(key, val);
            return val;
        }
    }

    public class FileSaverBarsZS : BinaryDataWriter
    {
        public class ItemEntry
        {
            public class ItemOffset {
                public string name;
                public long at;
                public uint value;

                public ItemOffset(string name, long at, uint value) {
                    this.name = name;
                    this.at = at;
                    this.value = value;
                }
            }
            internal List<ItemOffset> offsets;
            public string id;

            public ItemEntry(string id) {
                offsets = new List<ItemOffset>();
                this.id = id;
            }

            public void SetOffsetByName(string target, uint newValue) {
                for (int i = 0; i < offsets.Count; i++)
                    if (offsets[i].name == target)
                        offsets[i].value = newValue;
            }

            public void newOffset(string name, long at, uint value) {
                offsets.Add(new ItemOffset(name, at, value));
            }

            public void newOffset(string name, long at)
            {
                offsets.Add(new ItemOffset(name, at, 0));
            }

            public ItemOffset getOffset(string name) {
                for (int i = 0; i < offsets.Count; i++)
                    if (offsets[i].name == name)
                        return offsets[i];
                return null;
            }
        }

        internal BarsZsFile BARS { get; }
        public List<ItemEntry> items { get; private set; }

        public ItemEntry getEntry(string id) {
            for (int i = 0; i < items.Count; i++)
                if (items[i].id == id)
                    return items[i];
            return null;
        }

        public FileSaverBarsZS(Stream stream, BarsZsFile bars, bool leaveOpen = false) : base(stream, Encoding.ASCII, leaveOpen)
        {
            base.ByteOrder = ByteOrder.BigEndian;
            BARS = bars;
            items = new List<ItemEntry>();
        }

        public void Execute() {
            ((IFileDataZS)BARS).Save(this);

            // offset time
            for (int i = 0; i < items.Count; i++) {
                if (items[i].offsets != null) {
                    for (int o = 0; o < items[i].offsets.Count; o++) {
                        base.Position = items[i].offsets[o].at;
                        Write((int)items[i].offsets[o].value);
                    }
                }
            }
            //
            base.Position = 4;
            Write((uint)BaseStream.Length);
            Flush();
        }
    }
    public interface IFileDataZS
    {
        void Load(FileLoaderBarsZS loader);
        void Save(FileSaverBarsZS saver);
    }
}
