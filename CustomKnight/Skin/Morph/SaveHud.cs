using CustomKnight.Next.Skin;
using GlobalEnums;
using static UnityEngine.UI.SaveSlotButton;

namespace CustomKnight
{
    internal class SaveHud
    {
        public static SheetItem geoIcon = new SheetItem("SaveHud/geoIcon.png", 63f, 63f);
        public static SheetItem ggSoulOrb = new SheetItem("SaveHud/ggSoulOrb.png", 338f, 163f);
        public static SheetItem hardcoreSoulOrb = new SheetItem("SaveHud/hardcoreSoulOrb.png", 507f, 178f);
        public static SheetItem normalHealth = new SheetItem("SaveHud/normalHealth.png", 35f, 43f);
        public static SheetItem normalSoulOrb = new SheetItem("SaveHud/normalSoulOrb.png", 21f, 21f);
        public static SheetItem soulOrbIcon = new SheetItem("SaveHud/soulOrbIcon.png", 173f, 107f);
        public static SheetItem steelHealth = new SheetItem("SaveHud/steelHealth.png", 35f, 43f);
        public static SheetItem steelSoulOrb = new SheetItem("SaveHud/steelSoulOrb.png", 21f, 21f);
        public static SheetItem brokenSteelOrb = new SheetItem("SaveHud/brokenSteelOrb.png", 480f, 176f);
        public static SheetItem defeatedBackground = new SheetItem("AreaBackgrounds/defeatedBackground.png", 735f, 119f);
        public static Dictionary<string, SheetItem> AreaBackgrounds = new();
        public static AreaBackground[] defaultAreaBackgrounds;
        private static GameObject DefeatedBackgroundGo;
        private static GameObject BrokenSteelOrbGo;
        private static bool PendingGeneration = true;
        public static void ClearCache()
        {
            foreach (var zone in Enum.GetNames(typeof(GlobalEnums.MapZone)))
            {
                AreaBackgrounds[zone].ClearCache();
            }
            geoIcon.ClearCache();
            ggSoulOrb.ClearCache();
            hardcoreSoulOrb.ClearCache();
            normalHealth.ClearCache();
            normalSoulOrb.ClearCache();
            soulOrbIcon.ClearCache();
            steelHealth.ClearCache();
            steelSoulOrb.ClearCache();
        }
        public static void Hook()
        {
            foreach (var zone in Enum.GetNames(typeof(GlobalEnums.MapZone)))
            {
                AreaBackgrounds[zone] = new SheetItem($"AreaBackgrounds/{zone}.png", 0, 0);
            }
            On.SaveSlotBackgrounds.GetBackground_MapZone += SaveSlotBackgrounds_GetBackground_MapZone;
            On.UnityEngine.UI.SaveSlotButton.PresentSaveSlot += SaveSlotButton_PresentSaveSlot;
        }
        public static void UnHook()
        {
            On.SaveSlotBackgrounds.GetBackground_MapZone -= SaveSlotBackgrounds_GetBackground_MapZone;
            On.UnityEngine.UI.SaveSlotButton.PresentSaveSlot -= SaveSlotButton_PresentSaveSlot;
        }
        private static void SaveSlotButton_PresentSaveSlot(On.UnityEngine.UI.SaveSlotButton.orig_PresentSaveSlot orig, UnityEngine.UI.SaveSlotButton self, SaveStats saveStats)
        {
            var skin = SkinManager.GetSkinById(GlobalModSettings.GetSkinForProfileID(GetSlotIndex(self.saveSlot)));

            self.ggSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = ggSoulOrb.GetSpriteForSkin(skin) ?? self.ggSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            self.hardcoreSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = hardcoreSoulOrb.GetSpriteForSkin(skin) ?? self.hardcoreSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            self.soulOrbIcon.sprite = soulOrbIcon.GetSpriteForSkin(skin) ?? self.soulOrbIcon.sprite;
            self.geoIcon.sprite = geoIcon.GetSpriteForSkin(skin) ?? self.geoIcon.sprite;

            self.mpSlots.normalSoulOrb = normalSoulOrb.GetSpriteForSkin(skin) ?? self.mpSlots.normalSoulOrb;
            self.mpSlots.steelSoulOrb = steelSoulOrb.GetSpriteForSkin(skin) ?? self.mpSlots.steelSoulOrb;
            self.healthSlots.normalHealth = normalHealth.GetSpriteForSkin(skin) ?? self.healthSlots.normalHealth;
            self.healthSlots.steelHealth = steelHealth.GetSpriteForSkin(skin) ?? self.healthSlots.steelHealth;

            DefeatedBackgroundGo = self.gameObject.FindGameObjectInChildren("DefeatedBackground");
            if (DefeatedBackgroundGo != null)
            {
                DefeatedBackgroundGo.GetComponent<UnityEngine.UI.Image>().sprite = defeatedBackground.GetSpriteForSkin(skin) ?? DefeatedBackgroundGo.GetComponent<UnityEngine.UI.Image>().sprite;
            }
            BrokenSteelOrbGo = self.gameObject.FindGameObjectInChildren("BrokenSteelOrb");
            if (BrokenSteelOrbGo != null)
            {
                BrokenSteelOrbGo.GetComponent<UnityEngine.UI.Image>().sprite = brokenSteelOrb.GetSpriteForSkin(skin) ?? BrokenSteelOrbGo.GetComponent<UnityEngine.UI.Image>().sprite;
            }
            orig(self, saveStats);
            
            var currZone = !saveStats.bossRushMode ? saveStats.mapZone.ToString() : MapZone.GODS_GLORY.ToString();
            if (AreaBackgrounds.TryGetValue(currZone, out var mapzone))
            {/*
              * todo redo
                self.background.sprite = mapzone.Exists(skin) ? mapzone.GetSpriteForSkin(skin) : self.background.sprite;
                */
            }
        }


        private static int GetSlotIndex(SaveSlot slot)
        {
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
            return index;
        }

        private static AreaBackground SaveSlotBackgrounds_GetBackground_MapZone(On.SaveSlotBackgrounds.orig_GetBackground_MapZone orig, SaveSlotBackgrounds self, GlobalEnums.MapZone mapZone)
        {
            defaultAreaBackgrounds = self.areaBackgrounds;
            return orig(self, mapZone);
        }

        internal static Texture2D GetFromAssemblyOrExtract(Sprite sprite, string name)
        {
            Texture2D tex = AssemblyUtils.GetTextureFromResources($"{name.Replace('/', '.')}");
            if (tex != null)
            {
                CustomKnight.Instance.Log($"Extracting {name} from Assembly Resource");
            }
            else if (sprite != null)
            {
                CustomKnight.Instance.Log($"Extracting {name} from Default Sprite");
                tex = SpriteUtils.ExtractTextureFromSprite(sprite);
            }
            return tex;
        }


        internal static void LoadAll()
        {
            for (var i = 0; i < 4; i++)
            {
                var skin = SkinManager.GetSkinById(GlobalModSettings.GetSkinForProfileID(i));
                SheetItem.PreloadForSkin(skin);
            }
        }
    }
}
