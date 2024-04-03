namespace CustomKnight
{
    public class SpriteRendererMaterialPropertyBlock : MonoBehaviour {
        public MaterialPropertyBlock mpb;
        public SpriteRenderer sr;
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
    }
}