---
title: Swapper
parent: Authors
---

## Swapper

- Adds ability to skin bosses and any other objects that use a tk2dsprite 
- Adds ability to skin any other non-animated objects that use a sprite
- Adds ability to "skin" text in the game 
- Ability to dump the sprites / text to make it easier to figure out what to edit

if it can dump it, Custom knight can probably swap it.


#### Dumping Assets :
There are two styles of dumping CustomKnight 3.0 onwards, Names.Json has better support and smaller skin sizes but will take a long time to dump. The old Directory style of dump is generally faster to dump but has limitations on what GameObjects it can Swap.

> It is okay to mix both the dump styles, CK supports both of them concurrently when swapping.

#### To Dump assets :

- Enter the room you want to dump objects/text from and wait till that object or text is on screen.
- Under Custom Knight Menu, select the Tools option
- Select the desired dump style
- Hit the dump button
- Custom Knight will show the dumping progress on screen

> Note : Custom knight dump progress can sometimes decrease as more objects are created by the game while running, you must either pause the game or monitor the folder if the sprites you are interested in have been dumped.

#### Using the dumped assets :

> TODO add notes about understanding what was dumped and how to use these files
> particularly cover the importance of Directory sturcture / names.json depending on dumpstyle

#### How to add your Swaps to a skin

> Note : Sprites are only replace when the GameObject you are replacing has been encountered in the game.

 - To replace a sprite : `Mods/Custom Knight/Skins/<skin>/Swap/<scene-name>/<gameobjectname>.png`

 - To replace a sprite globally look for the sprite in the `/Skins/<skin>Global` directory it will allow you to replace enemies / npcs / objects globally if the file exists in this folder

 - To replace a text using it's in-game key :  `/Skins/<skin>/Swap/<scene-name>/<sheet><key>.txt`

 - To replace text without key (case insensitive find & replace) : add the text in `/Skins/<skin>/Swap/replace.txt`

###### For example :

```
elDer=>Cool Dude
```
will replace "elder","ELDER","ElDeR" with "Cool Dude"

#### Use Swapper to swap things independent of the selected skin : 
 - Create a Swap folder inside CustomKnight directory : `mods/CustomKnight/Swap/`
 - Treat this as a global "skin" that applies regardless of the skin being selected.
