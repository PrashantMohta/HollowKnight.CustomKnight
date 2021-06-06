using Modding;

namespace CustomKnight
{
    public class SaveSettings : ModSettings { }

    public class GlobalModSettings : ModSettings
    {
        public bool Preloads { get => GetBool(true); set => SetBool(value); }
        public string DefaultSkin { get => GetString("Default"); set => SetString(value); }
        public float PanelStartPos { get => GetFloat(80.0f); set => SetFloat(value);}
        public int NameLength { get => GetInt(10); set => SetInt(value); }
        public int OptionSize { get => GetInt(35); set => SetInt(value); }

    }

}