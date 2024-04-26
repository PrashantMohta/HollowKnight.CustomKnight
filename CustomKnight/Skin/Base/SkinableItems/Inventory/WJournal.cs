namespace CustomKnight
{
    internal class WJournal : Skinable_Sprite
    {
        public static string Name = "Inventory/WJournal";
        public WJournal() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Trinket1");
            ckTex.defaultSprite = sd.GetComponent<SpriteRenderer>().sprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = SkinManager.equipment.FindGameObjectInChildren("Trinket1");
            sd.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
