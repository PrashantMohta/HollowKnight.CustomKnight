using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CustomKnight.Canvas;
using HutongGames.PlayMaker.Actions;
using Modding;
using On.TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace CustomKnight
{
    public class CustomKnight : Mod, ITogglableMod , IGlobalSettings<GlobalModSettings>, ILocalSettings<SaveModSettings>
    {
        public static GlobalModSettings GlobalSettings { get; set; } = new GlobalModSettings();
        public static SaveModSettings SaveSettings { get; set; } = new SaveModSettings();

        public static bool Preloads;

        public static string DATA_DIR;
        public const string SKINS_FOLDER = "CustomKnight";
        public static string SKIN_FOLDER;

        public static bool savedDefaultTextures = false;
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
            "Charm_0",
            "Charm_1",
            "Charm_2",
            "Charm_3",
            "Charm_4",
            "Charm_5",
            "Charm_6",
            "Charm_7",
            "Charm_8",
            "Charm_9",
            "Charm_10",
            "Charm_11",
            "Charm_12",
            "Charm_13",
            "Charm_14",
            "Charm_15",
            "Charm_16",
            "Charm_17",
            "Charm_18",
            "Charm_19",
            "Charm_20",
            "Charm_21",
            "Charm_22",
            "Charm_23_Broken",
            "Charm_23_Fragile",
            "Charm_23_Unbreakable",
            "Charm_24_Broken",
            "Charm_24_Fragile",
            "Charm_24_Unbreakable",
            "Charm_25_Broken",
            "Charm_25_Fragile",
            "Charm_25_Unbreakable",
            "Charm_26",
            "Charm_27",
            "Charm_28",
            "Charm_29",
            "Charm_30",
            "Charm_31",
            "Charm_32",
            "Charm_33",
            "Charm_34",
            "Charm_35",
            "Charm_36_Black",
            "Charm_36_Full",
            "Charm_36_Left",
            "Charm_36_Right",
            "Charm_37",
            "Charm_38",
            "Charm_39",
            "Charm_40_1",
            "Charm_40_2",
            "Charm_40_3",
            "Charm_40_4",
            "Charm_40_5",
            "Charms",
        };
        public static Dictionary<string, CustomKnightTexture> Textures = new Dictionary<string, CustomKnightTexture>();

        public static CustomKnight Instance { get; private set; }
        
        public static readonly Dictionary<string, GameObject> GameObjects = new Dictionary<string, GameObject>();


        public override List<(string, string)> GetPreloadNames()
        {
            if (Preloads)
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


        public void LoadSkin(){
            if (SKIN_FOLDER == null)
            {
                Log("Skin folder null: setting to default");
                SKIN_FOLDER = SaveSettings.DefaultSkin != GlobalSettings.DefaultSkin ? SaveSettings.DefaultSkin : GlobalSettings.DefaultSkin;
            }

            SpriteLoader.Load();

            On.GeoControl.Start -= GeoControl_Start;
            On.GeoControl.Start += GeoControl_Start;
            ModHooks.AfterSavegameLoadHook += SpriteLoader.ModifyHeroTextures;
        }
        
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
            
            // Initial load
            if (preloadedObjects != null)
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
                
                foreach (string texName in _texNames)
                {
                    CustomKnightTexture texture = new CustomKnightTexture(texName + ".png", false, null, null);
                    Textures.Add(texName, texture);
                }
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

            GUIController.Instance.BuildMenus();
            LoadSkin();
        }

        private void GeoControl_Start(On.GeoControl.orig_Start orig, GeoControl self)
        {
            SpriteLoader._geoMat = self.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            
            //save default texture because we dont have a copy
            if(Textures["Geo"].defaultTex == null){
                Textures["Geo"].defaultTex  = (Texture2D)SpriteLoader._geoMat.mainTexture;
            }
            var geoTexture = Textures["Geo"].missing ? Textures["Geo"].defaultTex : Textures["Geo"].tex;
            if (geoTexture != null  && SpriteLoader._geoMat != null)
            {
               SpriteLoader._geoMat.mainTexture = geoTexture;
            }
            On.GeoControl.Start -= GeoControl_Start;
            orig(self);
        }

        public override string GetVersion() => "1.5.0-candidate";
        
        public void Unload()
        {
            Log("Unloading");
            ModHooks.AfterSavegameLoadHook -= SpriteLoader.ModifyHeroTextures;
            On.GeoControl.Start -= GeoControl_Start;
            SpriteLoader.UnloadAll();
            UnityEngine.Object.Destroy(GameObject.Find("Custom Knight Canvas"));
        }


        public void OnLoadGlobal(GlobalModSettings s)
        {
            CustomKnight.GlobalSettings = s;
            CustomKnight.Preloads = CustomKnight.GlobalSettings.Preloads;
        }

        public GlobalModSettings OnSaveGlobal()
        {
            return CustomKnight.GlobalSettings;
        }

        public void OnLoadLocal(SaveModSettings s)
        {
            CustomKnight.SaveSettings = s;
            SKIN_FOLDER = SaveSettings.DefaultSkin != GlobalSettings.DefaultSkin ? SaveSettings.DefaultSkin : GlobalSettings.DefaultSkin;
            if (null != CustomKnight.Instance){
                CustomKnight.Instance.LoadSkin();
            } 
        }

        public SaveModSettings OnSaveLocal()
        {
            return CustomKnight.SaveSettings;
        }
    }
}
