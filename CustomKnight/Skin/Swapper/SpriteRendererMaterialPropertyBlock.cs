using UnityEngine;

namespace CustomKnight
{
    /// <summary>
    /// Component that forcibly sets the MaterialPropertyBlock of the SpriteRenderer to a defined value every Update and LateUpdate
    /// </summary>
    public class SpriteRendererMaterialPropertyBlock : MonoBehaviour
    {

        private MaterialPropertyBlock defaultPropertyBlock;

        private SpriteRenderer sr;

        private Dictionary<String, MaterialPropertyBlock> mpbs = new Dictionary<String, MaterialPropertyBlock>();

        private bool hasCustomDefault;

        private void UpdateMaterialPropertyBlock()
        {
            
            if (sr == null)
            {
                sr = GetComponent<SpriteRenderer>();
            }
            if (defaultPropertyBlock == null)
            {
                defaultPropertyBlock = new MaterialPropertyBlock();
                sr.GetPropertyBlock(defaultPropertyBlock);
                hasCustomDefault = false;
            }
            if (mpbs.TryGetValue(sr.sprite.name, out var mpb))
            {
                sr.SetPropertyBlock(mpb);
            }
            else
            {
                var tex = CustomKnight.swapManager.GetTexture2D(gameObject.scene, sr.sprite.name);
                if (tex != null)
                {
                    AddSprite(sr.sprite.name, tex);
                    sr.SetPropertyBlock(mpbs[sr.sprite.name]);
                }
                else
                {

                    if (!hasCustomDefault)
                    {
                        CustomKnight.Instance.Log($"[UpdateMaterialPropertyBlock] Unknown Animated object:{name}:sprite:{sr.sprite.name}");
                    }
                    sr.SetPropertyBlock(defaultPropertyBlock);
                }
            }
        }

        /// <summary>
        /// Add a texture to this SpriteRenderer's default sprites
        /// </summary>
        /// <param name="tex"></param>
        public void SetDefault(Texture2D tex)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
#pragma warning disable CS0618 // Type or member is obsolete
            block.AddTexture("_MainTex", tex);
#pragma warning restore CS0618 // Type or member is obsolete
            defaultPropertyBlock = block;
            hasCustomDefault = true;
        }

        /// <summary>
        /// Add a texture to this SpriteRenderer's possible sprites
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tex"></param>
        public void AddSprite(string name, Texture2D tex)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
#pragma warning disable CS0618 // Type or member is obsolete
            block.AddTexture("_MainTex", tex);
#pragma warning restore CS0618 // Type or member is obsolete
            mpbs[name] = block;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void OnWillRenderObject()
        {
            UpdateMaterialPropertyBlock();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}