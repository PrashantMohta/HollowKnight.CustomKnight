namespace CustomKnight
{
    internal class InvDJ : Skinable_Sprite
    {
        public static string Name = "Inventory/DJump";
        public InvDJ() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Double Jump");
            ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Double Jump");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
