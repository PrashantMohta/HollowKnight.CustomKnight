namespace CustomKnight
{
    public class BEgg : Skinable_Sprite
    {
        public static string Name = "Inventory/BlackEgg";
        public BEgg() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Trinket4");
            
            ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Trinket4");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
