using CustomKnight.Canvas;
using CustomKnight.Skin.Swapper;
using GlobalEnums;
using static UnityEngine.UI.SaveSlotButton;

namespace CustomKnight
{
    public class CustomKnight : Mod,  IGlobalSettings<GlobalModSettings>, ILocalSettings<SaveModSettings>,ICustomMenuMod , ITogglableMod
    {
        public static GlobalModSettings GlobalSettings { get; set; } = new GlobalModSettings();
        public static SaveModSettings SaveSettings { get; set; } = new SaveModSettings();
        public static CustomKnight Instance { get; private set; }
        public static DumpManager dumpManager {get; private set;} = new DumpManager();
        public static SwapManager swapManager {get; private set;} = new SwapManager();

        public static CinematicsManager cinematicsManager {get; private set;} = new CinematicsManager();

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
            //On.SaveSlotBackgrounds.GetBackground_MapZone += SaveSlotBackgrounds_GetBackground_MapZone;
            On.UnityEngine.UI.SaveSlotButton.PresentSaveSlot += SaveSlotButton_PresentSaveSlot;
        }

        private void SaveSlotButton_PresentSaveSlot(On.UnityEngine.UI.SaveSlotButton.orig_PresentSaveSlot orig, UnityEngine.UI.SaveSlotButton self, SaveStats saveStats)
        {
            /*
            SaveSprite(self.ggSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite, "ggSoulOrb");
            SaveSprite(self.hardcoreSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite, "hardcoreSoulOrb");
            SaveSprite(self.soulOrbIcon.sprite, "soulOrbIcon");
            SaveSprite(self.healthSlots.normalHealth, "normalHealth");
            SaveSprite(self.healthSlots.steelHealth, "steelHealth");
            SaveSprite(self.geoIcon.sprite, "geoIcon");
            SaveSprite(self.mpSlots.normalSoulOrb, "normalSoulOrb");
            SaveSprite(self.mpSlots.steelSoulOrb, "steelSoulOrb"); 
            */
            self.ggSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = GetSpriteFromFile(self.saveSlot, "SaveHud/ggSoulOrb.png") ?? self.ggSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            self.hardcoreSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = GetSpriteFromFile(self.saveSlot, "SaveHud/hardcoreSoulOrb.png") ?? self.hardcoreSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            self.soulOrbIcon.sprite = GetSpriteFromFile(self.saveSlot, "SaveHud/soulOrbIcon.png") ?? self.soulOrbIcon.sprite;
            self.geoIcon.sprite = GetSpriteFromFile(self.saveSlot, "SaveHud/geoIcon.png") ?? self.geoIcon.sprite;

            self.mpSlots.normalSoulOrb = GetSpriteFromFile(self.saveSlot, "SaveHud/normalSoulOrb.png") ?? self.mpSlots.normalSoulOrb;
            self.mpSlots.steelSoulOrb = GetSpriteFromFile(self.saveSlot, "SaveHud/steelSoulOrb.png") ?? self.mpSlots.steelSoulOrb;
            self.healthSlots.normalHealth = GetSpriteFromFile(self.saveSlot, "SaveHud/normalHealth.png") ?? self.healthSlots.normalHealth;
            self.healthSlots.steelHealth = GetSpriteFromFile(self.saveSlot, "SaveHud/steelHealth.png") ?? self.healthSlots.steelHealth;
            orig(self, saveStats);
            self.background.sprite = GetSpriteForMapZone(self.saveSlot,!saveStats.bossRushMode ? saveStats.mapZone.ToString() : MapZone.GODS_GLORY.ToString()) ?? self.background.sprite;
        }

        private Sprite GetSpriteFromFile(SaveSlot slot,string file)
        {
            Log(slot.ToString() + "" + slot);
            var index = 0;
            if (slot == SaveSlot.SLOT_1)
            {
                index = 0;
            }
            else if (slot == SaveSlot.SLOT_2)
            {
                index = 1;
            }
            else if (slot == SaveSlot.SLOT_3)
            {
                index = 2;
            }
            else if (slot == SaveSlot.SLOT_4)
            {
                index = 3;
            }
            var skin = SkinManager.GetSkinById(CustomKnight.GlobalSettings.saveSkins[index]);
            if (skin.Exists(file))
            {
                var tex = skin.GetTexture(file);
                var pivot = new Vector2(0.5f, 0.5f);
                return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), pivot);
            }
            return null;
        }
        private Sprite GetSpriteForMapZone(SaveSlot slot, string mapZone)
        {
            var file = $"AreaBackgrounds/{mapZone}.png";
            return GetSpriteFromFile(slot,file);
        }
        
        private void SaveSprite(Sprite s,string str)
        {
            var tex = SpriteUtils.ExtractTextureFromSprite(s);
            dumpManager.SaveTextureByPath("Debug", str, tex);
        }
        private AreaBackground SaveSlotBackgrounds_GetBackground_MapZone(On.SaveSlotBackgrounds.orig_GetBackground_MapZone orig, SaveSlotBackgrounds self, GlobalEnums.MapZone mapZone)
        {
            /*for (int i = 0; i < self.areaBackgrounds.Length; i++)
            {
                var tex = SpriteUtils.ExtractTextureFromSprite(self.areaBackgrounds[i].backgroundImage);
                dumpManager.SaveTextureByPath("AreaBackgrounds", self.areaBackgrounds[i].areaName.ToString(), tex);

            }*/
            return orig(self, mapZone);
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
            SupportLazyModDevs.Unhook();
            SupportLazyModDevs.Enable();
            Log($"Initializing CustomKnight {version}");
            if (Instance == null) 
            { 
                Instance = this;
            }
            if(!isSatchelInstalled()){
                return;
            }
            SkinManager.checkDirectoryStructure();

            SkinManager.getSkinNames();             
            SkinManager.CurrentSkin = SkinManager.GetSkinById(CustomKnight.GlobalSettings.DefaultSkin);
            SkinManager.SetSkinById(CustomKnight.GlobalSettings.DefaultSkin);
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
            }
            if(CustomKnight.GlobalSettings.SwapperEnabled){
                swapManager.enabled = true;
                swapManager.active = true;
            }


            GUIController.Instance.BuildMenus();
            On.HeroController.Start += HeroControllerStart;
        }

        public  bool ToggleButtonInsideMenu {get;}= true;
        public MenuScreen GetMenuScreen(MenuScreen modListMenu,ModToggleDelegates? toggle){
            return BetterMenu.GetMenu(modListMenu,toggle);
        }

        public static event EventHandler<EventArgs> OnReady;

        public void HeroControllerStart(On.HeroController.orig_Start orig,HeroController self){
            orig(self);
            Log("HeroControllerStart");
            var currentSkinId = ( SaveSettings.DefaultSkin != GlobalSettings.DefaultSkin && SaveSettings.DefaultSkin != null ) ? SaveSettings.DefaultSkin : GlobalSettings.DefaultSkin;
            SkinManager.CurrentSkin = SkinManager.GetSkinById(currentSkinId);
            SkinManager.LoadSkin();
            OnReady?.Invoke(this, null);
        }
        public void OnLoadLocal(SaveModSettings s)
        {
            CustomKnight.SaveSettings = s;
        }

        internal static void toggleSwap(bool enable){
            swapManager.enabled = enable;
            if(!enable){
                swapManager.Unload();
                dumpManager.enabled = false;
                SupportLazyModDevs.Disable();
            } else {
                swapManager.Load();
                SupportLazyModDevs.Enable();
            }
        }

        internal static void toggleDump(bool enable){
            dumpManager.enabled = enable;
        }

        public void Unload(){
            SkinManager.Unload();
            On.HeroController.Start -= HeroControllerStart;
        }
        public SaveModSettings OnSaveLocal()
        {
            return CustomKnight.SaveSettings;
        }
        
    }
}
