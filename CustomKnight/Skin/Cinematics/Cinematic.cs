
using UnityEngine.Video;

namespace CustomKnight{
    public class Cinematic {
        public string ClipName;
        internal VideoClip OriginalVideo = null;

        public Cinematic(string ClipName){
            this.ClipName = ClipName;
        }

    }
}