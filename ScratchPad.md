CK 4.0 thought dump

- delete directory mode swaps
-- must migrate to names.json based swaps when loading such a skin
-- brute force will mean we will have to keep the skin size during auto migration
-- checking hash will be slow but result in smaller skin size, might also help with memory usage at runtime

- rename names.json to manifest.json
-- add a manifest version number in skin-config that allows for future extensions without renaming it
-- when dumping manifest.json, indicate if the game object was using a tk2d or unity sprite (this will allow authors to know when certain features are supported)
-- add some basic organisation and human readable aliases only
-- explore if we can use hashcache.json to generate names.json faster?
-- ability to swap tk2d uniquely 
-- add support for alts

something like :

```json

{
    "type" : "scene", // type scene / knight / ui / menu
    "group" : "Town", // scene name if type is scene, others may not need a group
    "items": [
        {   "id" : "<original asset hash>|<skinnable name>",
            "type" : "tk2d|sprite|particle|cinematic|customskinable", // these types will allow us to combine swap code with regular knight skinning code everything will have same base
            "alts" : [
                {
                    "name" : "defaultAltName", // mods can use altName to do fun things like select an alt for left/right or select knight alts for a particular scene.
                    "assets" : [ // assets contains multiple entries so we will apply each one uniquely when possible
                        { 
                            "file":"filename.png",
                            "paths":["paths-to-apply-1","paths-to-apply-2"]
                            //expose more options here for each of the types
                        },
                        { 
                            "file":"filename2.png",
                            "paths":["paths-to-apply-3"]
                        }
                    ]
                },
                {
                    "name" : "secondaryAltName",
                    "assets" : [
                        { 
                            "file":"filename.png",
                            "paths":["paths-to-apply-1",]
                        },
                        { 
                            "file":"filename2.png",
                            "paths":["paths-to-apply-2","paths-to-apply-3"]
                        }
                    ]
                },
                {
                    "name" : "tertiaryAltName",
                    "assets" : [ // assets has only one entry so we will apply without cloning tk2d
                        { 
                            "file":"filename.png",
                            "paths":["paths-to-apply-1","paths-to-apply-2","paths-to-apply-3"]
                        }
                    ]
                },
            ]
            
    ],
    }
}
```


- add support for alt presets
-- json files that use id->selected alt map per group since id is only unique per group
-- give in game menu to create an alt preset
---- create alt preset -> enter name -> assign alts -> save
---- edit alt preset -> choose alt preset -> assign alts -> save

something like:
```json

    {
        "name" : "PresetName",
        "baseAlt" : "defaultAltName",
        "overrides" : [
            { 
                "group" : "Town",
                "items" : [{"id":"itemId","altName":"SelectedAltName"}]
            }
        ]
    }
```

- add required mods field to skin-config
-- CK will refuse to apply a skin when these mods are not installed
-- offer a lumafly install link to these mods as alternative

- rewrite swap backend and skin loading to use same manifest files

- make skin names case insensitive
-- simplest solution sounds like it is to migrate the files to lowercase when trying to apply.

- make the pause menu have a show/hide button in addition to the keybind

