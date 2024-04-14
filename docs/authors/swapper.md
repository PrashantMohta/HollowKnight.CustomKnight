---
title: Swapper
parent: Authors
---
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
