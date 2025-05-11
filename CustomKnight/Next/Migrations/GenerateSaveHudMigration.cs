using System.IO;
using CustomKnight.Next.Skin;
using UnityEngine;
using static CustomKnight.SaveHud;
namespace CustomKnight.Next.Migrations
{
    internal class GenerateSaveHudMigration : BaseMigration
    {
        public override void Run(MigrationContext context)
        {
            var hudPath = Path.Combine(context.SkinPath, "ui", "hud.png");
            var OrbfullPath = Path.Combine(context.SkinPath, "ui", "orbfull.png");
            if (File.Exists(hudPath) && File.Exists(OrbfullPath))
            {
                GenerateSaveHud(context, TextureUtils.LoadTextureFromFile(hudPath), TextureUtils.LoadTextureFromFile(OrbfullPath));
            }
            if (context.SkinName == SkinManager.GetDefaultSkin().GetName()) // we dont ever want to generate area backgrounds for other skins (we will bundle for default too)
            {
                var overwrite = CustomKnight.GlobalSettings.GenerateDefaultSkin;
                ExtractDefaultSaveHud(context, overwrite);
                GenerateAreaBackgrounds(context, overwrite);
                if ((!Exists(context, defeatedBackground) || overwrite))
                {
                    var tex = GetFromAssemblyOrExtract(null, defeatedBackground.path);
                    defeatedBackground.texture = tex;
                    Save(context, defeatedBackground);
                }
                if ((!Exists(context, brokenSteelOrb) || CustomKnight.GlobalSettings.GenerateDefaultSkin))
                {
                    var tex = GetFromAssemblyOrExtract(null, brokenSteelOrb.path);
                    brokenSteelOrb.texture = tex;
                    Save(context, brokenSteelOrb);
                }
            }
        }


        public void GenerateAreaBackgrounds(MigrationContext context, bool overwrite = false)
        {
            if (defaultAreaBackgrounds == null)
            {
                return;
            }
            for (int i = 0; i < defaultAreaBackgrounds.Length; i++)
            {
                var areaName = defaultAreaBackgrounds[i].areaName.ToString();
                if (!Exists(context, AreaBackgrounds[areaName]) || overwrite)
                {

                    var tex = GetFromAssemblyOrExtract(defaultAreaBackgrounds[i].backgroundImage, AreaBackgrounds[areaName].path);
                    AreaBackgrounds[areaName].texture = tex;
                    Save(context,AreaBackgrounds[areaName]);
                }
            }
        }
        private void ExtractDefaultSaveHud(MigrationContext context, bool overwrite = false)
        {
            var sheets = new List<SheetItem>() { geoIcon, ggSoulOrb, hardcoreSoulOrb, normalHealth, normalSoulOrb, soulOrbIcon, steelHealth, steelSoulOrb };
            foreach (var sheet in sheets)
            {
                if (!overwrite) continue;
                var tex = GetFromAssemblyOrExtract(null, sheet.path);
                sheet.texture = tex;
                Save(context, sheet);
            }
        }
        public bool Exists(MigrationContext context, SheetItem item)
        {
            var path = Path.Combine(context.SkinPath, item.path);
            var result = File.Exists(path);
            if (!result)
            {
                CustomKnight.Instance.Log($"Missing {path} in skin {context.SkinName}");
            }
            return result;
        }


        public void Save(MigrationContext context, SheetItem item)
        {
            var skinDir = Path.Combine(SkinManager.SKINS_FOLDER, context.SkinName);
            var filepath = Path.Combine(skinDir, item.path);
            IoUtils.EnsureDirectory(Path.GetDirectoryName(filepath));
            if (item.texture == null && item.sprite != null)
            {
                item.texture = SpriteUtils.ExtractTextureFromSprite(item.sprite);
            }
            TextureUtils.WriteTextureToFile(item.texture, filepath);
        }
        public void GenerateSaveHud(MigrationContext context, Texture2D Hudpng, Texture2D OrbFull)
        {
            var fHudpng = Hudpng.Flip(true, true);
            if (!Exists(context, geoIcon))
            {
                geoIcon.useImage(fHudpng, 0f, 553f, 60f, 68f, false, false);
                geoIcon.rotateTexture(false);
                geoIcon.CorrectScale(1.1f, 3);
                Save(context, geoIcon);
            }
            if (!Exists(context, ggSoulOrb))
            {
                ggSoulOrb.useImage(fHudpng, 1507f, 1297f, 167f, 345f, false, false);
                ggSoulOrb.rotateTexture(false);
                ggSoulOrb.CorrectScale(2f);
                ggSoulOrb.texture = ggSoulOrb.texture.GetCropped(new Rect(0, 75, ggSoulOrb.size.width, ggSoulOrb.size.height));
                ggSoulOrb.Overlay(SheetItem.ScaleTexture(OrbFull, 125, 125), 83, 58);
                Save(context, ggSoulOrb);
            }
            if (!Exists(context, hardcoreSoulOrb))
            {
                hardcoreSoulOrb.useImage(fHudpng, 0f, 1847f, 501f, 201f, false, false);
                hardcoreSoulOrb.texture = hardcoreSoulOrb.texture.Flip(false, true);
                hardcoreSoulOrb.CorrectScale();
                hardcoreSoulOrb.Overlay(SheetItem.ScaleTexture(OrbFull, 109, 109), 128, 81);
                Save(context, hardcoreSoulOrb);
            }
            if (!Exists(context, normalHealth))
            {
                normalHealth.useImage(fHudpng, 275f, 813f, 65f, 59f, false, false);
                normalHealth.rotateTexture(false);
                normalHealth.CorrectScale();
                Save(context, normalHealth);
            }
            if (!Exists(context, normalSoulOrb))
            {
                normalSoulOrb.useImage(fHudpng, 75f, 681f, 45f, 48f, false, false);
                normalSoulOrb.rotateTexture(true);
                normalSoulOrb.CorrectScale();
                Save(context, normalSoulOrb);
            }
            if (!Exists(context, soulOrbIcon))
            {
                soulOrbIcon.useImage(fHudpng, 1360f, 1621f, 147f, 246f, false, false);
                soulOrbIcon.rotateTexture(false);
                soulOrbIcon.CorrectScale(1.1f, 5);
                soulOrbIcon.Overlay(SheetItem.ScaleTexture(OrbFull, 82, 82), 52, 42);
                Save(context, soulOrbIcon);
            }
            if (!Exists(context, steelHealth))
            {
                steelHealth.useImage(fHudpng, 275f, 813f, 65f, 59f, false, false);
                steelHealth.rotateTexture(false);
                steelHealth.CorrectScale();
                Save(context, steelHealth);
            }
            if (!Exists(context, steelSoulOrb))
            {
                steelSoulOrb.useImage(fHudpng, 75f, 681f, 45f, 48f, false, false);
                steelSoulOrb.rotateTexture(true);
                steelSoulOrb.CorrectScale();
                Save(context, steelSoulOrb);
            }
            if (!Exists(context, brokenSteelOrb))
            {
                brokenSteelOrb.useImage(fHudpng, 1025f, 1860f, 480f, 201f, false, false);
                brokenSteelOrb.texture = brokenSteelOrb.texture.Flip(false, true);
                brokenSteelOrb.CorrectScale();
                Save(context, brokenSteelOrb);
            }
        }
    }
}