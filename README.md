# CustomKnight 
A Hollow Knight mod to load custom texture sheets for the player

### Installing the Mod Manually:

1. Extract the .zip file.
2. Place the output folder under the Mods/CustomKnight directory:

``` 
Windows		C:\Program Files (x86)\Steam\steamapps\common\Hollow Knight\hollow_knight_Data\Managed\Mods\CustomKnight\
Mac		~/Library/Application Support/Steam/steamapps/common/Hollow Knight/hollow_knight.app/hollow_knight_Data/Resources/Data/Managed/Mods/CustomKnight/
Linux		~/.local/share/Steam/steamapps/common/Hollow Knight/hollow_knight_Data/Managed/Mods/CustomKnight/
```

3. Open the game (may take a while) and load a save.
4. Pause the game and go to Settings > Mods > CustomKnight & choose your desired skin.

### Installing Skins:

1. Download the skin 
2. Place the skin's images in a directory that is the same name as the skin
3. Copy this directory to the Skins directory under the game's Mods/CustomKnight directoy (mentioned earlier)

Make sure that the folder you copy opens directly to the image files themselves. That is, when you double click on the folder, it opens up to the PNGs themselves and not to another folder or zip. 

### Creating your own skins:

1. Edit the existing skin images in your photo editing software of choice.
2. Save each skin under the exact same name as the PNG file that it is overriding in the Defaults folder, e.g. if you 
   are creating a skin for Grimmchild, save the image as "Grimm.png".
3. Put the edited images in a folder with the name that you want your custom skin to have.
4. To use your custom skin, follow steps 2 - 3 in the "Installing Skins" section.

Note that, You do not need to have all the images that Default has in your folder, just the ones you edit will suffice.

### Additional Features:

### Cinematics
- Allows replacing some video cutscenes in the game with custom ones 
To use, add the appropriate file in `Mods/Custom Knight/Skins/<skinname>/Cinematics`  or if you want to replace them globally then `Mods/Custom Knight/Cinematics`

Note: the existing Cinematics cannot be dumped but you may acquire the original videos [here](https://github.com/PrashantMohta/HollowKnight.CustomKnight/releases/)

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

#### Swapper
- Adds ability to skin bosses and any other objects that use a tk2dsprite 
- Adds ability to skin any other non animated objects that use a sprite
- Adds ability to "skin" text in the game 
- Ability to dump the sprites / text to make it easier to figure out what to edit

Using Swapper as a skin author : 

 - To replace a sprite : `Mods/Custom Knight/Skins/<skin>/Swap/<scene-name>/<gameobjectname>.png`
(will only update once encountered in this scene, refer to the dumped png)

 - To replace a sprite globally look for the sprite in the `Global` directory it will allow you to replace enemies / npcs / objects globally if the file exists in this folder


 - To replace a text using it's in-game key :  `Mods/Custom Knight/Skins/<skin>/Swap/<scene-name>/<sheet><key>.txt`
(scene-name here is just for organisation, the text will update globally)

 - To replace a text without it's key (case insensitive find & replace) : add the text in `Mods/CustomKnight/Skins/<skin>/Swap/replace.txt`

For example :
```
elDer=>Cool Dude
```
will replace "elder","ELDER","ElDeR" with "Cool Dude"

- To Dump assets (slow):  Enable Swapper Dump in settings : enter the room you want to dump objects /text from and wait till that object or text is on screen and change scene.
( Dump is triggered by new object creation & scene change) 

Use Swapper to swap things across skins : 
 - Create a Swap folder inside CustomKnight directory : `mods/CustomKnight/Swap/`
 - Treat this as a global "skin" that applies regardless of the skin being selected.

#### CustomKnight API

- Adds ability to add more items that can be skinned (even custom items)
- Adds ability to extend or modify existing skins at runtime (Including Swapper) 
- Adds ability to provide a skin directly at runtime ( allows taking control of the skin without breaking compatibility)
- Adds ability to set the current skin

See example mods under `AddonExample` directory in this repo for more, xml docs are added as they felt needed.


