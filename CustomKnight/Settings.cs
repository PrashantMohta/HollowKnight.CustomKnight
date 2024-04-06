
using Modding.Converters;
using Newtonsoft.Json;

namespace CustomKnight
{
    public class SaveModSettings
    {
        public string DefaultSkin { get; set; } = null;
    }

    public class GlobalModSettings
    {

        public string Version { get; set; } = "";
        public bool Preloads { get; set; } = true;
        public string DefaultSkin { get; set; } = "Default";
        public int NameLength { get; set; } = 15;
        public int MaxSkinCache { get; set; } = 10; // it can be 10 because in most cases people will use 1-2 skins in one session anyway.
        public bool showMovedText { get; set; } = true;
        public bool SwapperEnabled { get; set; } = true;

        public string[] saveSkins = new string[] { "Default", "Default", "Default", "Default" };

        public bool DumpOldSwaps { get; set; } = false;
        public bool EnablePauseMenu { get; set; } = true;
        public bool EnableSaveHuds { get; set; } = true;

        public bool GenerateDefaultSkin { get; set; } = true;
        public List<string> FavoriteSkins { get; set; } = new();
        public List<string> RecentSkins { get; set; } = new();

        [JsonConverter(typeof(PlayerActionSetConverter))]
        public KeyBinds Keybinds = new KeyBinds();
        public void AddRecentSkin(string skinId)
        {
            RecentSkins.RemoveAll((v) => v == skinId);
            RecentSkins.Insert(0, skinId);
            if (RecentSkins.Count > MaxSkinCache)
            {
                RecentSkins = RecentSkins.GetRange(0, MaxSkinCache);
            }
        }
    }

}