using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomKnight
{
    internal class SheetItem { 
        public Rect size;
        public Sprite sprite;
        public Texture2D texture;
        public Vector2 pivot = new Vector2(0.5f, 0.5f);
        public Texture2D Transform(Texture2D original, Rect region)
        {
            int maxW = (int)Math.Max((float)size.width, (float)region.width);
            int maxH = (int)Math.Max((float)size.height, (float) region.height);
            Texture2D outTex = TextureUtils.createTextureOfColor(maxW, maxH, new Color32(0,0,0,0));
             
            int xPadding = (int)((maxW - region.width) * pivot.x);
            int yPadding = (int)((maxH - region.height) * pivot.y);
            CustomKnight.Instance.Log($"rect2 {region.xMin},{region.xMax},{region.yMin},{region.yMax}");
            original.Apply();

            for (int i = (int)region.xMin; i < (int)region.xMax; i++)
            {
                for (int j = (int)region.yMin; j < (int)region.yMax; j++) 
                { 
                    var c = original.GetPixel(i, j);
                    var fx = xPadding + i - (int)region.xMin;
                    var fy = yPadding + j - (int)region.yMin;
                    CustomKnight.Instance.Log($"({i},{j}):({fx},{fy})");
                    outTex.SetPixel(fx,fy , c);
                }
            }

            outTex.Apply();
            return outTex;
        }

        public void rotateTexture(bool clockwise)
        {
            Color32[] original = texture.GetPixels32();
            Color32[] rotated = new Color32[original.Length];
            int w = texture.width;
            int h = texture.height;

            int iRotated, iOriginal;

            for (int j = 0; j < h; ++j)
            {
                for (int i = 0; i < w; ++i)
                {
                    iRotated = (i + 1) * h - j - 1;
                    iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                    rotated[iRotated] = original[iOriginal];
                }
            }

            Texture2D rotatedTexture = new Texture2D(h, w);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            texture = rotatedTexture;
        }

        public SheetItem(float width, float height)
        {
            this.size = new Rect(new Vector2(0, 0), new Vector2(width, height));
        }

        public void useImage(Texture2D origTex, float x, float y, float width, float height, bool flipH = false, bool flipV = false)
        {
            var region = new Rect(new Vector2(x, y), new Vector2(width, height));
            if (flipH || flipV)
            {
                texture = origTex.Flip(flipH, flipV);
            } else
            {
                texture = origTex;
            }
            texture = Transform(texture, region);
            
            sprite = Sprite.Create(texture, size, pivot);
        }
    }
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
        public static void GenerateSaveHud(Texture2D Hudpng)
        {
            var fHudpng = Hudpng.Flip(true, true);

            geoIcon.useImage(fHudpng, 125f, 610f, 51f, 59f,false, false);
            geoIcon.rotateTexture(false);
            SaveFile("geoIcon", geoIcon);

            ggSoulOrb.useImage(fHudpng, 505f, 1681f, 338f, 163f, false, false);
            ggSoulOrb.texture = ggSoulOrb.texture.Flip(false, true);
            SaveFile("ggSoulOrb", ggSoulOrb);

            hardcoreSoulOrb.useImage(fHudpng, 508f, 1847f, 501f, 201f, false, false);
            hardcoreSoulOrb.texture = hardcoreSoulOrb.texture.Flip(false, true);
            SaveFile("hardcoreSoulOrb", hardcoreSoulOrb);

            normalHealth.useImage(fHudpng, 275f, 813f, 65f, 59f, false, false);
            normalHealth.rotateTexture(false);
            SaveFile("normalHealth", normalHealth);

            normalSoulOrb.useImage(fHudpng, 901f, 629f, 40f, 40f, false, false);
            normalSoulOrb.rotateTexture(false);
            SaveFile("normalSoulOrb", normalSoulOrb);

            soulOrbIcon.useImage(fHudpng, 1360f, 1621f, 147f, 246f, false, false);
            soulOrbIcon.rotateTexture(false);
            SaveFile("soulOrbIcon", soulOrbIcon);

            steelHealth.useImage(fHudpng, 275f, 813f, 65f, 59f, false, false);
            steelHealth.rotateTexture(false);
            SaveFile("steelHealth", steelHealth);

            steelSoulOrb.useImage(fHudpng, 901f, 629f, 40f, 40f, false, false);
            steelSoulOrb.rotateTexture(false);
            SaveFile("steelSoulOrb", steelSoulOrb);

        }
    }
}
