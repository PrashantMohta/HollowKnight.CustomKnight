using System.IO;

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

            geoIcon.useImage(fHudpng, 125f, 610f, 51f, 59f,false, false);
            geoIcon.rotateTexture(false);
            geoIcon.CorrectScale(1.3f,10);
            SaveFile("geoIcon", geoIcon);

            ggSoulOrb.useImage(fHudpng, 505f, 1681f, 338f, 163f, false, false);
            ggSoulOrb.texture = ggSoulOrb.texture.Flip(false, true);
            ggSoulOrb.CorrectScale();
            ggSoulOrb.Overlay(SheetItem.ScaleTexture(OrbFull,125,125),76,56);
            SaveFile("ggSoulOrb", ggSoulOrb);

            hardcoreSoulOrb.useImage(fHudpng, 508f, 1847f, 501f, 201f, false, false);
            hardcoreSoulOrb.texture = hardcoreSoulOrb.texture.Flip(false, true);
            hardcoreSoulOrb.CorrectScale();
            hardcoreSoulOrb.Overlay(SheetItem.ScaleTexture(OrbFull, 109, 109), 125, 81);
            SaveFile("hardcoreSoulOrb", hardcoreSoulOrb);

            normalHealth.useImage(fHudpng, 275f, 813f, 65f, 59f, false, false);
            normalHealth.rotateTexture(false);
            normalHealth.CorrectScale();
            SaveFile("normalHealth", normalHealth);

            normalSoulOrb.useImage(fHudpng, 901f, 629f, 40f, 40f, false, false);
            normalSoulOrb.rotateTexture(false);
            normalSoulOrb.CorrectScale();
            SaveFile("normalSoulOrb", normalSoulOrb);

            soulOrbIcon.useImage(fHudpng, 1360f, 1621f, 147f, 246f, false, false);
            soulOrbIcon.rotateTexture(false);
            soulOrbIcon.CorrectScale(1.1f, 5);
            soulOrbIcon.Overlay(SheetItem.ScaleTexture(OrbFull, 82, 82), 52, 42);
            SaveFile("soulOrbIcon", soulOrbIcon);

            steelHealth.useImage(fHudpng, 275f, 813f, 65f, 59f, false, false);
            steelHealth.rotateTexture(false);
            steelHealth.CorrectScale();
            SaveFile("steelHealth", steelHealth);

            steelSoulOrb.useImage(fHudpng, 901f, 629f, 40f, 40f, false, false);
            steelSoulOrb.rotateTexture(false);
            steelSoulOrb.CorrectScale();
            SaveFile("steelSoulOrb", steelSoulOrb);

        }
    }
}
