### Cinematics
- Allows replacing some video cutscenes in the game with custom ones 
To use, add the appropriate file in `Mods/Custom Knight/Skins/<skinname>/Cinematics`  or if you want to replace them globally then `Mods/Custom Knight/Cinematics`

Note: the existing Cinematics cannot be dumped but you may acquire the original videos [here](https://github.com/PrashantMohta/HollowKnight.CustomKnight/releases/tag/v2.2.0)

Currently Supported Cinematics : 
```
Prologue.webm
Intro.webm
StagTunnelRun.webm
CharmSlugKiss.webm
Nailsmith.webm
NailsmithPaint.webm
Blacksmith.webm
FinalA.webm
FinalB.webm
FinalC.webm
FinalD.webm
FinalE.webm
MrMushroom.webm
Telescope.webm
Fountain.webm
MaskShatter.webm
```
Required Format :
```
Video: 1080p, 30fps, VP8 encoding
Audio: 48000hz sample rate, 2 channels, Vorbis encoding
Container: WebM
```
FFMPEG command to get this Format:

`ffmpeg -i <INFILE> -c:v libvpx -crf 10 -b:v 8M -c:a libvorbis -q 6 <OUTFILE>.webm`
