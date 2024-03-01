#pragma author DillyzThe1
#pragma description The v5 version of the BARS file.
#include <type/magic.pat>

struct Header {
    u32 fileSize; // file length in bytes
    be u16 endian; // 65534 for BIG, other for SMALL.
    u16 version; // give 0x0201 pls
    u32 fileCount; // the number of audio files present
};

struct FileInfo {
    s32 idk;
};

struct AMTA {
    type::Magic<"AMTA"> magic;
    be u16 endian; // 65534 for BIG, other for SMALL.
    be u16 version;
    u32 amtaSize;
    padding[4];
    u32 unk_1; // almost always 52 (0x34000000) or 56 (0x38000000)
    padding[4];
    u32 unk_2;
    padding[8]; // UNKNOWN BYTES
    s32 nameOffset;
    char fileName[] @ addressof(nameOffset) + nameOffset;
    
    //u8 awesomeBytes[amtaSize] @ addressof(this);
};

struct BWAV {
    type::Magic<"BWAV"> magic;
    be u16 endian; // 65534 for BIG, other for SMALL.
    u16 version;
    padding[20]; // UNKNOWN BYTES
    u32 fileSize;
    u8 bwavBytes[((fileSize + 13) / 14) * 8 + 128] @ addressof(this);
};

struct FileSegment {
    s32 amtaPos;
    s32 bwavPos;
    
    AMTA amta @ amtaPos;
    BWAV bwav @ bwavPos;
};

struct BarsV5 {
    type::Magic<"BARS"> magic;
    Header header;
    FileInfo fileInfos[header.fileCount];
    FileSegment files[header.fileCount];
    //AMTA amta;
};

BarsV5 barsv5 @ 0x00;