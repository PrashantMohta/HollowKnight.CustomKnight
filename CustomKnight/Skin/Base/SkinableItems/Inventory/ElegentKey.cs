namespace CustomKnight
{
    internal class ElegentKey : Skinable_Sprite
    {
        public static string Name = "Inventory/ElegentKey";
        public ElegentKey() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("White Key");
            ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("White Key");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
