using System.IO;
using UnityEngine;
using static Satchel.IoUtils;
namespace CustomKnight
{
    internal static class DefaultSkin
    {
        internal static bool isGeneratingDefaultSkin = false;
        private static IEnumerator GenerateSkin()
        {
            yield return new WaitWhile(() => HeroController.instance == null || CharmIconList.Instance == null);
            SpriteLoader.PullDefaultTextures();
            CustomKnight.swapManager.SkinChangeSwap(SkinManager.CurrentSkin);
            CustomKnight.GlobalSettings.Preloads = true;
            foreach (var skinable in SkinManager.Skinables.Values)
            {
                skinable.DumpTexture();
            }
            CustomKnight.GlobalSettings.Preloads = false;
            isGeneratingDefaultSkin = false;
        }

        public static void SaveSkin()
        {
            isGeneratingDefaultSkin = true;
            CoroutineHelper.GetRunner().StartCoroutine(GenerateSkin());
        }

        public static void Save(this Texture2D texture,string path)
        {
            var folderPath = Path.Combine(SkinManager.SKINS_FOLDER, "GeneratedDefault");
            var itemPath = Path.Combine(folderPath, path);
            EnsureDirectory(Path.GetDirectoryName(itemPath));

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

    }
}
