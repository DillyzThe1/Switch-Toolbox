# Switch-Toolbox
A tool to edit many formats of Nintendo Switch, 3DS and Wii U. 

## Notice

**This is a fork of the original [Switch Toolbox](https://github.com/KillzXGaming/Switch-Toolbox/).**

The program felt a little under-worked with newer titles, which I find that unfortunate due to my advancing needs.<br>
So what was the solution on January 1st, 2024 when I discovered that the original Switch Toolbox was discontinued?<br>
I decided to take matters into my own hands.

Even if I may not be the best tool developer or have the best Switch/Wii U/3DS understandings, I can 100% help myself and my friends to the convienence of whatever I please.

Please support and respect the original creator's decisions alongside my own.<br>
[![KillzXGaming](https://avatars.githubusercontent.com/u/13475262?v=4)](https://github.com/KillzXGaming) <-- really cool guy who made the original

Thank you for this wonderful tool, KillzXGaming.

# Download
https://github.com/DillyzThe1/Switch-Toolbox/releases

Keep in mind that, just like the original, this tool is still (and all of the new changes are) very experimental.<br>
If something breaks from a commit, or if you would like a dev build, you can download manually from [here](https://ci.appveyor.com/project/DillyzThe1/switch-toolbox/history).<br>
Just select a commit, go to artifacts tab and download the zip.<br>
If you did find an issue with the latest build, please report your [issues here](https://github.com/DillyzThe1/Switch-Toolbox/issues/new) on Github.

## Discord
DillyzThe1 (New Switch Toolbox Fork): https://discord.gg/49NFTwcYgZ<br>
KillzXGaming (Original Switch Toolbox Creator): https://discord.gg/eyvGXWP

## Support 
If you'd like to support me or get help on the new version's features, you can join the [Modding Discord!](https://discord.gg/49NFTwcYgZ)<br>
If you'd like to support the [original creator](https://github.com/KillzXGaming/), you can [donate!](https://www.paypal.com/donate?business=TCG7P6PH6V3PU&currency_code=USD)

## New Features

Changes to file formats:
- BARS.ZS Support
	- You can now open, view, dump, and even modify or save Bars.ZS files!
	- This includes being able to see ALL of the Table0, Table1, Table2, PairTable, OffsetTable, & InstrumentInfoTable data.
	- A convenient "Duplicate" option is also available.
	- Saving/Re-encoding is FULLY supported!
- ~~Viewing BWAV files~~
	- DELAYED: I don't feel like fully implementing this yet, as I don't feel like it's worth my time until the Bars.ZS support is finished.

Changes to the hex editor:
- Edit > Trim File now exists.
	- This feature will cut off any data beyond a chosen point, intended for prefetch files.
	- A popup will ask how long you want the trimmed file to be.

## Original Features

This tool currently features:
- BFRES
   - Fully supports Wii U and Switch
   - Model importing (DAE, FBX, OBJ, and CSV)
   - Material editing (Render info, texture mapping, parameters, etc.)
   - Material copying
   - Animation and model sub section can be exported / imported
   - Can delete, add, replace individual objects from an FMDL
   - Can create new sub sections and data
   - Can preview skeletal, SRT, param, texture pattern, and bone visual animations. (param animations vary)
   - Can export and import fully rigged models with bone support
   - Can convert gif files to texture pattern animations, very WIP atm
   
Can edit formats:
- BFRES
- BNTX
- BFLYT
- BFLAN
- NUTEXB
- XTX
- GTX
- SARC
- BARS
- KCL
- BFLIM
- GFPAK
- BEA
- AAMP (Switch, 3DS and Wii U)
- BYAML/BYML (Switch, 3DS and Wii U)
- PTCL (Switch, 3DS and Wii U)
- TMPK
- PAK / IGA (Crash Bandicoot / Crash Team Racing)
- IGZ Textures (Crash Bandicoot / Crash Team Racing)
- BFFNT (Textures only)

Can preview:
- BCRES
  * Models, materials, and textures.
- BFSHA
  * Can view options, samplers, attributes, and uniform blocks.
- BNSH
  * Can extract shader vertex and fragment shaders from variations/programs.
- SHARCFB and SHARC
  * Basic preview of some shader program data.
  * Can edit both v1 and v2 AAMP (Wii U and Switch)
- EFC
  * Can preview effect tables and link PTCL.
- NUT
  * Can preview NTWU, NTP3, and NTWD variants. Editing will be implemented soon.
- MSBT
  * Very basic previewing.
- MP3, OGG, IDSP, HPS, WAV, BFWAV, BFSTM, BCWAV, BCWAV
  * Can listen to audio and convert between certain formats. Thanks to VGAudio and CSCore
- NARC
- SP2 (Team Sonic Racing)
- GFMDL
- TEX (3DS)
- NUSHDB (Switch Namco Shaders)
- SDF (Snow Drop Engine)
- NCA
- NSP
- IStorage
- NXARC
- LZARC
- IGA .pak
- RARC
- GMX (TPHD Models)
- MKAGPDX Model .bin files
- MKAGPDX Archive .pac files
- ME01 and SA01 archives
- Luigi's Mansion 2: Dark Moon (archives, models, and textures)
- TPL
- TXE
- BTI
- G1T
- CMB (OoT3D, MM3D, LM3DS)
- CTXB (OoT3D, MM3D, LM3DS)
- GAR (OoT3D, MM3D, LM3DS)
- ZSI (OoT3D, MM3D)
- BinGZ (Hyrule Warriors)
- PAC (Sonic Forces)
- GameCube ISOs (file system)


## Tutorials
[New Switch Toolbox](https://github.com/DillyzThe1/Switch-Toolbox/wiki)<br>
[Original Switch Toolbox](https://github.com/KillzXGaming/Switch-Toolbox/wiki)
   
## Issues or Requests
https://github.com/DillyzThe1/Switch-Toolbox/issues
   
## Building
To build make sure you have Visual Studio installed (I use 2017, older versions may not work) and open the solution (.sln), then build the solution as release. It should compile properly on the latest.

In the event that the tool cannot compile, check references. All the libraries used are stored in Switch-Toolbox/Lib folder. 

Also, while compiling, Visual Studio might throw errors about files coming from external sources (ie. the web) and will therefore fail to read and compile them. In this event, go into the project root folder and run: `Get-ChildItem -Path "C:\\Full\\Path\\To\\Folder" -Recurse | Unblock-File`.

## Credits
- KillzXGaming (Original Switch Toolbox)
- DillyzThe1 (New Switch Toolbox fork)


- Smash Forge Devs (SMG, Ploaj,  jam1garner, smb123w64gb, etc.) for some code ported over, specifically animation stuff, ETC1 encoder and some rendering.
- Ploaj for a base on the DAE writer.
- Assimp devs for their massive asset library!
- Wexos (helped figure out a few things, i.e. format list to assign each attribute)
- JuPaHe64 for the base 3D renderer.
- Every File Explorer devs (Gericom) for Yaz0 and bitmap font stuff.
- exelix for BYAML, SARC and KCL library.
- Syroot for helpful IO extensions and libraries.
- GDKChan for SPICA library (used for BCH), PICA shaders stuff used with BCRES, structs for BCRES, and some DDS decode methods.
- AboodXD for some foundation stuff with exelix's SARC library, Wii U (GPU7) and Switch (Tegra X1) textures swizzling, reading/converting uncompressed types for DDS, and documentation for GTX, XTX, and BNTX. Library for Yaz0 made by AboodXD and helped port it to the tool.
- MelonSpeedruns for Switch Toolbox logo.
- BrawlBox team for brawl libraries used for BRRES parsing.
- Sage of Mirrors for SuperBMDLib. 
- Ambrosia for BTI and TXE support.
- Kuriimu for some IO and file parsing help.
- Skyth and Radfordhound for PAC documentation.
- Ac_K for ASTC decoder c# port from Ryujinx. 
- pkNX and kwsch for Fnv hashing and useful pkmn code/structure references.
- Dragonation for useful code on the structure for some flatbuffers in pokemon switch
- mvit and Rei for help with gfpak hash strings and also research for formats.
- QuickBMS for some compression code ported (LZ77 WII)

##  Resources
- [TreeView Icons by icons8](https://icons8.com/)
- Smash Forge (Currently placeholders)

## Documentation (File Formats)
- http://mk8.tockdom.com/wiki/
- https://wiki.oatmealdome.me/Category:File_formats
- https://github.com/Kinnay/Nintendo-File-Formats/wiki
- http://Avsys.xyz/wiki/Category:File_Formats

## Libraries
- [SuperBMDLib](https://github.com/Sage-of-Mirrors/SuperBMD)
- [BrawlLib (for BRRES section conversion)](https://github.com/libertyernie/brawltools)
- [exelix (SARC, KCL, and BYML libraries)](https://github.com/exelix11/EditorCore/tree/master/FileFormatPlugins)
- [ZstdNet (compression)](https://github.com/skbkontur/ZstdNet)
- [Be.HexEditor by Bernhard Elbl](https://sourceforge.net/projects/hexbox/)
- [GL Editor Framework by jupahe64](https://github.com/jupahe64/GL_EditorFramework)
- [WeifenLuo for docking suite](http://dockpanelsuite.com/)
- [SF Graphics by SMG (experimental)](https://github.com/ScanMountGoat/SFGraphics) (currently just a placeholder for shader workflow and some useful things)
- [NAudio (Audio & MIDI Library)](https://github.com/naudio/NAudio)
- [VGAudio](https://github.com/Thealexbarney/VGAudio)
- [CSCore](https://github.com/filoe/cscore)
- [Assimp](https://bitbucket.org/Starnick/assimpnet/src/master/)
- [OpenTK](https://github.com/opentk/opentk)
- [BezelEngineArchive Library](https://github.com/KillzXGaming/BEA-Library-Editor)
- [Syroot BinaryData](https://gitlab.com/Syroot/BinaryData)
- [Syroot Maths](https://gitlab.com/Syroot/Maths)
- [Syroot BFRES Library (Wii U)](https://gitlab.com/Syroot/NintenTools.Bfres)
- [LibHac](https://github.com/Thealexbarney/LibHac)
- [ASTC Decoder](https://github.com/Ryujinx/Ryujinx/blob/b2b736abc2569ab5d8199da666aef8d8394844a0/Ryujinx.Graphics/Graphics3d/Texture/AstcDecoder.cs)

## Helpful Tools
- [Kuriimu](https://github.com/IcySon55/Kuriimu)

License
 in Switch_Toolbox\Lib\Licenses
 
 Please note if you do not want your library used or if I'm missing credits! 
