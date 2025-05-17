
using Modding.Converters;
using Newtonsoft.Json;

namespace CustomKnight
{
    /// <summary>
    /// Per save settings
    /// </summary>
    public class SaveModSettings
    {
        /// <summary>
        /// Selected Skin
        /// </summary>
        public string DefaultSkin { get; set; } = null;
    }

    /// <summary>
    /// Overall CK settings
    /// </summary>
    public class GlobalModSettings
    {
        /// <summary>
        /// Version on which the settings file was created
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// Enables or disables Preloading gameObjects
        /// </summary>
        public bool Preloads { get; set; } = true;

        /// <summary>
        /// Currently Selected Skin
        /// </summary>
        public string DefaultSkin { get; set; } = SkinManager.DEFAULT_SKIN;

        /// <summary>
        /// Max Length of the Skin Names that is displayed in UI
        /// </summary>
        public int NameLength { get; set; } = 15;

        /// <summary>
        /// Total Number of skins to keep in memory per session
        /// </summary>
        public int MaxSkinCache { get; set; } = 10; // it can be 10 because in most cases people will use 1-2 skins in one session anyway.

        /// <summary>
        /// Enables Swappe
        /// </summary>
        public bool SwapperEnabled { get; set; } = true;


        /// <summary>
        /// Skins for each of the save slots
        /// </summary>
        [JsonProperty]
        private Dictionary<int, string> saveSkins = new Dictionary<int, string>() { { 0, SkinManager.DEFAULT_SKIN }, { 1, SkinManager.DEFAULT_SKIN }, { 2, SkinManager.DEFAULT_SKIN }, { 3, SkinManager.DEFAULT_SKIN } };


        /// <summary>
        /// Option to enable the new Pause Menu
        /// </summary>
        public bool EnablePauseMenu { get; set; } = true;

        /// <summary>
        /// Option to enable the Save Selection screen to be skinned
        /// </summary>
        public bool EnableSaveHuds { get; set; } = true;

        /// <summary>
        /// Option to enable swapping particles (only active when swap is enabled)
        /// </summary>
        public bool EnableParticleSwap { get; set; } = false;

        /// <summary>
        /// Used to indicate to generate default skin on next restart
        /// </summary>
        public bool GenerateDefaultSkin { get; set; } = true;

        /// <summary>
        /// Favorite skins are kept at the top of the skin list
        /// </summary>
        public List<string> FavoriteSkins { get; set; } = new();

        /// <summary>
        /// Recent skins are kept after the favorite skins
        /// </summary>
        public List<string> RecentSkins { get; set; } = new();

        /// <summary>
        /// Saves the keybinds
        /// </summary>
        [JsonConverter(typeof(PlayerActionSetConverter))]
        public KeyBinds Keybinds = new();
        internal void AddRecentSkin(string skinId)
        {
            RecentSkins.RemoveAll((v) => v == skinId);
            RecentSkins.Insert(0, skinId);
            if (RecentSkins.Count > MaxSkinCache)
            {
                RecentSkins = RecentSkins.GetRange(0, MaxSkinCache);
            }
        }

        internal static string GetSkinForProfileID(int id)
        {
            if (CustomKnight.GlobalSettings.saveSkins.TryGetValue(id, out var skinName))
            {
                return skinName;
            }
            SetSkinForProfileID(id, SkinManager.DEFAULT_SKIN);
            return SkinManager.DEFAULT_SKIN;
        }


        internal static void SetSkinForProfileID(int id, string skinName)
        {
            CustomKnight.GlobalSettings.saveSkins[GameManager.instance.profileID - 1] = skinName;
        }

        internal static void ResetProfileSkins()
        {
            CustomKnight.GlobalSettings.saveSkins = new GlobalModSettings().saveSkins;
        }
    }

}