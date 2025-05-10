using Newtonsoft.Json;
using Satchel.JsonConverters;

namespace CustomKnight
{

    /// <summary>
    /// Author side configuration for a skin
    /// </summary>
    public class SkinConfig
    {
        /// <summary>
        /// Should enable the filter over defender's crest effect
        /// </summary>
        public bool dungFilter = true;

        /// <summary>
        /// Should disable the filter applied on the wraiths sheet
        /// </summary>
        public bool wraithsFilter = false;

        /// <summary>
        /// Color that flashes when Melody is triggered
        /// </summary>
        [JsonConverter(typeof(ColorConverter))]
        public Color brummColor = new Color(1, 1, 1, 1);

        /// <summary>
        /// Color that flashes when the player heals
        /// </summary>
        [JsonConverter(typeof(ColorConverter))]
        public Color flashColor = new Color(1, 1, 1);

        /// <summary>
        /// Color that flashes when an enemy is under the effect of defender's crest
        /// </summary>
        [JsonConverter(typeof(ColorConverter))]
        public Color dungFlash = new Color(0.45f, 0.27f, 0f);

        /// <summary>
        /// Should the mod try to auto-detect Alts? should be disabled on authored skins
        /// </summary>
        [Obsolete("Wont work in CK4.0+")]
        public bool detectAlts = true;

        /// <summary>
        /// default filename to List of available alternate filenames
        /// </summary>
        [Obsolete("Wont work in CK4.0+")]
        public Dictionary<string, List<string>> alternates = new Dictionary<string, List<string>>();

        /// <summary>
        /// Ctor
        /// </summary>
        public SkinConfig()
        {
        }
    }

}
