using CustomKnight.NewUI;
using CustomKnight.Skin.Swapper;

namespace CustomKnight
{
    /// <summary>
    /// Main Mod Class
    /// </summary>
    public class CustomKnight : Mod, IGlobalSettings<GlobalModSettings>, ILocalSettings<SaveModSettings>, ICustomMenuMod, ITogglableMod
    {
        /// <summary>
        /// GlobalSettings
        /// </summary>
        public static GlobalModSettings GlobalSettings { get; set; } = new GlobalModSettings();
        /// <summary>
        /// Settings for current save
        /// </summary>
        public static SaveModSettings SaveSettings { get; set; } = new SaveModSettings();
        /// <summary>
        /// Current Mod instance
        /// </summary>
        public static CustomKnight Instance { get; private set; }
        /// <summary>
        /// Current DumpManager instance
        /// </summary>
        public static DumpManager dumpManager { get; private set; } = new DumpManager();
        /// <summary>
        /// Current SwapManager instance
        /// </summary>
        public static SwapManager swapManager { get; private set; } = new SwapManager();
        /// <summary>
        /// Stores Preloaded gameobjects
        /// </summary>
        public static readonly Dictionary<string, GameObject> GameObjects = new();
        /// <summary>
        /// Event called when CK is fully ready (after applying a skin)
        /// </summary>
        public static event EventHandler<EventArgs> OnReady;
        /// <summary>
        /// Event called when CK is initialised but before applying skin
        /// </summary>
        public static event EventHandler<EventArgs> OnInit;
        /// <summary>
        /// Event called when the mod is unloaded
        /// </summary>
        public static event EventHandler<EventArgs> OnUnload;
        /// <summary>
        /// ToggleButtonInsideMenu
        /// </summary>
        public bool ToggleButtonInsideMenu { get; } = true;
        internal static void touchSatchelVersion()
        {
            Satchel.AssemblyUtils.Version();
        }
        internal static bool isSatchelInstalled()
        {
            var isInstalled = false;
            try
            {
                touchSatchelVersion();
                isInstalled = true;
            }
            catch (Exception e)
            {
                Modding.Logger.Log(e);
            }
            return isInstalled;
        }
        internal void getVersionSafely()
        {
            version = Satchel.AssemblyUtils.GetAssemblyVersionHash();
        }

        internal string version;
        /// <summary>
        /// GetName
        /// </summary>
        /// <returns></returns>
        public new string GetName() => "Custom Knight";
        /// <summary>
        /// GetVersion
        /// </summary>
        /// <returns></returns>
        public override string GetVersion()
        {
            version = "Satchel not found";
            if (isSatchelInstalled())
            {
                getVersionSafely();
            }
            return version;
        }
        /// <summary>
        /// OnLoadGlobal
        /// </summary>
        /// <param name="s"></param>
        public void OnLoadGlobal(GlobalModSettings s)
        {
            if (s.Version == GetVersion())
            {
                CustomKnight.GlobalSettings = s;
            }
            else
            {
                var DefaultSettings = new GlobalModSettings();
                CustomKnight.GlobalSettings = s;
                CustomKnight.GlobalSettings.Version = GetVersion();
                CustomKnight.GlobalSettings.NameLength = DefaultSettings.NameLength;
            }
        }
        /// <summary>
        /// OnSaveGlobal
        /// </summary>
        /// <returns></returns>
        public GlobalModSettings OnSaveGlobal()
        {
            return CustomKnight.GlobalSettings;
        }
        /// <summary>
        /// OnLoadLocal
        /// </summary>
        /// <param name="s"></param>
        public void OnLoadLocal(SaveModSettings s)
        {
            CustomKnight.SaveSettings = s;
        }
        /// <summary>
        /// OnSaveLocal
        /// </summary>
        /// <returns></returns>
        public SaveModSettings OnSaveLocal()
        {
            return CustomKnight.SaveSettings;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public CustomKnight()
        {
            // needs an early hook
            PreloadedTk2dSpritesHandler.Hook();
            SpriteFlashManager.Hook();
            CinematicsManager.Hook();
        }

        /// <summary>
        /// GetPreloadNames
        /// </summary>
        /// <returns></returns>
        public override List<(string, string)> GetPreloadNames()
        {
            List<(string, string)> preloadsList;
            if (GlobalSettings.Preloads || GlobalSettings.GenerateDefaultSkin)
            {
                preloadsList = new List<(string, string)>
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
            else
            {
                preloadsList = new List<(string, string)>();
            }
            if (GlobalSettings.GenerateDefaultSkin)
            {
                preloadsList.AddRange(new List<(string, string)>
                {
                    ("GG_Vengefly","Giant Buzzer Col")
                });
            }
            return preloadsList;
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="preloadedObjects"></param>
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            Log($"Initializing CustomKnight {version}");
            //Making sure skinables list exists
            Log($"Found {SkinManager.Skinables.Count} Skinables");
            PreloadedTk2dSpritesHandler.Unhook();
            PreloadedTk2dSpritesHandler.Enable();
            SaveHud.Hook();
            OnInit?.Invoke(this, null);

            if (!isSatchelInstalled())
            {
                return;
            }
            SkinManager.checkDirectoryStructure();
            SkinManager.getSkinNames();
            SaveHud.LoadAll();
            SkinManager.CurrentSkin = SkinManager.GetSkinById(CustomKnight.GlobalSettings.DefaultSkin);
            SkinManager.SetSkinById(CustomKnight.GlobalSettings.DefaultSkin);
            // Initial load
            if (preloadedObjects != null)
            {
                if (preloadedObjects["GG_Vengefly"].TryGetValue("Giant Buzzer Col", out var enemy))
                {
                    var hm = enemy.GetComponent<HealthManager>();
                    var prefab = ReflectionHelper.GetField<HealthManager, GameObject>(hm, "largeGeoPrefab");
                    (SkinManager.Skinables[Geo.NAME] as Geo).SetGeoDefaultTexture(prefab);
                }
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

            if (CustomKnight.GlobalSettings.SwapperEnabled)
            {
                swapManager.enabled = true;
                swapManager.active = true;
            }


            InputListener.Start();
            if (CustomKnight.GlobalSettings.EnablePauseMenu)
            {
                UIController.EnableMenu();
            }

            if (CustomKnight.GlobalSettings.GenerateDefaultSkin)
            {
                DefaultSkin.SaveSkin();
            }
            else
            {
                DefaultSkin.isGeneratingDefaultSkin = false;
            }
            On.HeroController.Start += HeroControllerStart;
        }

        /// <summary>
        /// GetMenuScreen
        /// </summary>
        /// <param name="modListMenu"></param>
        /// <param name="toggle"></param>
        /// <returns></returns>
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggle)
        {
            return BetterMenu.GetMenu(modListMenu, toggle);
        }

        internal void HeroControllerStart(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);
            Log("HeroControllerStart");
            var currentSkinId = (SaveSettings.DefaultSkin != GlobalSettings.DefaultSkin && SaveSettings.DefaultSkin != null) ? SaveSettings.DefaultSkin : GlobalSettings.DefaultSkin;
            SkinManager.CurrentSkin = SkinManager.GetSkinById(currentSkinId);
            SkinManager.LoadSkin();
            OnReady?.Invoke(this, null);
        }


        internal static void toggleSwap(bool enable)
        {
            swapManager.enabled = enable;
            if (!enable)
            {
                swapManager.Unhook();
                dumpManager.Unhook();
                PreloadedTk2dSpritesHandler.Disable();
            }
            else
            {
                swapManager.Hook();
                dumpManager.Hook();
                PreloadedTk2dSpritesHandler.Enable();
            }
        }

        /// <summary>
        /// Unload
        /// </summary>
        public void Unload()
        {
            SaveHud.UnHook();
            SkinManager.Unload();
            OnUnload?.Invoke(this, null);
            On.HeroController.Start -= HeroControllerStart;
        }

        internal static void SaveSprite(Sprite s, string str)
        {
            var tex = SpriteUtils.ExtractTextureFromSprite(s);
            CustomKnight.dumpManager.SaveTextureByPath("Debug", str, tex);
        }
    }
}
