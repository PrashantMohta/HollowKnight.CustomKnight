using CustomKnight.Next.Skin.Enum;

namespace CustomKnight.Next.Skin
{
    public interface ISkin
    {
        /// <summary>
        /// List of features supported by the skin
        /// </summary>
        public abstract Feature[] GetFeatures();

        /// <summary>
        ///  GetId
        /// </summary>
        /// <returns>The unique id of the skin as a <c>string</c></returns>
        public abstract string GetId();

        /// <summary>
        ///  GetName
        /// </summary>
        /// <returns>The Name to be displayed in the menu as a <c>string</c></returns>
        public abstract string GetName();

        /// <summary>
        ///  shouldCache
        /// </summary>
        /// <returns>A <c>bool</c> representing if the texture can be cached in memory or not.</returns>
        public abstract bool ShouldCache();

        /// <summary>
        ///     GetGroups
        /// </summary>
        /// <returns>A <c>ItemGroup</c>> representing all the item groups in the skin</returns>
        public abstract ItemGroup[] GetGroups();
        Texture2D GetIcon();
    }
}
