using Modding;

namespace CustomKnight
{
    public class SaveSettings : ModSettings { }

    public class GlobalSettings : ModSettings
    {
        public bool Preloads { get => GetBool(true); set => SetBool(value); }
        public string DefaultSkin { get => GetString("Default"); set => SetString(value); }
    }
}