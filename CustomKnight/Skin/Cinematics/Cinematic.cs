
using UnityEngine.Video;

namespace CustomKnight
{
    /// <summary>
    /// Class that defines a replacable Cinamatic
    /// </summary>
    public class Cinematic
    {
        /// <summary>
        /// Name of the current Cinematic
        /// </summary>
        public string ClipName { get; private set; }
        internal VideoClip OriginalVideo = null;
        internal XB1CinematicVideoPlayer player = null;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="ClipName"></param>
        public Cinematic(string ClipName)
        {
            this.ClipName = ClipName;
        }

    }
}