namespace CustomKnight
{
    internal class InvSD : Skinable_Sprite
    {
        public static string Name = "Inventory/CRHeart";
        public InvSD() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Super Dash");
            ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Super Dash");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
