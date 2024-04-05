namespace CustomKnight
{
    public class Ore : Skinable_Sprite
    {
        public static string Name = "Inventory/Ore";
        public Ore() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Ore");
            ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Ore");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
