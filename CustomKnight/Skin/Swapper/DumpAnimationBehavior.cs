using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomKnight.Skin.Swapper
{
    /// <summary>
    /// Class that allows dumping tricky animated sprites
    /// </summary>
    public class DumpAnimationBehavior : MonoBehaviour
    {

        private SpriteRenderer sr;

        /// <summary>
        /// Dictionary holding sprite name to Sprite
        /// </summary>
        public Dictionary<String, Sprite> sprites = new Dictionary<String, Sprite>();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void OnWillRenderObject()
        {
            if (sr == null)
            {
                sr = GetComponent<SpriteRenderer>();
            }
            if (!sprites.ContainsKey(sr.sprite.name)) {
                CustomKnight.Instance.Log($"[OnWillRenderObject] Animated object:{name}:sprite:{sr.sprite.name}");
                CustomKnight.dumpManager.detected += 1;
                var tex = SpriteUtils.ExtractTextureFromSprite(sr.sprite);// TextureUtils.duplicateTexture(sr.sprite.texture);
                CustomKnight.dumpManager.SaveTextureDump(gameObject.scene,Path.Combine("ck_spr_anim", sr.sprite.name), tex);
                CustomKnight.dumpManager.done += 1;
                sprites[sr.sprite.name] = sr.sprite;
                GameObject.Destroy(tex);
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


    }



}
