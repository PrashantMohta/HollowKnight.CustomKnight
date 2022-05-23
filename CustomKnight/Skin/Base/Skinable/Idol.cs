namespace CustomKnight
{
    public class Idol : Skinable_Sprite
    {
        public static string Name = "Inventory/Idol";
        public Idol() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Trinket3");
            ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sd.GetComponent<SpriteRenderer>().sprite), sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit); ;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd =SkinManager.equipment.FindGameObjectInChildren("Trinket3");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
