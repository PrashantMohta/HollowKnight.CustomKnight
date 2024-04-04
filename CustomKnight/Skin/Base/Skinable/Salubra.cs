namespace CustomKnight
{
    internal class Salubra : Skinable
    {
        public static string NAME = "Salubra";
        public Salubra() : base(Salubra.NAME) { }
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
                block.AddTexture("_MainTex", tex);
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
