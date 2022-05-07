namespace CustomKnight
{
    public class WJournal : Skinable_Sprite
    {
        public static string Name = "WJournal";
        public WJournal() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Trinket1");
            ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sd.GetComponent<SpriteRenderer>().sprite), sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Trinket1");
            sd.GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
        }
    }
}
