namespace CustomKnight
{

    /// <summary>
    /// Interface that must be implemented if the skin supports configs and settings
    /// </summary>
    public interface ISupportsConfig
    {
        /// <summary>
        /// Method that must return the SkinConfig
        /// </summary>
        /// <returns>SkinConfig</returns>
        public SkinConfig GetConfig();

        /// <summary>
        /// Method that must return the SkinSettings
        /// </summary>
        /// <returns>SkinSettings</returns>
        public SkinSettings GetSettings();
    }
}
