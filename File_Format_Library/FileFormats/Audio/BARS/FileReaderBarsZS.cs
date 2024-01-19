using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Syroot.BinaryData;
using BarsLib.IO;

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
    public interface IFileDataZS
    {
        void Load(FileLoaderBarsZS loader);

        //void Save(FileSaver saver);
    }
}
