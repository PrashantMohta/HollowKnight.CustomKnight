namespace CustomKnight
{
    public class Tear : Skinable_Sprite
    {
        public static string Name = "Inventory/Tear";
        public Tear() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Acid Armour");
            ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Acid Armour");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}