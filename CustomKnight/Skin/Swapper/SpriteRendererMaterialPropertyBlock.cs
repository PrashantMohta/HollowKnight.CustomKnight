namespace CustomKnight
{
    /// <summary>
    /// Component that forcibly sets the MaterialPropertyBlock of the SpriteRenderer to a defined value every Update and LateUpdate
    /// </summary>
    public class SpriteRendererMaterialPropertyBlock : MonoBehaviour
    {
        /// <summary>
        /// The MaterialPropertyBlock to set
        /// </summary>
        public MaterialPropertyBlock mpb;

        /// <summary>
        /// The SpriteRenderer to set the block to
        /// </summary>
        public SpriteRenderer sr;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Update()
        {
            if (sr == null)
            {
                sr = GetComponent<SpriteRenderer>();
            }
            if (mpb != null)
            {
                sr.SetPropertyBlock(mpb);
            }
        }

        public void LateUpdate()
        {
            if (sr == null)
            {
                sr = GetComponent<SpriteRenderer>();
            }
            if (mpb != null)
            {
                sr.SetPropertyBlock(mpb);
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}