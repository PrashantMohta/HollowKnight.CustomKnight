namespace CustomKnight
{
    internal class NailArtBG : Skinable_Sprite
    {
        public static string Name = "Inventory/ArtBG";
        public NailArtBG() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject bg = SkinManager.invitem.FindGameObjectInChildren("Art Backboard");
            ckTex.defaultSprite = bg.GetComponent<InvNailArtBackboard>().activeSprite;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject bg = SkinManager.invitem.FindGameObjectInChildren("Art Backboard");
            bg.GetComponent<InvNailArtBackboard>().activeSprite = sprite;
        }
    }
}
