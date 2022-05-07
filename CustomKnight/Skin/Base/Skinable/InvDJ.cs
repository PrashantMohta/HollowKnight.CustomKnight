namespace CustomKnight
{
    public class InvDJ : Skinable_Sprite
    {
        public static string Name = "DJump";
        public InvDJ() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Double Jump");
            ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sd.GetComponent<SpriteRenderer>().sprite), sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Double Jump");
            sd.GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
    }
}
