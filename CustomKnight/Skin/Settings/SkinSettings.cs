namespace CustomKnight
{
    /// <summary>
    /// User side skin settings that are updated by the mod
    /// </summary>
    public class SkinSettings
    {
        /// <summary>
        /// Alternates that are selected for each default filename
        /// </summary>
        public Dictionary<string, string> selectedAlternates = new Dictionary<string, string>();

        /// <summary>
        /// Ctor
        /// </summary>
        public SkinSettings()
        {
            foreach (var kvp in SkinManager.Skinables)
            {
                var name = kvp.Value.name + ".png";
                selectedAlternates[name] = name;
            }
        }
    }

}
