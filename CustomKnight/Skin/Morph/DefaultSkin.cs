using System.IO;
using static Satchel.IoUtils;
namespace CustomKnight
{
    internal static class DefaultSkin
    {
        internal static bool isGeneratingDefaultSkin = false;
        private static IEnumerator GenerateSkin()
        {
            yield return new WaitWhile(() => HeroController.instance == null || CharmIconList.Instance == null);
            foreach (var skinable in SkinManager.Skinables.Values)
            {
                skinable.prepare();
            }
            SpriteLoader.PullDefaultTextures();
            foreach (var skinable in SkinManager.Skinables.Values)
            {
                CustomKnight.Instance.Log($"Trying to generate default skin for {skinable.name}");
                yield return new WaitWhile(() =>
                {
                    if (skinable.ckTex.defaultTex == null && skinable.ckTex.defaultSprite == null)
                    {
                        skinable.prepare();
                        skinable.SaveTexture();
                        return true;
                    }
                    return false;
                }
                );
                try
                {
                    skinable.DumpDefaultTexture();
                }
                catch (Exception ex)
                {
                    CustomKnight.Instance.Log($"Failed to generate default skin for {skinable.name}");
                    CustomKnight.Instance.Log(ex);
                }
            }
            isGeneratingDefaultSkin = false;
            CustomKnight.GlobalSettings.GenerateDefaultSkin = false;
            CustomKnight.Instance.Log($"Done Generating");
        }

        public static void SaveSkin()
        {
            isGeneratingDefaultSkin = true;
            CoroutineHelper.GetRunner().StartCoroutine(GenerateSkin());
        }
        public static void Save(Texture2D texture, string skinName, string path, bool overwrite = false)
        {
            var folderPath = Path.Combine(SkinManager.SKINS_FOLDER, skinName);
            var itemPath = Path.Combine(folderPath, path);
            EnsureDirectory(Path.GetDirectoryName(itemPath));
            if (File.Exists(itemPath) && !overwrite)
            {
                CustomKnight.Instance.Log($"File exists at {itemPath}, Skipping.");
                return;
            }
            Texture2D dupe = texture.isReadable ? texture : TextureUtils.duplicateTexture(texture);
            byte[] texBytes = dupe.EncodeToPNG();
            if (dupe != texture)
            {
                GameObject.Destroy(dupe);
            }

            try
            {
                File.WriteAllBytes(itemPath, texBytes);
            }
            catch (IOException e)
            {
                CustomKnight.Instance.Log(e.ToString());
            }

        }

        public static void Save(Texture2D texture, string path)
        {
            Save(texture, "Default", path);
        }

    }
}
