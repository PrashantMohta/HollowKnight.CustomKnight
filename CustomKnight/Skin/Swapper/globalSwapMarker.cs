namespace CustomKnight
{
    /// <summary>
    /// Component to allow swapper to detect a GameObject as if it is on another path (this is a hack and will only work for global directory)
    /// </summary>
    public class GlobalSwapMarker : MonoBehaviour
    {
        /// <summary>
        /// Original path of the game object
        /// </summary>
        public string originalPath = "";

        /// <summary>
        /// Opt out this object from skinning, will only be skinned once the actual object this is a clone of is encountered
        /// </summary>
        public bool optOut = false;
    }
}
