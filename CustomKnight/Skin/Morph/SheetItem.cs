namespace CustomKnight
{
    internal class SheetItem
    {
        public Rect size;
        public Sprite sprite;
        public Texture2D texture;
        public Vector2 pivot = new Vector2(0.5f, 0.5f);

        public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
            Color[] rpixels = result.GetPixels(0);
            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);
            for (int px = 0; px < rpixels.Length; px++)
            {
                rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
            }
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }
        public Texture2D GetRegion(Texture2D original, Rect region)
        {
            int maxW = (int)Math.Max((float)size.width, (float)region.width);
            int maxH = (int)Math.Max((float)size.height, (float)region.height);
            Texture2D outTex = TextureUtils.createTextureOfColor(maxW, maxH, new Color32(0, 0, 0, 0));

            int xPadding = (int)((maxW - region.width) * pivot.x);
            int yPadding = (int)((maxH - region.height) * pivot.y);
            original.Apply();

            for (int i = (int)region.xMin; i < (int)region.xMax; i++)
            {
                for (int j = (int)region.yMin; j < (int)region.yMax; j++)
                {
                    var c = original.GetPixel(i, j);
                    var fx = xPadding + i - (int)region.xMin;
                    var fy = yPadding + j - (int)region.yMin;
                    outTex.SetPixel(fx, fy, c);
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
            }
            else
            {
                texture = origTex;
            }
            texture = GetRegion(texture, region);
            sprite = Sprite.Create(texture, size, pivot);
        }

        public void CorrectScale(float scale = 1f, int removePad = 0)
        {
            if (size.width != texture.width || size.height != texture.height || scale != 1f)
            {
                var aspectRatio = (float)texture.width / (float)texture.height;
                var newWidth = size.height * aspectRatio;
                var newHeight = size.height;
                if (newWidth > size.width)
                {
                    newWidth = size.width;
                    newHeight = size.width / aspectRatio;
                }
                texture = ScaleTexture(texture, (int)(newWidth * scale), (int)(newHeight * scale));
                texture = GetRegion(texture, new Rect(new Vector2(0 + removePad, 0 + removePad), new Vector2(texture.width - 2 * removePad, texture.height - 2 * removePad)));
            }
        }

        public void Overlay(Texture2D orbFull, int x, int y)
        {
            var xPadding = x - orbFull.width / 2;
            var yPadding = y - orbFull.height / 2;
            for (int i = 0; i < orbFull.width; i++)
            {
                for (int j = 0; j < orbFull.height; j++)
                {
                    var c = orbFull.GetPixel(i, j);
                    var co = texture.GetPixel(xPadding + i, yPadding + j);
                    c.r = c.r * c.a + co.r * (1 - c.a);
                    c.g = c.g * c.a + co.g * (1 - c.a);
                    c.b = c.b * c.a + co.b * (1 - c.a);
                    c.a = c.a + co.a * (1 - c.a);
                    texture.SetPixel(xPadding + i, yPadding + j, c);
                }
            }
            texture.Apply();
        }
    }

}
