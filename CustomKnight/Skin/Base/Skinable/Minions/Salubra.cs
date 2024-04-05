namespace CustomKnight
{
    internal class Salubra : Skinable
    {
        public static string NAME = "Salubra";
        public Salubra() : base(NAME) { }

        public override void ApplyTexture(Texture2D tex)
        {
            var go = HeroController.instance.gameObject.FindGameObjectInChildren("Blessing_Ghost");
            if (go == null) { return; }
            var behaviour = go.GetAddComponent<SpriteRendererMaterialPropertyBlock>();
            if (tex != null)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
#pragma warning disable CS0618 // Type or member is obsolete
                block.AddTexture("_MainTex", tex);
#pragma warning restore CS0618 // Type or member is obsolete
                behaviour.mpb = block;
                behaviour.enabled = true;
            }
            else
            {
                behaviour.enabled = false;
            }
        }

        public override void SaveDefaultTexture()
        {
            var go = HeroController.instance.gameObject.FindGameObjectInChildren("Blessing_Ghost");
            if (go == null) { return; }
            var sr = go.GetComponent<SpriteRenderer>();
            if (sr != null) { return; }
            ckTex.defaultTex = sr.sprite.texture;
        }
    }
}
