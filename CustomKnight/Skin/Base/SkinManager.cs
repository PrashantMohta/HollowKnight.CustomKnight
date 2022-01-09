using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Modding;
using System.Linq;

using static Satchel.IoUtils;

namespace CustomKnight{
    public static class SkinManager{
        public static bool savedDefaultTextures = false;
        private static List<string> _texNames = new List<string>
        {
            "Icon",
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
            "DoubleJFeather",
            "SDCrystalBurst",
            "Leak",
            "Liquid",
            "HitPt",
            "ShadowDashBlobs",
            "Beam",
            "Compass"
        };
        
        public static Dictionary<string, CustomKnightTexture> Textures = new Dictionary<string, CustomKnightTexture>();
        public static Dictionary<string, Skinnable> Skinnables = new Dictionary<string, Skinnable>();

        public static string DATA_DIR;
        public static string SKINS_FOLDER;
        public static string SKIN_FOLDER;

        public static List<string> skinsArr;
        public static List<string> skinNamesArr;

        public static void SetDataDir(){
            DATA_DIR = Satchel.AssemblyUtils.getCurrentDirectory();
            SKINS_FOLDER = Path.Combine(DATA_DIR,"Skins");
        }
        static SkinManager(){
            if(Skinnables != null){
                Skinnables.Add("Compass",new Compass());
            }
            if(CustomKnight.isSatchelInstalled()){
                SetDataDir();
            }
            
        }

        public static void getSkinNames()
        {
            var dirs = Directory.GetDirectories(SKINS_FOLDER);
            var maxLen = CustomKnight.GlobalSettings.NameLength;

            if (skinsArr == null)
            {
                skinsArr = new List<string>();
                skinNamesArr = new List<string>();
            }

            for (int i = 0 ; i< dirs.Length ; i++)
            {
                string directoryName = new DirectoryInfo(dirs[i]).Name;
                string buttonText = directoryName.Length <= maxLen ? directoryName : directoryName.Substring(0,maxLen - 3) + "...";
                
                if (skinsArr.Contains(directoryName)) continue;
                
                skinsArr.Add(directoryName);
                skinNamesArr.Add(buttonText); 
            }
        }

        public static void checkDirectoryStructure(){
            EnsureDirectory(DATA_DIR);
            EnsureDirectory(SKINS_FOLDER);
            if (Directory.GetDirectories(SKINS_FOLDER).Length == 0)
            {
                CustomKnight.Instance.Log("There are no Custom Knight skin folders in the Custom Knight Skins directory.");
                return;
            }
        }
        public static void init(){      
            
            foreach (string texName in _texNames)
            {
                CustomKnightTexture texture;
                if(Skinnables.TryGetValue(texName,out var skinable)){
                    texture = skinable.ckTex;
                } else {
                    texture = new CustomKnightTexture(texName + ".png", false, null, null);
                }
                Textures.Add(texName, texture);
            }
        }

        private static void GeoControl_Start(On.GeoControl.orig_Start orig, GeoControl self)
        {
            SpriteLoader._geoMat = self.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            
            //save default texture because we dont have a copy
            if(SkinManager.Textures["Geo"].defaultTex == null){
                SkinManager.Textures["Geo"].defaultTex  = (Texture2D)SpriteLoader._geoMat.mainTexture;
            }
            var geoTexture = SkinManager.Textures["Geo"].missing ? SkinManager.Textures["Geo"].defaultTex : SkinManager.Textures["Geo"].tex;
            if (geoTexture != null  && SpriteLoader._geoMat != null)
            {
               SpriteLoader._geoMat.mainTexture = geoTexture;
            }
            On.GeoControl.Start -= GeoControl_Start;
            orig(self);
        }

        public static void LoadSkin(){
            if (SKIN_FOLDER == null)
            {
                SKIN_FOLDER = CustomKnight.SaveSettings.DefaultSkin != CustomKnight.GlobalSettings.DefaultSkin ? CustomKnight.SaveSettings.DefaultSkin : CustomKnight.GlobalSettings.DefaultSkin;
            }
            SpriteLoader.Load();
            On.GeoControl.Start -= GeoControl_Start;
            On.GeoControl.Start += GeoControl_Start;
            ModHooks.AfterSavegameLoadHook += SpriteLoader.ModifyHeroTextures;
        }

        public static void Unload(){
            //load default skin for charms and such
            SKIN_FOLDER = "Default";
            LoadSkin();
            On.GeoControl.Start -= GeoControl_Start;
            CustomKnight.dumpManager.Unload();
            CustomKnight.swapManager.Unload();
        }

        public static void ChangeSkin(string skinName)
        {
            CustomKnight.Instance.Log("trying to apply skin " + skinName);
            if(SKIN_FOLDER == skinName) { return; } 
            SKIN_FOLDER = skinName;
            GameManager.instance.StartCoroutine(ChangeSkinRoutine());
        }

        private static IEnumerator ChangeSkinRoutine()
        {
            HeroController.instance.GetComponent<SpriteFlash>().flashFocusHeal();
            LoadSkin();
            yield return new WaitUntil(() => SpriteLoader.LoadComplete);
        }
    }

}
