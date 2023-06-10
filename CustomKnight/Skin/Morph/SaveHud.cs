using GlobalEnums;
using System.IO;
using static UnityEngine.UI.SaveSlotButton;

namespace CustomKnight
{
    internal class SaveHud
    {
        public static SheetItem geoIcon = new SheetItem(63f, 63f);
        public static SheetItem ggSoulOrb = new SheetItem(338f, 163f);
        public static SheetItem hardcoreSoulOrb = new SheetItem(507f, 178f);
        public static SheetItem normalHealth = new SheetItem(35f, 43f);
        public static SheetItem normalSoulOrb = new SheetItem(21f, 21f);
        public static SheetItem soulOrbIcon = new SheetItem(173f, 107f);
        public static SheetItem steelHealth = new SheetItem(35f, 43f);
        public static SheetItem steelSoulOrb = new SheetItem(21f, 21f);

        public static void Hook()
        {
            //On.SaveSlotBackgrounds.GetBackground_MapZone += SaveSlotBackgrounds_GetBackground_MapZone;
            On.UnityEngine.UI.SaveSlotButton.PresentSaveSlot += SaveSlotButton_PresentSaveSlot;
        }

        public static void UnHook()
        {
            //On.SaveSlotBackgrounds.GetBackground_MapZone -= SaveSlotBackgrounds_GetBackground_MapZone;
            On.UnityEngine.UI.SaveSlotButton.PresentSaveSlot -= SaveSlotButton_PresentSaveSlot;
        }
        private static void SaveSlotButton_PresentSaveSlot(On.UnityEngine.UI.SaveSlotButton.orig_PresentSaveSlot orig, UnityEngine.UI.SaveSlotButton self, SaveStats saveStats)
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
            self.background.sprite = GetSpriteForMapZone(self.saveSlot, !saveStats.bossRushMode ? saveStats.mapZone.ToString() : MapZone.GODS_GLORY.ToString()) ?? self.background.sprite;
        }

        private static Sprite GetSpriteFromFile(SaveSlot slot, string file)
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
            var skin = SkinManager.GetSkinById(CustomKnight.GlobalSettings.saveSkins[index]);
            if (skin.Exists(file))
            {
                var tex = skin.GetTexture(file);
                var pivot = new Vector2(0.5f, 0.5f);
                return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), pivot);
            }
            return null;
        }
        private static Sprite GetSpriteForMapZone(SaveSlot slot, string mapZone)
        {
            var file = $"AreaBackgrounds/{mapZone}.png";
            return GetSpriteFromFile(slot, file);
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

        public static bool Exists(string name)
        {
            var state = SkinManager.GetCurrentSkin().Exists($"SaveHud/{name}.png");
            if (!state)
            {
                CustomKnight.Instance.Log($"Missing SaveHud/{name}.png will be generated.");

            }
            return state;
        }
        public static void SaveFile(string name, SheetItem item)
        {
            var skinDir = Path.Combine(SkinManager.SKINS_FOLDER, SkinManager.GetCurrentSkin().GetId());
            IoUtils.EnsureDirectory(skinDir);
            var SaveHudDir = Path.Combine(skinDir, "SaveHud"); 
            IoUtils.EnsureDirectory(SaveHudDir);
            TextureUtils.WriteTextureToFile(item.texture, SaveHudDir + $"/{name}.png");
        }
        public static void GenerateSaveHud(Texture2D Hudpng,Texture2D OrbFull)
        {
            var fHudpng = Hudpng.Flip(true, true);
            if (!Exists("geoIcon")) { 
                geoIcon.useImage(fHudpng, 125f, 610f, 51f, 59f,false, false);
                geoIcon.rotateTexture(false);
                geoIcon.CorrectScale(1.3f,10);
                SaveFile("geoIcon", geoIcon);
            }
            if (!Exists("ggSoulOrb")) { 
                ggSoulOrb.useImage(fHudpng, 505f, 1681f, 338f, 163f, false, false);
                ggSoulOrb.texture = ggSoulOrb.texture.Flip(false, true);
                ggSoulOrb.CorrectScale();
                ggSoulOrb.Overlay(SheetItem.ScaleTexture(OrbFull,125,125),76,56);
                SaveFile("ggSoulOrb", ggSoulOrb);
            }
            if (!Exists("hardcoreSoulOrb")) { 
                hardcoreSoulOrb.useImage(fHudpng, 508f, 1847f, 501f, 201f, false, false);
                hardcoreSoulOrb.texture = hardcoreSoulOrb.texture.Flip(false, true);
                hardcoreSoulOrb.CorrectScale();
                hardcoreSoulOrb.Overlay(SheetItem.ScaleTexture(OrbFull, 109, 109), 125, 81);
                SaveFile("hardcoreSoulOrb", hardcoreSoulOrb);
            }
            if (!Exists("normalHealth")) { 
                normalHealth.useImage(fHudpng, 275f, 813f, 65f, 59f, false, false);
                normalHealth.rotateTexture(false);
                normalHealth.CorrectScale();
                SaveFile("normalHealth", normalHealth);
            }
            if (!Exists("normalSoulOrb")) { 
                normalSoulOrb.useImage(fHudpng, 901f, 629f, 40f, 40f, false, false);
                normalSoulOrb.rotateTexture(false);
                normalSoulOrb.CorrectScale();
                SaveFile("normalSoulOrb", normalSoulOrb);
            }
            if (!Exists("soulOrbIcon")) { 
                soulOrbIcon.useImage(fHudpng, 1360f, 1621f, 147f, 246f, false, false);
                soulOrbIcon.rotateTexture(false);
                soulOrbIcon.CorrectScale(1.1f, 5);
                soulOrbIcon.Overlay(SheetItem.ScaleTexture(OrbFull, 82, 82), 52, 42);
                SaveFile("soulOrbIcon", soulOrbIcon);
            }
            if (!Exists("steelHealth")) { 
                steelHealth.useImage(fHudpng, 275f, 813f, 65f, 59f, false, false);
                steelHealth.rotateTexture(false);
                steelHealth.CorrectScale();
                SaveFile("steelHealth", steelHealth);
            }
            if (!Exists("steelSoulOrb")) { 
                steelSoulOrb.useImage(fHudpng, 901f, 629f, 40f, 40f, false, false);
                steelSoulOrb.rotateTexture(false);
                steelSoulOrb.CorrectScale();
                SaveFile("steelSoulOrb", steelSoulOrb);
            }
        }
    }
}
