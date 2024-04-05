namespace CustomKnight
{
    internal class Salubra : Skinable
    {
        public static string NAME = "Salubra";
        public Salubra() : base(NAME) { }
        public void apply()
        {
        }

        public override void ApplyTexture(Texture2D tex)
        {
            var GO = HeroController.instance.gameObject.FindGameObjectInChildren("Blessing_Ghost");
            if (GO == null) { return; }
            var behaviour = GO.GetAddComponent<SpriteRendererMaterialPropertyBlock>();
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

        public override void SaveDefaultTexture() { }
    }
}
