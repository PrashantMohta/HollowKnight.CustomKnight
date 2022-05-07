namespace CustomKnight
{
    public class RelicBG : Skinable_Sprite
    {
        public static string Name = "RelicBG";
        public RelicBG() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject bg = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("trinket_backboard");
            ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(bg.GetComponent<InvRelicBackboard>().activeSprite), bg.GetComponent<InvRelicBackboard>().activeSprite.pixelsPerUnit); ;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject bg = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("trinket_backboard");
            bg.GetComponent<InvRelicBackboard>().activeSprite = CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
        }
    }
}
