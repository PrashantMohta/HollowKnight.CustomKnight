using System;
using System.Collections.Generic;
using System.IO;
using CustomKnight.Canvas;
using HutongGames.PlayMaker.Actions;
using ModCommon;
using ModCommon.Util;
using Modding;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomKnight
{
    public class CustomKnight : Mod<SaveSettings, GlobalSettings>, ITogglableMod
    {
        public class CustomKnightTexture
        {
            public bool missing;
            public string fileName;
            public Sprite defaultCharmSprite;
            public Texture2D defaultTex;
            public Texture2D tex;

            public CustomKnightTexture()
            {
                fileName = null;
                missing = false;
                defaultTex = null;
                tex = null;
                defaultCharmSprite = null;
            }
            
            public CustomKnightTexture(string fileName, bool missing, Texture2D defaultTex, Texture2D tex, Sprite defaultCharmSprite)
            {
                this.fileName = fileName;
                this.missing = missing;
                this.defaultTex = defaultTex;
                this.tex = tex;
                this.defaultCharmSprite = defaultCharmSprite;
            }
        }

        public static Dictionary<string, CustomKnightTexture> Textures = new Dictionary<string, CustomKnightTexture>();

        internal static GlobalSettings settings;

        private static List<string> _texNames = new List<string>
        {
            "Knight",
            "Sprint",
            "Shade",
            "ShadeOrb",
            "Wraiths",
            "VoidSpells",
            "VS",
            "Hud",
            "OrbFull",
            "Geo",
            "Dreamnail",
            "DreamArrival",
            "Unn",
            "Wings",
            "Quirrel",
            "Webbed",
            "Cloak",
            "Shriek",
            "Hornet",
            "Birthplace",
            "Baldur",
            "Fluke",
            "Grimm",
            "Shield",
            "Weaver",
            "Hatchling",
            "Inventory",
        };
        
        public const string SKINS_FOLDER = "CustomKnight";
        public static string SKIN_FOLDER;

        public static string DATA_DIR;

        public static CustomKnight Instance { get; private set; }

        public override List<(string, string)> GetPreloadNames()
        {
            if (GlobalSettings.Preloads)
            {
                return new List<(string, string)>
                {
                    ("Abyss_10", "higher_being/Dish Plat/Knight Dummy"),
                    ("Abyss_12", "Scream 2 Get/Cutscene Knight"),
                    ("Abyss_21", "Shiny Item DJ/Knight Cutscene"),
                    ("Crossroads_50", "Quirrel Lakeside/Sit Region/Knight Sit"),
                    ("Deepnest_East_12", "Hornet Blizzard Return Scene"),
                    ("Deepnest_Spider_Town", "RestBench Spider/Webbed Knight"),
                    ("Dream_Abyss", "End Cutscene/Dummy"),
                    ("GG_Door_5_Finale", "abyss_door_5_cutscene_sequence/main_chars"),
                    ("GG_Vengefly", "Boss Scene Controller/Dream Entry/Knight Dream Arrival"),
                    ("RestingGrounds_07", "Dream Moth/Knight Dummy"),
                };   
            }
            
            return new List<(string, string)>();
        }
        
        
        public static readonly Dictionary<string, GameObject> GameObjects = new Dictionary<string, GameObject>();

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            switch (SystemInfo.operatingSystemFamily)
            {
                case OperatingSystemFamily.MacOSX:
                    DATA_DIR = Path.GetFullPath(Application.dataPath + "/Resources/Data/Managed/Mods/" + SKINS_FOLDER);
                    break;
                default:
                    DATA_DIR = Path.GetFullPath(Application.dataPath + "/Managed/Mods/" + SKINS_FOLDER);
                    break;
            }

            settings = GlobalSettings;

            // Initial load
            if (preloadedObjects != null && settings.Preloads)
            {
                GameObjects.Add("Cloak", preloadedObjects["Abyss_10"]["higher_being/Dish Plat/Knight Dummy"]);
                GameObjects.Add("Shriek", preloadedObjects["Abyss_12"]["Scream 2 Get/Cutscene Knight"]);
                GameObjects.Add("Wings", preloadedObjects["Abyss_21"]["Shiny Item DJ/Knight Cutscene"]);
                GameObjects.Add("Quirrel", preloadedObjects["Crossroads_50"]["Quirrel Lakeside/Sit Region/Knight Sit"]);
                GameObjects.Add("Hornet", preloadedObjects["Deepnest_East_12"]["Hornet Blizzard Return Scene"]);
                GameObjects.Add("Webbed", preloadedObjects["Deepnest_Spider_Town"]["RestBench Spider/Webbed Knight"]);
                GameObjects.Add("Birthplace", preloadedObjects["Dream_Abyss"]["End Cutscene/Dummy"]);
                GameObjects.Add("DreamArrival", preloadedObjects["GG_Vengefly"]["Boss Scene Controller/Dream Entry/Knight Dream Arrival"]);
                GameObjects.Add("Dreamnail", preloadedObjects["RestingGrounds_07"]["Dream Moth/Knight Dummy"]);
            }

            foreach (string texName in _texNames)
            {
                if (Textures.ContainsKey(texName))
                {
                    break;
                }
                
                CustomKnightTexture texture = new CustomKnightTexture(texName + ".png", false, null, null, null);
                Textures.Add(texName, texture);
            }

            Instance = this;

            if (!Directory.Exists(DATA_DIR))
            {
                Directory.CreateDirectory(DATA_DIR);
            }

            if (Directory.GetDirectories(DATA_DIR).Length == 0)
            {
                Log("There are no Custom Knight skin folders in the Custom Knight directory.");
                return;
            }

            if (SKIN_FOLDER == null)
            {
                SKIN_FOLDER = settings.DefaultSkin;
            }

            SaveGlobalSettings();

            ResetTextures();

            GUIController.Instance.BuildMenus();

            SpriteLoader.Load();

            On.GeoControl.Start -= GeoControl_Start;
            On.GeoControl.Start += GeoControl_Start;
            ModHooks.Instance.AfterSavegameLoadHook += SpriteLoader.ModifyHeroTextures;
        }

        private bool _gotGeoTex;
        private void GeoControl_Start(On.GeoControl.orig_Start orig, GeoControl geo)
        {
            if (SpriteLoader.ChangedSkin)
            {
                SpriteLoader.ChangedSkin = false;
                CustomKnightTexture geoTex = Textures["Geo"];
            
                if (!_gotGeoTex)
                {
                    _gotGeoTex = true;
                    geoTex.defaultTex = geo.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                }
                
                geo.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = geoTex.missing ? geoTex.defaultTex : geoTex.tex;
            }

            orig(geo);
        }

        public override string GetVersion() => "1.2.3";

        private void ResetTextures()
        {
            foreach (KeyValuePair<string, CustomKnightTexture> pair in Textures)
            {
                CustomKnightTexture texture = pair.Value;
                Object.Destroy(texture.tex);
                texture.tex = null;
                texture.missing = false;
                if (!File.Exists((DATA_DIR + "/" + SKIN_FOLDER + "/" + texture.fileName).Replace("\\", "/")))
                {
                    //Log($"Missing file {texture.fileName} from folder {SKIN_FOLDER}.");
                    texture.missing = true;
                }
            }
        }
        
        public void Unload()
        {
            Log("Unloading");
            ModHooks.Instance.AfterSavegameLoadHook -= SpriteLoader.ModifyHeroTextures;
            On.GeoControl.Start -= GeoControl_Start;
            SpriteLoader.UnloadAll();
            Object.Destroy(GameObject.Find("Custom Knight Canvas"));
        }
    }
}