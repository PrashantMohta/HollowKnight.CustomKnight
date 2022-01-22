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
        internal static bool savedDefaultTextures = false;
        internal static Dictionary<string, Skinable> Skinables = new Dictionary<string, Skinable>{
            {Knight.NAME,new Knight()},
            {Sprint.NAME,new Sprint()},
            {Unn.NAME,new Unn()},
            {Shade.NAME,new Shade()},
            {ShadeOrb.NAME,new ShadeOrb()},

            {Wraiths.NAME,new Wraiths()},
            {VoidSpells.NAME,new VoidSpells()},
            {VS.NAME,new VS()},

            {Geo.NAME,new Geo()},
            {Hud.NAME,new Hud()},
            {OrbFull.NAME,new OrbFull()},
            {Liquid.NAME,new Liquid()},
            
            {QOrbs.NAME,new QOrbs()},
            {QOrbs2.NAME,new QOrbs2()},
            {ScrOrbs.NAME,new ScrOrbs()},
            {ScrOrbs2.NAME,new ScrOrbs2()},
            {DungRecharge.NAME, new DungRecharge()},
            {SDCrystalBurst.NAME,new SDCrystalBurst()},
            {DoubleJFeather.NAME,new DoubleJFeather()},
            {Leak.NAME,new Leak()},
            {HitPt.NAME,new HitPt()},
            {ShadowDashBlobs.NAME,new ShadowDashBlobs()},

            {Baldur.NAME,new Baldur()},
            {Fluke.NAME,new Fluke()},
            {Grimm.NAME,new Grimm()},
            {Shield.NAME,new Shield()},
            {Weaver.NAME,new Weaver()},
            {Hatchling.NAME,new Hatchling()},
            {Compass.NAME,new Compass()},
            {Grubberfly.NAME,new Grubberfly()},

            {"Cloak",new Preload("Cloak",() => CustomKnight.GameObjects["Cloak"])},
            {"Shriek",new Preload("Shriek",() => CustomKnight.GameObjects["Shriek"])},
            {"Wings",new Preload("Wings",() => CustomKnight.GameObjects["Wings"])},
            {"Quirrel",new Preload("Quirrel",() => CustomKnight.GameObjects["Quirrel"])},
            {"Webbed",new Preload("Webbed",() => CustomKnight.GameObjects["Webbed"])},
            {"DreamArrival",new Preload("DreamArrival",() => CustomKnight.GameObjects["DreamArrival"])},
            {"Dreamnail",new Preload("Dreamnail",() => CustomKnight.GameObjects["Dreamnail"])},
            {"Hornet",new Preload("Hornet",() => CustomKnight.GameObjects["Hornet"])},
            {"Birthplace",new Preload("Birthplace",() => CustomKnight.GameObjects["Birthplace"])},

            {"Charm_0",new Charm("Charm_0",0)},
            {"Charm_1",new Charm("Charm_1",1)},
            {"Charm_2",new Charm("Charm_2",2)},
            {"Charm_3",new Charm("Charm_3",3)},
            {"Charm_4",new Charm("Charm_4",4)},
            {"Charm_5",new Charm("Charm_5",5)},
            {"Charm_6",new Charm("Charm_6",6)},
            {"Charm_7",new Charm("Charm_7",7)},
            {"Charm_8",new Charm("Charm_8",8)},
            {"Charm_9",new Charm("Charm_9",9)},
            {"Charm_10",new Charm("Charm_10",10)},
            {"Charm_11",new Charm("Charm_11",11)},
            {"Charm_12",new Charm("Charm_12",12)},
            {"Charm_13",new Charm("Charm_13",13)},
            {"Charm_14",new Charm("Charm_14",14)},
            {"Charm_15",new Charm("Charm_15",15)},
            {"Charm_16",new Charm("Charm_16",16)},
            {"Charm_17",new Charm("Charm_17",17)},
            {"Charm_18",new Charm("Charm_18",18)},
            {"Charm_19",new Charm("Charm_19",19)},
            {"Charm_20",new Charm("Charm_20",20)},
            {"Charm_21",new Charm("Charm_21",21)},
            {"Charm_22",new Charm("Charm_22",22)},
            {"Charm_23_Broken",new Charm("Charm_23_Broken",23)},
            {"Charm_23_Fragile",new Charm("Charm_23_Fragile",23)},
            {"Charm_23_Unbreakable",new Charm("Charm_23_Unbreakable",23)},
            {"Charm_24_Broken",new Charm("Charm_24_Broken",24)},
            {"Charm_24_Fragile",new Charm("Charm_24_Fragile",24)},
            {"Charm_24_Unbreakable",new Charm("Charm_24_Unbreakable",24)},
            {"Charm_25_Broken",new Charm("Charm_25_Broken",25)},
            {"Charm_25_Fragile",new Charm("Charm_25_Fragile",25)},
            {"Charm_25_Unbreakable",new Charm("Charm_25_Unbreakable",25)},
            {"Charm_26",new Charm("Charm_26",26)},
            {"Charm_27",new Charm("Charm_27",27)},
            {"Charm_28",new Charm("Charm_28",28)},
            {"Charm_29",new Charm("Charm_29",29)},
            {"Charm_30",new Charm("Charm_30",30)},
            {"Charm_31",new Charm("Charm_31",31)},
            {"Charm_32",new Charm("Charm_32",32)},
            {"Charm_33",new Charm("Charm_33",33)},
            {"Charm_34",new Charm("Charm_34",34)},
            {"Charm_35",new Charm("Charm_35",35)},
            {"Charm_36_Black",new Charm("Charm_36_Black",36)},
            {"Charm_36_Full",new Charm("Charm_36_Full",36)},
            {"Charm_36_Left",new Charm("Charm_36_Left",36)},
            {"Charm_36_Right",new Charm("Charm_36_Right",36)},
            {"Charm_37",new Charm("Charm_37",37)},
            {"Charm_38",new Charm("Charm_38",38)},
            {"Charm_39",new Charm("Charm_39",39)},
            {"Charm_40_1",new Charm("Charm_40_1",40)},
            {"Charm_40_2",new Charm("Charm_40_2",40)},
            {"Charm_40_3",new Charm("Charm_40_3",40)},
            {"Charm_40_4",new Charm("Charm_40_4",40)},
            {"Charm_40_5",new Charm("Charm_40_5",40)}
           // {"PinsScarab", new Pins()}
        };
        internal static string DATA_DIR;
        internal static string SKINS_FOLDER;
        internal static string SKIN_FOLDER;
        internal static List<string> skinsArr;
        internal static List<string> skinNamesArr;

        internal static void SetDataDir(){
            DATA_DIR = Satchel.AssemblyUtils.getCurrentDirectory();
            SKINS_FOLDER = Path.Combine(DATA_DIR,"Skins");
        }
        static SkinManager(){
            if(CustomKnight.isSatchelInstalled()){
                SetDataDir();
            }
        }

        internal static void getSkinNames()
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

        internal static void checkDirectoryStructure(){
            EnsureDirectory(DATA_DIR);
            EnsureDirectory(SKINS_FOLDER);
            if (Directory.GetDirectories(SKINS_FOLDER).Length == 0)
            {
                CustomKnight.Instance.Log("There are no Custom Knight skin folders in the Custom Knight Skins directory.");
                return;
            }
        }
        
        internal static void LoadSkin(){
            if (SKIN_FOLDER == null)
            {
                SKIN_FOLDER = CustomKnight.SaveSettings.DefaultSkin != CustomKnight.GlobalSettings.DefaultSkin ? CustomKnight.SaveSettings.DefaultSkin : CustomKnight.GlobalSettings.DefaultSkin;
            }
            SpriteLoader.Load();
            On.GeoControl.Start -= ((Geo)Skinables[Geo.NAME]).GeoControl_Start;
            On.GeoControl.Start += ((Geo)Skinables[Geo.NAME]).GeoControl_Start;
            ModHooks.AfterSavegameLoadHook += SpriteLoader.ModifyHeroTextures;
        }

        internal static void Unload(){
            //load default skin for charms and such
            SKIN_FOLDER = "Default";
            LoadSkin();
            On.GeoControl.Start -= ((Geo)Skinables[Geo.NAME]).GeoControl_Start;
            CustomKnight.dumpManager.Unload();
            CustomKnight.swapManager.Unload();
        }

        internal static void ChangeSkin(string skinName)
        {
            CustomKnight.Instance.Log("trying to apply skin " + skinName);
            if(SKIN_FOLDER == skinName) { return; } 
            SKIN_FOLDER = skinName;
            if(HeroController.instance != null){
                GameManager.instance.StartCoroutine(ChangeSkinRoutine());
            }
        }

        private static IEnumerator ChangeSkinRoutine()
        {
            HeroController.instance.GetComponent<SpriteFlash>().flashFocusHeal();
            LoadSkin();
            yield return new WaitUntil(() => SpriteLoader.LoadComplete);
        }
    }

}
