using System.Collections.Generic;
using System.IO;
using System.Linq;
using VGAudio.Containers;
using VGAudio.Containers.NintendoWare;
using VGAudio.Containers.NintendoWare.Structures;
using VGAudio.Formats;
using VGAudio.Formats.GcAdpcm;
using VGAudio.Utilities;
using VGAudio.Codecs.GcAdpcm;
using System;
using System.Security.AccessControl;
using Assimp;
using System.Windows.Controls;

namespace Toolbox.Library
{
    public class BWavStructure
    {
        public ushort BwavVersion { get; set; }
        public uint Crc32Hash { get; set; }
        public bool Prefetch { get; set; }
        public ushort ChannelCount { get; set; }
        public Endianness Endianness { get; set; }
        public NwVersion Version { get; set; }

        public StreamInfo StreamInfo { get; set; }
        public TrackInfo TrackInfo { get; set; }
        public List<ChannelInfo> ChannelInfos { get; set; }
    }

    public class BWavConfiguration : Configuration
    {

        private int _loopPointAlignment = -1;
        public Endianness? Endianness { get; set; }
        public NwVersion Version { get; set; }
        public NwCodec Codec { get; set; } = NwCodec.GcAdpcm;

        public int LoopPointAlignment
        {
            get
            {
                if (_loopPointAlignment == -1)
                {
                    switch (Codec)
                    {
                        case NwCodec.GcAdpcm: return GcAdpcmMath.ByteCountToSampleCount(8192);
                        case NwCodec.Pcm16Bit: return 8192 / 2;
                        case NwCodec.Pcm8Bit: return 8192;
                    };
                    return 0;
                }

                return _loopPointAlignment;
            }
            set
            {
                _loopPointAlignment = value;
            }
        }
    }

    /*
        https://gota7.github.io/Citric-Composer/specs/binaryWav.html

        Remember: The file goes as such:
            - FileHeader (Fixed 0x00 pos, fixed 0x10 length)
            - ChannelInfo[ChannelCount]
            - Padding
            - ChannelSamples[ChannelCount]
    */
    public class BWavReader : AudioReader<BWavReader, BWavStructure, BWavConfiguration>
    {
        protected override BWavStructure ReadFile(Stream stream, bool readAudioData = true)
        {
            Endianness endianness;
            BinaryReader br;
            using (br = Helpers.GetBinaryReader(stream, Endianness.LittleEndian))
            {
                string headerMagic = br.ReadUTF8(4);
                if (headerMagic != "BWAV")
                    throw new InvalidDataException("File has no BWAV header!");
                ushort num = br.ReadUInt16();
                endianness = (num == 65534) ? Endianness.BigEndian : Endianness.LittleEndian;
            }

            br = Helpers.GetBinaryReader(stream, endianness);
            BWavStructure bwavStructure = new BWavStructure { Endianness = endianness };
            ReadHeader(br, bwavStructure);
            return bwavStructure;
        }

        private static void ReadHeader(BinaryReader reader, BWavStructure structure)
        {
            reader.BaseStream.Position = 6;
            structure.BwavVersion = reader.ReadUInt16(); // version?
            structure.Crc32Hash = reader.ReadUInt32(); // crc32 hash (all sample data in one large byte array without padding hashed)
            structure.Prefetch = reader.ReadUInt16() == 1;
            structure.ChannelCount = reader.ReadUInt16();

            Console.WriteLine("BWAV INFO SO FAR!!\nBwavVersion = " + structure.BwavVersion + ", Crc32Hash = " + structure.Crc32Hash
                + ", " + (structure.Prefetch ? "Prefetched" : "Not a Prefetch") + ", ChannelCount" + structure.ChannelCount);

            for (int i = 0; i < structure.ChannelCount; i++) {
                ChannelInfo ci = new ChannelInfo();

                reader.ReadUInt16(); // should be 1
                reader.ReadUInt16(); // channel panning (0 left, 1 right, 2 mid)
                reader.ReadUInt32(); // sample rate
                reader.ReadUInt32(); // numb. samples in non-prefetch?
                reader.ReadUInt32(); // numb. samples in this?

                // coefficients???
                for (int o = 0; o < 8; o++)
                    reader.ReadUInt16();

                reader.ReadUInt32(); // absolute start of offset data in non-prefetch??
                reader.ReadUInt32(); // ummmmmm....??? same thing for this?
                reader.ReadUInt32(); // 1 = loop?
                reader.ReadUInt32(); // loop end sample? (0xFFFFFFFF if no loop)
                reader.ReadUInt32(); // loop start sample?? (0 if no loop)
                reader.ReadUInt16(); // i don't even know
                reader.ReadUInt16(); // ...
                reader.ReadUInt16(); // dude?
                reader.ReadUInt16(); // >!rm,tg.>R<1f/gndjh.2.1?????
            }
        }

        protected override IAudioFormat ToAudioStream(BWavStructure structure)
        {
           /* switch (structure.StreamInfo.Codec)
            {
                case NwCodec.GcAdpcm: return ToAdpcmStream(structure);
                case NwCodec.Pcm16Bit: return ToPcm16Stream(structure);
                case NwCodec.Pcm8Bit: return ToPcm8Stream(structure);
            }*/
            return null;
        }
    }
}
