#pragma author DillyzThe1
#pragma BFRES.ZS Reverse Engineering.
#include <type/magic.pat>

struct FMDL_FVTX {
    be type::Magic<"FVTX"> magic;
    u8 atrCount;
    u8 bufCount;
    u16 sIndex;
    u32 vertNum;
    u8 vertSkinCount;
    padding[3];
    s32 atrArrayOffset;
    s32 atrIndexGroupOffset;
    s32 bufferArrayOffset;
    u32 userPointer;
};

struct FMDL {
    be type::Magic<"FMDL"> magic;
    s32 fileNameOffset;
    s32 filePathOffset;
    if (filePathOffset != 0)
        char filePath[] @ filePathOffset + 2;
    u32 fsklOffset;
    u32 fvtxArrayOffset;
    u32 fshpIndexGroupOffset;
    u32 fmatIndexGroupOffset;
    u32 userDataIndexGroupOffset;
    u16 fvtxCount;
    u16 fshpCount;
    u16 fmatCount;
    u16 userDataEntryCount;
    u32 vertNum;
    u32 userPointer;

    FMDL_FVTX fvtxArray @ 0x00000280;
};

struct BFRES {
    be type::Magic<"FRES"> magic;
    be u32 num;
    be u32 version;
    be u16 endianness;
    u16 header_length; // always 0x0010?
    u32 fileLength;
    u16 flag;
    u16 blockOffset;
    u32 num3;
    u32 num4;
    
    // name
    u32 fileNameOffset;
    char fileName[] @ fileNameOffset + 2;
    
    s32 strTableLength;
    s32 strTableOffset;
    // index groups
    s32 fileOffsets[12];
    u16 fileCounts[12];
    
    u32 userPointer;
};

BFRES bfres @ 0x00;
FMDL fmdl @ 0xF0;