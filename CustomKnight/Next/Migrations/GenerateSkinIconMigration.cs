using System.IO;
using CustomKnight.Next.Skin;

namespace CustomKnight.Next.Migrations
{
    internal class GenerateSkinIconMigration : BaseMigration
    {
        public override void Run(MigrationContext context)
        {

            var knightPath = Path.Combine(context.SkinPath, "knight", "knight.png");
            var orbFull = Path.Combine("ui","orbfull.png");
            var orbIcon = Path.Combine("ui", "orbicon.png");
            var OrbfullPath = Path.Combine(context.SkinPath, orbFull);
            var OrbIconPath = Path.Combine(context.SkinPath, orbIcon);
            if (File.Exists(OrbIconPath))
            {
                return;
            }
            Texture2D defaultOrb = Texture2D.blackTexture;

            if (File.Exists(knightPath))
            {
                var defaultSkin = SkinManager.GetDefaultSkin();
                if (File.Exists(OrbfullPath))
                {
                    defaultOrb = TextureUtils.LoadTextureFromFile(OrbfullPath);
                }
                else if (DefaultSkin.Exists(orbFull))
                {
                    defaultOrb = DefaultSkin.GetTexture(orbFull);
                }
                var tex = TextureUtils.LoadTextureFromFile(knightPath).GetCropped(new Rect(2802f, 4096f - 3155f, 86f, 120f));
                if (defaultOrb != Texture2D.blackTexture)
                {
                    tex = SheetItem.Overlay(defaultOrb, tex, 50, 65);
                }
                DefaultSkin.Save(tex, context.SkinName, orbIcon, false);
            }

        }
    }
}