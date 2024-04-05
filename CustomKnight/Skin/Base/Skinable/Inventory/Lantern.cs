namespace CustomKnight
{
    public class Lantern : Skinable_Sprite
    {
        public static string Name = "Inventory/Lantern";
        public Lantern() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject cw = SkinManager.equipment.FindGameObjectInChildren("Lantern");
            ckTex.defaultSprite = cw.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject cw = SkinManager.equipment.FindGameObjectInChildren("Lantern");
            cw.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
