namespace CustomKnight
{
    public class InvSD : Skinable_Sprite
    {
        public static string Name = "CRHeart";
        public InvSD() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Super Dash");
            ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sd.GetComponent<SpriteRenderer>().sprite), sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Super Dash");
            sd.GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
        }
    }
}
