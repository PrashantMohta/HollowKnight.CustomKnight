namespace CustomKnight
{

    /// <summary>
    /// Interface that must be implemented for your skin to have support for overrides(alternate sheets)
    /// </summary>
    [Obsolete("Wont work in CK4.0+")]
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

}
