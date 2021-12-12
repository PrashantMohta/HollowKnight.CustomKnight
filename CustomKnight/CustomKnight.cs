using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HutongGames.PlayMaker.Actions;
using Modding;

using On.TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Object = System.Object;
using Random = UnityEngine.Random;

using Modding.Menu;
using Modding.Menu.Config;
using Patch = Modding.Patches;
using CustomKnight.Canvas;
using static Satchel.AssemblyUtils;

namespace CustomKnight
{
    public class CustomKnight : Mod,  IGlobalSettings<GlobalModSettings>, ILocalSettings<SaveModSettings>,ICustomMenuMod , ITogglableMod
    {
        public static GlobalModSettings GlobalSettings { get; set; } = new GlobalModSettings();
        public static SaveModSettings SaveSettings { get; set; } = new SaveModSettings();
        public static CustomKnight Instance { get; private set; }
        public static DumpManager dumpManager {get; private set;} = new DumpManager();
        public static SwapManager swapManager {get; private set;} = new SwapManager();

        public static readonly Dictionary<string, GameObject> GameObjects = new Dictionary<string, GameObject>();
        internal static void touchSatchelVersion(){
            Satchel.AssemblyUtils.Version();
        }
        internal static bool isSatchelInstalled(){
            var isInstalled = false;
            try{
                touchSatchelVersion();
                isInstalled = true;
            } catch (Exception e){
                Modding.Logger.Log(e);
            }
            return isInstalled;
        }
        internal void getVersionSafely(){
            version = Satchel.AssemblyUtils.GetAssemblyVersionHash();
        }
        public string version;
        new public string GetName() => "Custom Knight";
        public override string GetVersion(){
            version = "Satchel not found";
            if(isSatchelInstalled()){
                getVersionSafely();
            }
            return version;
        }
        public void OnLoadGlobal(GlobalModSettings s)
        {
            if(s.Version == GetVersion()){
                CustomKnight.GlobalSettings = s;
            } else {
                CustomKnight.GlobalSettings = s;
                CustomKnight.GlobalSettings.Version = GetVersion();
                CustomKnight.GlobalSettings.NameLength = new GlobalModSettings().NameLength;
            }
            SkinManager.SKIN_FOLDER = CustomKnight.GlobalSettings.DefaultSkin;
        }

        public GlobalModSettings OnSaveGlobal()
        {
            return CustomKnight.GlobalSettings;
        }

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

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            
            if (Instance == null) 
            { 
                Instance = this;
            }
            if(!isSatchelInstalled()){
                return;
            }
            SkinManager.checkDirectoryStructure();

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

                SkinManager.init();
                SkinManager.getSkinNames();

            }
            if(CustomKnight.GlobalSettings.SwapperEnabled){
                swapManager.enabled = true;
                swapManager.active = true;
            }

            ModMenu.setModMenu(SkinManager.SKIN_FOLDER,CustomKnight.GlobalSettings.Preloads);
            
            if(GlobalSettings.showMovedText){
                GUIController.Instance.BuildMenus();
            }
            ModHooks.AfterSavegameLoadHook += LoadSaveGame;
        }

        public  bool ToggleButtonInsideMenu {get;}= true;
        public MenuScreen GetMenuScreen(MenuScreen modListMenu,ModToggleDelegates? toggle){
            MenuScreen m = ModMenu.createMenuScreen(modListMenu,toggle);
            ModMenu.RefreshOptions();
            return m;
        }

        public void LoadSaveGame(SaveGameData data){
            Log("LoadSaveGame");
            SkinManager.SKIN_FOLDER = SaveSettings.DefaultSkin != GlobalSettings.DefaultSkin ? SaveSettings.DefaultSkin : GlobalSettings.DefaultSkin;
            ModMenu.setModMenu(SkinManager.SKIN_FOLDER,CustomKnight.GlobalSettings.Preloads);
            SkinManager.LoadSkin();
        }
        public void OnLoadLocal(SaveModSettings s)
        {
            CustomKnight.SaveSettings = s;
        }

        public static void toggleSwap(bool enable){
            swapManager.enabled = enable;
            if(!enable){
                swapManager.Unload();
                dumpManager.enabled = false;
            } else {
                swapManager.Load();
            }
        }

        public static void toggleDump(bool enable){
            dumpManager.enabled = enable;
        }

        public void Unload(){
            SkinManager.Unload();
            ModHooks.AfterSavegameLoadHook -= LoadSaveGame;
        }
        public SaveModSettings OnSaveLocal()
        {
            return CustomKnight.SaveSettings;
        }
        
    }
}
