using Newtonsoft.Json;

namespace CustomKnight
{
    // user side
    public class SkinSettings
    {
        public Dictionary<string, string> selectedAlternates = new Dictionary<string, string>();

        public SkinSettings()
        {
            foreach (var kvp in SkinManager.Skinables)
            {
                var name = kvp.Value.name + ".png";
                selectedAlternates[name] = name;
            }
        }
    }

    // author side
    public class SkinConfig
    {
        public bool dungFilter = true;
        public bool wraithsFilter = false;

        [JsonConverter(typeof(ColorHandler))]
        public Color brummColor = new Color(1, 1, 1, 1);

        [JsonConverter(typeof(ColorHandler))]
        public Color flashColor = new Color(1, 1, 1);

        [JsonConverter(typeof(ColorHandler))]
        public Color dungFlash = new Color(0.45f, 0.27f, 0f);

        public bool detectAlts = true;
        public Dictionary<string, List<string>> alternates = new Dictionary<string, List<string>>();
        public SkinConfig()
        {
            foreach (var kvp in SkinManager.Skinables)
            {
                var name = kvp.Value.name + ".png";
                alternates[name] = new List<string> { name };
            }
        }
    }

}
