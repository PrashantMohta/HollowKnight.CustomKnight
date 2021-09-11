using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Modding;
using System.Linq;


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
            "ShadowDashBlobs"
        };
        public static Dictionary<string, CustomKnightTexture> Textures = new Dictionary<string, CustomKnightTexture>();

        public static string DATA_DIR = Satchel.AssemblyUtils.getCurrentDirectory();
        public static string SKINS_FOLDER = Path.Combine(DATA_DIR,"Skins");
        public static string SKIN_FOLDER;

        public static string[] skinsArr;
        public static string[] skinNamesArr;

        public static void getSkinNames(){
            if(skinsArr!= null || skinNamesArr!=null) {return;}

            var dirs = Directory.GetDirectories(SKINS_FOLDER);
            var maxLen = CustomKnight.GlobalSettings.NameLength;
            skinsArr = new string[dirs.Length];
            skinNamesArr = new string[dirs.Length];
            for (int i = 0 ; i< dirs.Length ; i++)
            {
                string directoryName = new DirectoryInfo(dirs[i]).Name;
                string buttonText = directoryName.Length <= maxLen ? directoryName : directoryName.Substring(0,maxLen - 3) + "...";
                skinsArr[i] = directoryName;
                skinNamesArr[i] = buttonText; 
            }
        }

        public static void checkDirectoryStructure(){
            if (!Directory.Exists(DATA_DIR))
            {
                Directory.CreateDirectory(DATA_DIR);
            }
            if (!Directory.Exists(SKINS_FOLDER))
            {
                Directory.CreateDirectory(SKINS_FOLDER);
            }

            if (Directory.GetDirectories(SKINS_FOLDER).Length == 0)
            {
                CustomKnight.Instance.Log("There are no Custom Knight skin folders in the Custom Knight Skins directory.");
                return;
            }
        }
        public static bool dirHasPng(string sourceDirectory, SearchOption op){
           var assets = Directory.EnumerateFiles(sourceDirectory, "*.png", op);
           return assets.Any();
        }

        public static void CopyAllFiles(string currentPath,string root){
            string[] files = Directory.GetFiles(currentPath);
            foreach(string file in files){
                try{
                    File.Copy(file, Path.Combine(root,Path.GetFileName(file)));
                } catch (Exception e){
                    CustomKnight.Instance.Log("A File could not be Copied : " + e.ToString());
                }
            }
        }

        public static void getPngsToRoot(string currentPath, string root){
            try {
                List<string> queue = new List<string>();
                string[] dirs = Directory.GetDirectories(currentPath);
                foreach (string dir in dirs){
                    CustomKnight.Instance.Log("Looking in" + dir);
                    if(dirHasPng(dir,SearchOption.TopDirectoryOnly)){
                        CopyAllFiles(dir,root);
                    } else if(dirHasPng(dir,SearchOption.AllDirectories)){
                        queue.Add(dir);
                    }
                }
                foreach(string dir in queue){
                    getPngsToRoot(dir,root);
                }
            } catch (Exception e) {
                CustomKnight.Instance.Log("The Skin could not be fixed : " + e.ToString());
            }
        }
        public static void fixSkinStructures(){
            try{
                string[] skinDirectories = Directory.GetDirectories(SKINS_FOLDER);
                foreach (string dir in skinDirectories)
                {
                    if(!dirHasPng(dir,SearchOption.TopDirectoryOnly) && dirHasPng(dir,SearchOption.AllDirectories)){
                        CustomKnight.Instance.Log("A broken skin found! " + dir);
                        getPngsToRoot(dir,dir);
                    }
                }
            } catch (Exception e) {
                CustomKnight.Instance.Log("Failed to fix : "+ e.ToString());
            }

        }

        public static void init(){      
            
            foreach (string texName in _texNames)
            {
                CustomKnightTexture texture = new CustomKnightTexture(texName + ".png", false, null, null);
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