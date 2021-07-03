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

namespace CustomKnight
{
    public class CustomKnight : Mod,  IGlobalSettings<GlobalModSettings>, ILocalSettings<SaveModSettings>,ICustomMenuMod , ITogglableMod
    {
        public static GlobalModSettings GlobalSettings { get; set; } = new GlobalModSettings();
        public static SaveModSettings SaveSettings { get; set; } = new SaveModSettings();
        public static CustomKnight Instance { get; private set; }
        
        public static readonly Dictionary<string, GameObject> GameObjects = new Dictionary<string, GameObject>();
        new public string GetName() => "Custom Knight";
        
        public override string GetVersion() => "1.5.0-candidate";
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
            
            Instance = this;
            SkinManager.checkDirectory();

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

            Swapster.setSwapsterEnabled(CustomKnight.GlobalSettings.swapsterEnabled);
            ModMenu.setModMenu(SkinManager.SKIN_FOLDER,CustomKnight.GlobalSettings.Preloads);
            
            if(GlobalSettings.showMovedText){
                GUIController.Instance.BuildMenus();
            }
            // SkinManager.LoadSkin();
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
