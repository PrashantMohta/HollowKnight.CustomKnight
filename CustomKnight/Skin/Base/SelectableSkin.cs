using System.IO;
using static Satchel.IoUtils;
namespace CustomKnight
{

    /// <summary>
    ///     The Interface that a Class must implement to be treated as a skin.
    /// </summary>
    public interface ISelectableSkin
    {
        /// <summary>
        ///  GetId
        /// </summary>
        /// <returns>The unique id of the skin as a <c>string</c></returns>
        public string GetId();

        /// <summary>
        ///  GetName
        /// </summary>
        /// <returns>The Name to be displayed in the menu as a <c>string</c></returns>
        public string GetName();

        /// <summary>
        ///  shouldCache
        /// </summary>
        /// <returns>A <c>bool</c> representing if the texture can be cached in memory or not.</returns>
        public bool shouldCache();

        /// <summary>
        ///  hasSwapper
        /// </summary>
        /// <returns>A <c>bool</c> representing if the skin has support for swapper files.</returns>
        public bool hasSwapper();

        /// <summary>
        ///  getSwapperPath
        /// </summary>
        /// <returns>A <c>string</c> representing the absolute path to the directory containing the swap folder.</returns>
        public string getSwapperPath();

        /// <summary>
        ///  Exists
        /// </summary>
        /// <param name="FileName">A <c>string</c> that identifies the file</param>
        /// <returns>A <c>bool</c> representing if the file exists in this skin.</returns>
        public bool Exists(string FileName);



        /// <summary>
        ///  GetTexture
        /// </summary>
        /// <param name="FileName">A <c>string</c> that identifies the texture file</param>
        /// <returns>A <c>Texture2D</c>, the texture for that filename.</returns>
        public Texture2D GetTexture(string FileName);

        /// <summary>
        ///  GetFile reads an arbitrary file from the skin
        /// </summary>
        /// <param name="FileName">A <c>string</c> that identifies the file</param>
        /// <returns>A <c>byte[]</c>, the contents of that filename.</returns>
        public byte[] GetFile(string FileName);


        /// <summary>
        ///  HasCinematic returns if a given ciematic exists
        /// </summary>
        /// <param name="CinematicName">A <c>string</c> that identifies the Cinematic</param>
        /// <returns>A <c>bool</c>, representing  if a given ciematic exists</returns>
        public bool HasCinematic(string CinematicName);

        /// <summary>
        ///  GetCinematicUrl returns the Url for the given Cinematic
        /// </summary>
        /// <param name="CinematicName">A <c>string</c> that identifies the Cinematic</param>
        /// <returns>A <c>string</c>, the Url for the given Cinematic (a file://  url for local files).</returns>
        public string GetCinematicUrl(string CinematicName);


    }

    public interface ISupportsOverrides
    {

        /// <summary>
        /// HasOverrides returns if a given file has overrides
        /// </summary>
        /// <param name="FileName">A <c>string</c> that identifies the file</param>
        /// <returns>A <c>bool</c> representing if the file exists in this skin.</returns>
        public bool HasOverrides(string FileName);

        /// <summary>
        /// GetAllOverrides returns the overrides if a given file has overrides
        /// </summary>
        /// <param name="FileName">A <c>string</c> that identifies the file</param>
        /// <returns>A string[] of overrides</returns>
        public string[] GetAllOverrides(string FileName);

        /// <summary>
        /// SetOverride sets a file as the selected override
        /// </summary>
        /// <param name="FileName">A <c>string</c> that identifies the file</param>
        /// <param name="AlternateFileName">A <c>string</c> that identifies the alternate file</param>
        public void SetOverride(string FileName, string AlternateFileName);

        /// <summary>
        /// GetOverride gets the currently selected override (or default)
        /// </summary>
        /// <param name="FileName">A <c>string</c> that identifies the file</param>
        /// <returns>A <c>string</c> that identifies the overridden file</returns>
        public string GetOverride(string FileName);
    }

    /// <summary>
    ///     The Class that represents all static skins that CustomKnight manages.
    /// </summary>
    public class StaticSkin : ISelectableSkin, ISupportsOverrides
    {
        public string SkinDirectory = "";

        public SkinConfig skinConfig;
        public SkinSettings skinSettings;
        internal static SettingsLoader<SkinConfig> skinConfigLoader = new SettingsLoader<SkinConfig>();
        internal static SettingsLoader<SkinSettings> skinSettingsLoader = new SettingsLoader<SkinSettings>();

        private void MigrateCharms()
        {
            var skinFolder = Path.Combine(SkinManager.SKINS_FOLDER, SkinDirectory);
            var charmsFolder = Path.Combine(skinFolder, "Charms");
            EnsureDirectory(charmsFolder);
            string[] files = Directory.GetFiles(skinFolder);
            foreach (string file in files)
            {
                if (!Path.GetFileName(file).StartsWith("Charm_"))
                {
                    continue;
                }
                try
                {
                    File.Move(file, Path.Combine(charmsFolder, Path.GetFileName(file)));
                }
                catch (Exception e)
                {
                    CustomKnight.Instance.LogError("A File could not be Copied : " + e.ToString());
                }
            }
        }

        private void DetectAlternates()
        {
            var skinFolder = Path.Combine(SkinManager.SKINS_FOLDER, SkinDirectory);
            var charmsFolder = Path.Combine(skinFolder, "Charms");
            var inventoryFolder = Path.Combine(skinFolder, "Inventory");
            EnsureDirectory(charmsFolder);
            EnsureDirectory(inventoryFolder);

            var altFound = false;

            string[] files = Directory.GetFiles(skinFolder);

            // base skin
            foreach (var kvp in SkinManager.Skinables)
            {
                var name = kvp.Value.name + ".png";
                var possibleAlts = Array.FindAll(files, (file) => Path.GetFileName(file).Contains(kvp.Value.name) && !skinConfig.alternates[name].Contains(Path.GetFileName(file)));
                foreach (var possibleAlt in possibleAlts)
                {
                    skinConfig.alternates[name].Add(Path.GetFileName(possibleAlt));
                }
            }
            // charms 
            files = Directory.GetFiles(charmsFolder);
            foreach (var kvp in SkinManager.Skinables)
            {
                if (!kvp.Value.name.StartsWith("Charms/"))
                {
                    continue;
                }
                var baseFileName = kvp.Value.name.Substring("Charms/".Length);
                var name = baseFileName + ".png";
                var possibleAlts = Array.FindAll(files, (file) => Path.GetFileName(file).Contains(baseFileName) && !skinConfig.alternates[kvp.Value.name + ".png"].Contains("Charms/" + Path.GetFileName(file)));
                foreach (var possibleAlt in possibleAlts)
                {
                    skinConfig.alternates[kvp.Value.name + ".png"].Add("Charms/" + Path.GetFileName(possibleAlt));
                }
            }
            // inventory
            files = Directory.GetFiles(inventoryFolder);
            foreach (var kvp in SkinManager.Skinables)
            {

                if (!kvp.Value.name.StartsWith("Inventory/"))
                {
                    continue;
                }
                var baseFileName = kvp.Value.name.Substring("Inventory/".Length);
                var name = baseFileName + ".png";
                var possibleAlts = Array.FindAll(files, (file) => Path.GetFileName(file).Contains(baseFileName) && !skinConfig.alternates[kvp.Value.name + ".png"].Contains("Inventory/" + Path.GetFileName(file)));
                foreach (var possibleAlt in possibleAlts)
                {
                    skinConfig.alternates[kvp.Value.name + ".png"].Add("Inventory/" + Path.GetFileName(possibleAlt));
                }
            }
            SaveSettings();
        }
        public StaticSkin(string DirectoryName)
        {
            SkinDirectory = DirectoryName;
            MigrateCharms();
            skinConfig = skinConfigLoader.Load(($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/skin-config.json").Replace("\\", "/"));
            skinSettings = skinSettingsLoader.Load(($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/skin-settings.json").Replace("\\", "/"));
            if (skinConfig.detectAlts)
            {
                skinConfig.detectAlts = false;
                DetectAlternates();
            }
        }

        public void SaveSettings()
        {
            skinConfigLoader.Save(($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/skin-config.json").Replace("\\", "/"), skinConfig);
            skinSettingsLoader.Save(($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/skin-settings.json").Replace("\\", "/"), skinSettings);
        }
        public bool shouldCache() => true;
        public string GetId() => SkinDirectory;
        public string GetName() => SkinDirectory;
        public bool hasSwapper() => true;
        public string getSwapperPath() => Path.Combine(SkinManager.SKINS_FOLDER, SkinDirectory);

        private Dictionary<string, string> CinematicFileUrlCache = new();
        public bool Exists(string FileName)
        {
            string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/{FileName}").Replace("\\", "/");
            return File.Exists(file);
        }
        public Texture2D GetTexture(string FileName)
        {
            Texture2D texture = null;
            try
            {
                string OverriddenFile = GetOverride(FileName);
                string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/{OverriddenFile}").Replace("\\", "/");
                byte[] texBytes = File.ReadAllBytes(file);
                texture = new Texture2D(2, 2);
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
                string OverriddenFile = GetOverride(FileName);
                string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/{OverriddenFile}").Replace("\\", "/");
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
                string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/Cinematics/{CinematicName}").Replace("\\", "/");
                CinematicFileUrlCache[CinematicName] = GetCinematicUrl(CinematicName);
                return CinematicFileUrlCache[CinematicName].Length > 0;
            }

        }

        public string GetCinematicUrl(string CinematicName)
        {
            string path = "";
            string file = ($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/Cinematics/{CinematicName}").Replace("\\", "/");
            if (File.Exists(file + ".webm"))
            {
                path = file + ".webm";
            }
            CustomKnight.Instance.LogFine("[GetCinematicUrl]" + CinematicName + ":" + path);
            return path;
        }

        public bool HasOverrides(string FileName)
        {
            CustomKnight.Instance.Log($" {FileName} : count");

            if (skinConfig.alternates != null && skinConfig.alternates.TryGetValue(FileName, out var overrides))
            {
                CustomKnight.Instance.Log($"{overrides.Count} : count");

                return overrides.Count > 1;
            }
            return false;
        }

        public string[] GetAllOverrides(string FileName)
        {
            if (skinConfig.alternates != null && skinConfig.alternates.TryGetValue(FileName, out var overrides))
            {
                return overrides.ToArray();
            }
            return new string[] { };
        }

        public void SetOverride(string FileName, string AlternateFileName)
        {
            if (skinSettings.selectedAlternates == null)
            {
                skinSettings.selectedAlternates = new Dictionary<string, string>();
            }
            skinSettings.selectedAlternates[FileName] = AlternateFileName;
            skinSettingsLoader.Save(($"{SkinManager.SKINS_FOLDER}/{SkinDirectory}/skin-settings.json").Replace("\\", "/"), skinSettings);
        }

        public string GetOverride(string FileName)
        {
            if (skinSettings.selectedAlternates != null && skinSettings.selectedAlternates.TryGetValue(FileName, out var alternate))
            {
                return alternate;
            }

            var overrides = GetAllOverrides(FileName);
            if (overrides.Length > 0)
            {
                return overrides[0];
            }

            return FileName;
        }
    }

}