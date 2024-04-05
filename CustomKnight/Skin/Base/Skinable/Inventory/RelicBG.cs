namespace CustomKnight
{
    public class RelicBG : Skinable_Sprite
    {
        public static string Name = "Inventory/RelicBG";
        public RelicBG() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject bg = SkinManager.inv.FindGameObjectInChildren("trinket_backboard");
            ckTex.defaultSprite = bg.GetComponent<InvRelicBackboard>().activeSprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject bg = SkinManager.inv.FindGameObjectInChildren("trinket_backboard");
            bg.GetComponent<InvRelicBackboard>().activeSprite = sprite;
        }
    }
}
