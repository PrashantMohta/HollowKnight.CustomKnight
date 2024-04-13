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
        public static Dictionary<string, SheetItem> AreaBackgrounds = new();
        public static AreaBackground[] defaultAreaBackgrounds;

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
            var skin = SkinManager.GetSkinById(CustomKnight.GlobalSettings.saveSkins[GetSlotIndex(self.saveSlot)]);
            if (!skin.Exists($"SaveHud/geoIcon.png") && skin.Exists(SkinManager.Skinables[Hud.NAME].ckTex.fileName) && skin.Exists(SkinManager.Skinables[OrbFull.NAME].ckTex.fileName))
            {
                GenerateSaveHud(skin, skin.GetTexture(SkinManager.Skinables[Hud.NAME].ckTex.fileName), skin.GetTexture(SkinManager.Skinables[OrbFull.NAME].ckTex.fileName));
            }
            self.ggSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = ggSoulOrb.GetSpriteForSkin(skin) ?? self.ggSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            self.hardcoreSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = hardcoreSoulOrb.GetSpriteForSkin(skin) ?? self.hardcoreSoulOrbCg.gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            self.soulOrbIcon.sprite = soulOrbIcon.GetSpriteForSkin(skin) ?? self.soulOrbIcon.sprite;
            self.geoIcon.sprite = geoIcon.GetSpriteForSkin(skin) ?? self.geoIcon.sprite;

            self.mpSlots.normalSoulOrb = normalSoulOrb.GetSpriteForSkin(skin) ?? self.mpSlots.normalSoulOrb;
            self.mpSlots.steelSoulOrb = steelSoulOrb.GetSpriteForSkin(skin) ?? self.mpSlots.steelSoulOrb;
            self.healthSlots.normalHealth = normalHealth.GetSpriteForSkin(skin) ?? self.healthSlots.normalHealth;
            self.healthSlots.steelHealth = steelHealth.GetSpriteForSkin(skin) ?? self.healthSlots.steelHealth;
            orig(self, saveStats);
            if (skin.GetName() == "Default") // we dont ever want to generate area backgrounds for other skins (we will bundle for default too)
            {
                GenerateAreaBackgrounds(skin);
            }
            var currZone = !saveStats.bossRushMode ? saveStats.mapZone.ToString() : MapZone.GODS_GLORY.ToString();
            if (AreaBackgrounds.TryGetValue(currZone, out var mapzone))
            {
                self.background.sprite = mapzone.Exists(skin) ? mapzone.GetSpriteForSkin(skin) : self.background.sprite;
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
        public static void GenerateAreaBackgrounds(ISelectableSkin skin)
        {
            for (int i = 0; i < defaultAreaBackgrounds.Length; i++)
            {
                var areaName = defaultAreaBackgrounds[i].areaName.ToString();
                if (!AreaBackgrounds[areaName].Exists(skin))
                {
                    var tex = SpriteUtils.ExtractTextureFromSprite(defaultAreaBackgrounds[i].backgroundImage);
                    AreaBackgrounds[areaName].texture = tex;
                    AreaBackgrounds[areaName].Save(skin);
                }
            }
        }
        public static void GenerateSaveHud(Texture2D Hudpng, Texture2D OrbFull)
        {
            GenerateSaveHud(SkinManager.GetCurrentSkin(), Hudpng, OrbFull);
        }
        public static void GenerateSaveHud(ISelectableSkin skin, Texture2D Hudpng, Texture2D OrbFull)
        {
            var fHudpng = Hudpng.Flip(true, true);
            if (!geoIcon.Exists(skin))
            {
                geoIcon.useImage(fHudpng, 0f, 553f, 60f, 68f, false, false);
                geoIcon.rotateTexture(false);
                geoIcon.CorrectScale(1.1f, 3);
                geoIcon.Save(skin);
            }
            if (!ggSoulOrb.Exists(skin))
            {
                ggSoulOrb.useImage(fHudpng, 1507f, 1297f, 167f, 345f, false, false);
                ggSoulOrb.rotateTexture(false);
                ggSoulOrb.CorrectScale(2f);
                ggSoulOrb.texture = ggSoulOrb.texture.GetCropped(new Rect(0, 75, ggSoulOrb.size.width, ggSoulOrb.size.height));
                ggSoulOrb.Overlay(SheetItem.ScaleTexture(OrbFull, 125, 125), 83, 58);
                ggSoulOrb.Save(skin);
            }
            if (!hardcoreSoulOrb.Exists(skin))
            {
                hardcoreSoulOrb.useImage(fHudpng, 0f, 1847f, 501f, 201f, false, false);
                hardcoreSoulOrb.texture = hardcoreSoulOrb.texture.Flip(false, true);
                hardcoreSoulOrb.CorrectScale();
                hardcoreSoulOrb.Overlay(SheetItem.ScaleTexture(OrbFull, 109, 109), 128, 81);
                hardcoreSoulOrb.Save(skin);
            }
            if (!normalHealth.Exists(skin))
            {
                normalHealth.useImage(fHudpng, 275f, 813f, 65f, 59f, false, false);
                normalHealth.rotateTexture(false);
                normalHealth.CorrectScale();
                normalHealth.Save(skin);
            }
            if (!normalSoulOrb.Exists(skin))
            {
                normalSoulOrb.useImage(fHudpng, 75f, 681f, 45f, 48f, false, false);
                normalSoulOrb.rotateTexture(true);
                normalSoulOrb.CorrectScale();
                normalSoulOrb.Save(skin);
            }
            if (!soulOrbIcon.Exists(skin))
            {
                soulOrbIcon.useImage(fHudpng, 1360f, 1621f, 147f, 246f, false, false);
                soulOrbIcon.rotateTexture(false);
                soulOrbIcon.CorrectScale(1.1f, 5);
                soulOrbIcon.Overlay(SheetItem.ScaleTexture(OrbFull, 82, 82), 52, 42);
                soulOrbIcon.Save(skin);
            }
            if (!steelHealth.Exists(skin))
            {
                steelHealth.useImage(fHudpng, 275f, 813f, 65f, 59f, false, false);
                steelHealth.rotateTexture(false);
                steelHealth.CorrectScale();
                steelHealth.Save(skin);
            }
            if (!steelSoulOrb.Exists(skin))
            {
                steelSoulOrb.useImage(fHudpng, 75f, 681f, 45f, 48f, false, false);
                steelSoulOrb.rotateTexture(true);
                steelSoulOrb.CorrectScale();
                steelSoulOrb.Save(skin);
            }
        }

        internal static void LoadAll()
        {
            for (var i = 0; i < 4; i++)
            {
                var skin = SkinManager.GetSkinById(CustomKnight.GlobalSettings.saveSkins[i]);
                SheetItem.PreloadForSkin(skin);
            }
        }
    }
}
