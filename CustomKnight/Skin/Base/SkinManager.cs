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
        internal static string DATA_DIR;
        internal static string SKINS_FOLDER;
        internal static ISelectableSkin CurrentSkin,DefaultSkin;
        internal static List<ISelectableSkin> ProvidedSkins = new List<ISelectableSkin>();
        internal static List<ISelectableSkin> SkinsList {get; private set;}

        public static Dictionary<string, Skinable> Skinables = new Dictionary<string, Skinable>{
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
        static SkinManager(){
            if(CustomKnight.isSatchelInstalled()){
                SetDataDir();
            }
        }    

        internal static void SetDataDir(){
            DATA_DIR = Satchel.AssemblyUtils.getCurrentDirectory();
            SKINS_FOLDER = Path.Combine(DATA_DIR,"Skins");
        }

        internal static string MaxLength(string skinName,int length){ 
            return skinName.Length <= length ? skinName : skinName.Substring(0,length - 3) + "...";
        }
        internal static void getSkinNames()
        {
            var dirs = Directory.GetDirectories(SKINS_FOLDER);
            SkinsList = new List<ISelectableSkin>();

            for (int i = 0 ; i< dirs.Length ; i++)
            {
                string directoryName = new DirectoryInfo(dirs[i]).Name;
                SkinsList.Add(new StaticSkin(directoryName));
            }
            for (int i = 0 ; i< ProvidedSkins.Count ; i++)
            {
                SkinsList.Add(ProvidedSkins[i]);
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
            if (CurrentSkin == null)
            {
                var CurrentSkinName = CustomKnight.SaveSettings.DefaultSkin != CustomKnight.GlobalSettings.DefaultSkin ? CustomKnight.SaveSettings.DefaultSkin : CustomKnight.GlobalSettings.DefaultSkin;
                CurrentSkin = GetSkinById(CurrentSkinName);
            }
            SpriteLoader.Load();
            On.GeoControl.Start -= ((Geo)Skinables[Geo.NAME]).GeoControl_Start;
            On.GeoControl.Start += ((Geo)Skinables[Geo.NAME]).GeoControl_Start;
            ModHooks.AfterSavegameLoadHook += SpriteLoader.ModifyHeroTextures;
        }

        internal static void Unload(){
            //load default skin for charms and such
            CurrentSkin = GetDefaultSkin();
            LoadSkin();
            On.GeoControl.Start -= ((Geo)Skinables[Geo.NAME]).GeoControl_Start;
            CustomKnight.dumpManager.Unload();
            CustomKnight.swapManager.Unload();
        }
        private static IEnumerator ChangeSkinRoutine(bool skipFlash)
        {
            if(!skipFlash){
                HeroController.instance.GetComponent<SpriteFlash>().flashFocusHeal();
            }
            LoadSkin();
            yield return new WaitUntil(() => SpriteLoader.LoadComplete);
        }
 
        /// <summary>
        ///     Add a skin to the skin list provided by an external mod.
        /// </summary>
        /// <param name="NewSkin">an <c>ISelectableSkin</c> that represents the skin</param>
        /// <returns>true if the skin is added</returns>
        public static bool AddSkin(ISelectableSkin NewSkin){
            var Exists = SkinManager.ProvidedSkins.Exists(skin => skin.GetId() == NewSkin.GetId());
            if(!Exists){
                SkinManager.ProvidedSkins.Add(NewSkin);
                BetterMenu.UpdateSkinList();
            }
            return !Exists;
        }
        
        /// <summary>
        ///     Gets a skin from the overall skin list that matches a given id.
        /// </summary>
        /// <param name="id">a <c>string</c> that uniquely identifies the skin</param>
        /// <returns>an <c>ISelectableSkin</c> that represents the skin or the default skin</returns>
        public static ISelectableSkin GetSkinById(string id){
            return SkinManager.SkinsList.Find( skin => skin.GetId() == id) ?? GetDefaultSkin();
        }
        
        /// <summary>
        ///     Gets the default skin.
        /// </summary>
        /// <returns>an <c>ISelectableSkin</c> that represents the default skin</returns>
        public static ISelectableSkin GetDefaultSkin(){
            if(DefaultSkin == null){
                DefaultSkin = GetSkinById("Default");
            }
            return DefaultSkin;
        }
        
        /// <summary>
        ///     Gets the current skin.
        /// </summary>
        /// <returns>an <c>ISelectableSkin</c> that represents the current skin</returns>
        public static ISelectableSkin GetCurrentSkin(){
            if(CurrentSkin == null){
                CurrentSkin = GetSkinById("Default");
            }
            return CurrentSkin;
        }
        
        /// <summary>
        ///     Gets all the installed skins (includes mod provided skins).
        /// </summary>
        /// <returns>an <c>ISelectableSkin[]</c> that represents all the installed skins</returns>
        public static ISelectableSkin[] GetInstalledSkins(){
            return SkinsList.ToArray();
        }

        /// <summary>
        ///     Refreshes the current skin, useful when the provided skin needs to change.
        /// </summary>
        /// <param name="skipFlash">a <c>bool</c> that determines if the knight should flash white</param>
        public static void RefreshSkin(bool skipFlash){
            if(HeroController.instance != null){
                GameManager.instance.StartCoroutine(ChangeSkinRoutine(skipFlash));
            }
        }
        public static event EventHandler<EventArgs> OnSetSkin;

        /// <summary>
        ///     Change the current skin, to the one whose id is provided.
        /// </summary>
        /// <param name="id">a <c>string</c> that uniquely identifies the skin</param>
        public static void SetSkinById(string id)
        {
            var Skin = GetSkinById(id);
            CustomKnight.Instance.Log("Trying to apply skin :" + Skin.GetId());
            if(CurrentSkin.GetId() == Skin.GetId()) { return; } 
            CurrentSkin = Skin;
            // use this when saving so you save to the right settings
            if(GameManager.instance.IsNonGameplayScene()){
                CustomKnight.GlobalSettings.DefaultSkin = Skin.GetId();
            } else {
                CustomKnight.GlobalSettings.DefaultSkin = Skin.GetId();
                CustomKnight.SaveSettings.DefaultSkin = Skin.GetId();
            };
            RefreshSkin(false);
            OnSetSkin?.Invoke(CustomKnight.Instance,new EventArgs());
        }      

   
    }

}
