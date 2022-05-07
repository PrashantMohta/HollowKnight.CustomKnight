namespace CustomKnight
{
    public class ElegentKey : Skinable_Sprite
    {
        public static string Name = "ElegentKey";
        public ElegentKey() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("White Key");
            ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sd.GetComponent<SpriteRenderer>().sprite), sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("White Key");
            sd.GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
    }
}
