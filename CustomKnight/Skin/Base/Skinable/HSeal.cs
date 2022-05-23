namespace CustomKnight
{
    public class HSeal : Skinable_Sprite
    {
        public static string Name = "Inventory/HSeal";
        public HSeal() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Trinket2");
            ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Trinket2");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
