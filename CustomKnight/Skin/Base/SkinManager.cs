using System.IO;
using static Satchel.IoUtils;

namespace CustomKnight
{

    public static class SkinManager{
        internal static bool savedDefaultTextures = false;
        public static string DATA_DIR {get; internal set;}
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
            {Deathpt.NAME,new Deathpt()},
            {DDeathpt.NAME,new DDeathpt()},

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
            {"Charm_40_5",new Charm("Charm_40_5",40)},
            {"Nail_1",new Nail("Inventory/Nail_1") },
            {"Nail_2",new Nail("Inventory/Nail_2") },
            {"Nail_3",new Nail("Inventory/Nail_3") },
            {"Nail_4",new Nail("Inventory/Nail_4") },
            {"Nail_5",new Nail("Inventory/Nail_5") },
            {"Heart_0",new Heart("Inventory/Heart_0",0) },
            {"Heart_1",new Heart("Inventory/Heart_1",1) },
            {"Heart_2",new Heart("Inventory/Heart_2",2) },
            {"Heart_3",new Heart("Inventory/Heart_3",3) },
            {"Heart_4",new Heart("Inventory/Heart_4",4) },
            {"Vessel_0",new Vessel("Inventory/Vessel_0",0) },
            {"Vessel_1",new Vessel("Inventory/Vessel_1",1) },
            {"Vessel_2",new Vessel("Inventory/Vessel_2",2) },
            {"Vessel_3",new Vessel("Inventory/Vessel_3",3) },
            {"Fireball_1",new InvSpell("Inventory/Fireball_1",1) },
            {"Fireball_2",new InvSpell("Inventory/Fireball_2",2) },
            {"Quake_1",new InvSpell("Inventory/Quake_1",1) },
            {"Quake_2",new InvSpell("Inventory/Quake_2",2) },
            {"Scream_1",new InvSpell("Inventory/Scream_1",1) },
            {"Scream_2",new InvSpell("Inventory/Scream_2",2) },
            {"Focus",new InvSpell("Inventory/Focus",0) },
            {NailArtBG.Name,new NailArtBG() },
            {"DSlash",new InvNormal("Inventory/DSlash") },
            {"GSlash",new InvNormal("Inventory/GSlash") },
            {"CSlash",new InvNormal("Inventory/CSlash") },
            {"DreamGate",new InvNormal("Inventory/DreamGate") },
            {"DreamNail_0",new DreamNail("Inventory/DreamNail_0") },
            {"DreamNail_1",new DreamNail("Inventory/DreamNail_1") },
            {"InvGeo",new InvNormal("Inventory/Geo") },
            {"GodFinder_0",new GodFinder("Inventory/GodFinder_0") },
            {"GodFinder_1",new GodFinder("Inventory/GodFinder_1") },
            {"GodFinder_2",new GodFinder("Inventory/GodFinder_2") },
            {"Cloak_1",new InvCloak(1) },
            {"Cloak_2",new InvCloak(2) },
            {InvClaw.Name,new InvClaw() },
            {InvSD.Name ,new InvSD() },
            {Lantern.Name,new Lantern() },
            {InvDJ.Name,new InvDJ() },
            {Tear.Name,new Tear() },
            {SlyKey.Name,new SlyKey() },
            {ElegentKey.Name,new ElegentKey() },
            {WJournal.Name,new WJournal() },
            {HSeal.Name,new HSeal() },
            {Idol.Name,new Idol() },
            {BEgg.Name,new BEgg() },
            {"Flower_0",new Flower(0) },
            {"Flower_1",new Flower(1) },
            {TramPass.Name,new TramPass() },
            {Ore.Name,new Ore() },
            {CityKey.Name,new CityKey() },
            {LoveKey.Name,new LoveKey() },
            {Brand.Name,new Brand() },
            {REgg.Name,new REgg() },
            {SimpleKey.Name,new SimpleKey() },
            {"Map",new MAQ("Inventory/Map") },
            {"Quill",new MAQ("Inventory/Quill") },
            {"MapQuill",new MAQ("Inventory/MapQuill") },
            {RelicBG.Name,new RelicBG() },
            {SpellBG.Name,new SpellBG() },
            {DeathNail.NAME,new DeathNail() },
            {DeathAsh.NAME,new DeathAsh() },
            {BrummWave.NAME,new BrummWave() },
            {BrummShield.NAME,new BrummShield() },
            {FlowerBreak.NAME,new FlowerBreak() }
           // {"PinsScarab", new Pins()}
        };

        public static GameObject _inv;
        public static GameObject inv
        {
            get
            {
                if(_inv == null)
                {
                    _inv = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv");
                }
                return _inv;
            }
        }
        public static GameObject _equipment;
        public static GameObject equipment
        {
            get
            {
                if(_equipment == null)
                {
                    _equipment = inv.FindGameObjectInChildren("Equipment");
                }
                return _equipment;
            }
        }
        public static GameObject _invitem;
        public static GameObject invitem
        {
            get
            {
                if(_invitem == null)
                {
                    _invitem = inv.FindGameObjectInChildren("Inv_Items"); 
                }
                return _invitem;
            }
        }

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
            EnsureDirectory(Path.Combine(SKINS_FOLDER, "Default"));
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
            if(!skipFlash && HeroController.instance != null){
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
            CoroutineHelper.GetRunner().StartCoroutine(ChangeSkinRoutine(skipFlash));
        }
        public static event EventHandler<EventArgs> OnSetSkin;

        /// <summary>
        ///     Change the current skin, to the one whose id is provided.
        /// </summary>
        /// <param name="id">a <c>string</c> that uniquely identifies the skin</param>
        public static void SetSkinById(string id)
        {
            var Skin = GetSkinById(id);
            CustomKnight.Instance.Log("Trying to apply skin :" + Skin.GetId() + $" on save slot {GameManager.instance.profileID}");
            if(CurrentSkin != null && CurrentSkin.GetId() == Skin.GetId()) { return; } 
            CurrentSkin = Skin;
            BetterMenu.SelectedSkin(SkinManager.CurrentSkin.GetId());
            // use this when saving so you save to the right settings
            if(GameManager.instance.IsNonGameplayScene()){
                CustomKnight.GlobalSettings.DefaultSkin = CurrentSkin.GetId();
            } else {
                CustomKnight.GlobalSettings.DefaultSkin = CurrentSkin.GetId();
                CustomKnight.SaveSettings.DefaultSkin = CurrentSkin.GetId();
                CustomKnight.GlobalSettings.saveSkins[GameManager.instance.profileID-1] = CurrentSkin.GetId();
            };
            RefreshSkin(false);
            OnSetSkin?.Invoke(CustomKnight.Instance,new EventArgs());
        }      

   
    }

}
