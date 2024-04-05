using CustomKnight.Canvas;
using CustomKnight.NewUI;
using CustomKnight.Skin.Swapper;

namespace CustomKnight
{
    public class CustomKnight : Mod, IGlobalSettings<GlobalModSettings>, ILocalSettings<SaveModSettings>, ICustomMenuMod, ITogglableMod
    {
        public static GlobalModSettings GlobalSettings { get; set; } = new GlobalModSettings();
        public static SaveModSettings SaveSettings { get; set; } = new SaveModSettings();
        public static CustomKnight Instance { get; private set; }
        public static DumpManager dumpManager { get; private set; } = new DumpManager();
        public static SwapManager swapManager { get; private set; } = new SwapManager();
        public static SpriteFlashManager spriteFlashManager { get; private set; } = new SpriteFlashManager();
        public static CinematicsManager cinematicsManager { get; private set; } = new CinematicsManager();

        public static readonly Dictionary<string, GameObject> GameObjects = new Dictionary<string, GameObject>();
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
        public string version;
        new public string GetName() => "Custom Knight";
        public override string GetVersion()
        {
            version = "Satchel not found";
            if (isSatchelInstalled())
            {
                getVersionSafely();
            }
            return version;
        }
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

        public GlobalModSettings OnSaveGlobal()
        {
            return CustomKnight.GlobalSettings;
        }

        public CustomKnight()
        {
            SupportLazyModDevs.Hook();
        }



        public override List<(string, string)> GetPreloadNames()
        {
            List<(string, string)> preloadsList;
            if (GlobalSettings.Preloads || GlobalSettings.GenerateDefaultSkin)
            {  preloadsList = new List<(string, string)>
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
            } else
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
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            Log($"Initializing CustomKnight {version}");
            //Making sure skinables list exists
            Log($"Found {SkinManager.Skinables.Count} Skinables");
            SupportLazyModDevs.Unhook();
            SupportLazyModDevs.Enable();
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


            GUIController.Instance.BuildMenus();
            if (CustomKnight.GlobalSettings.EnablePauseMenu)
            {
                UIController.CreateGUI();
            }

            if (CustomKnight.GlobalSettings.GenerateDefaultSkin)
            {
                DefaultSkin.SaveSkin();
            } else
            {
                DefaultSkin.isGeneratingDefaultSkin = false;
            }
            On.HeroController.Start += HeroControllerStart;
        }

        public bool ToggleButtonInsideMenu { get; } = true;
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggle)
        {
            return BetterMenu.GetMenu(modListMenu, toggle);
        }

        public static event EventHandler<EventArgs> OnReady, OnInit, OnUnload;

        public void HeroControllerStart(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);
            Log("HeroControllerStart");
            var currentSkinId = (SaveSettings.DefaultSkin != GlobalSettings.DefaultSkin && SaveSettings.DefaultSkin != null) ? SaveSettings.DefaultSkin : GlobalSettings.DefaultSkin;
            SkinManager.CurrentSkin = SkinManager.GetSkinById(currentSkinId);
            SkinManager.LoadSkin();
            OnReady?.Invoke(this, null);
        }
        public void OnLoadLocal(SaveModSettings s)
        {
            CustomKnight.SaveSettings = s;
        }

        internal static void toggleSwap(bool enable)
        {
            swapManager.enabled = enable;
            if (!enable)
            {
                swapManager.Unload();
                dumpManager.enabled = false;
                SupportLazyModDevs.Disable();
            }
            else
            {
                swapManager.Load();
                SupportLazyModDevs.Enable();
            }
        }

        internal static void toggleDump(bool enable)
        {
            dumpManager.enabled = enable;
        }

        public void Unload()
        {
            SaveHud.UnHook();
            SkinManager.Unload();
            OnUnload?.Invoke(this, null);
            On.HeroController.Start -= HeroControllerStart;
        }
        public SaveModSettings OnSaveLocal()
        {
            return CustomKnight.SaveSettings;
        }


        public static void SaveSprite(Sprite s, string str)
        {
            var tex = SpriteUtils.ExtractTextureFromSprite(s);
            CustomKnight.dumpManager.SaveTextureByPath("Debug", str, tex);
        }
    }
}
