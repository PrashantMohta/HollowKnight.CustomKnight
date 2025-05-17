using System.IO;
using CustomKnight.Next.Migrations;
using CustomKnight.Next.Skin;
using CustomKnight.Next.Skin.Enum;
using static Satchel.IoUtils;
namespace CustomKnight
{
    /// <summary>
    ///     The Class that represents all static skins that CustomKnight manages.
    /// </summary>
    internal class StaticSkin : ISkin, ISelectableSkin, ISupportsConfig
    {
        internal string SkinDirectory = "";
        private SkinConfig skinConfig;
        private SkinSettings skinSettings;
        private static SettingsLoader<SkinConfig> skinConfigLoader = new SettingsLoader<SkinConfig>();
        private static SettingsLoader<SkinSettings> skinSettingsLoader = new SettingsLoader<SkinSettings>();
        private string GetPath() => Path.Combine(SkinManager.SKINS_FOLDER, SkinDirectory);
        private List<Feature> features = new List<Feature>(); //todo populate
        private List<ItemGroup> items = new List<ItemGroup>(); //todo populate
        public StaticSkin(string DirectoryName)
        {
            SkinDirectory = DirectoryName;
            MigrationManager.RunMigrations(new MigrationContext { SkinName = GetName(), SkinPath = GetPath() });
            skinConfig = skinConfigLoader.Load(Path.Combine(GetPath(), "config.json"));
            skinSettings = skinSettingsLoader.Load(Path.Combine(GetPath(), "settings.json"));
        }

        public Feature[] GetFeatures() => [.. features];
        public string GetId() => SkinDirectory;
        public string GetName() => SkinDirectory;
        public bool ShouldCache() => true;
        public SkinConfig GetConfig() => skinConfig;
        public SkinSettings GetSettings() => skinSettings;
        public ItemGroup[] GetGroups() => [.. items];

        public Texture2D GetIcon()
        {
            throw new NotImplementedException();
        }

        // might yeet all of these afterwards
        public bool shouldCache() => ShouldCache();
        public bool hasSwapper() => true;
        public string getSwapperPath() => GetPath();


        private Dictionary<string, string> CinematicFileUrlCache = new();
        public bool Exists(string FileName)
        {
            string file = Path.Combine(GetPath(), FileName);
            return File.Exists(file);
        }
        public Texture2D GetTexture(string FileName)
        {
            Texture2D texture = null;
            try
            {
                string file = Path.Combine(GetPath(), FileName);
                byte[] texBytes = File.ReadAllBytes(file);
                texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                texture.LoadImage(texBytes);
            }
            catch (Exception e)
            {
                CustomKnight.Instance.Log(e.ToString());
            }
            return texture;
        }
        public byte[] GetFile(string FileName)
        {
            byte[] data = null;
            try
            {
                string file = Path.Combine(GetPath(), FileName);
                data = File.ReadAllBytes(file);
            }
            catch (Exception e)
            {
                CustomKnight.Instance.Log(e.ToString());
            }
            return data;
        }

        public bool HasCinematic(string CinematicName)
        {
            if (CinematicFileUrlCache.TryGetValue(CinematicName, out var url))
            {
                return url.Length > 0;
            }
            else
            {
                EnsureDirectory($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/Cinematics/");
                string file = $"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/Cinematics/{CinematicName}".Replace("\\", "/");
                CinematicFileUrlCache[CinematicName] = GetCinematicUrl(CinematicName);
                return CinematicFileUrlCache[CinematicName].Length > 0;
            }

        }

        public string GetCinematicUrl(string CinematicName)
        {
            string path = "";
            string file = $"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/Cinematics/{CinematicName}".Replace("\\", "/");
            if (File.Exists(file + ".webm"))
            {
                path = file + ".webm";
            }
            CustomKnight.Instance.LogFine("[GetCinematicUrl]" + CinematicName + ":" + path);
            return path;
        }

    }

}