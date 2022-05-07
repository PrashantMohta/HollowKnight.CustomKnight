namespace CustomKnight
{
    public class Idol : Skinable_Sprite
    {
        public static string Name = "Idol";
        public Idol() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Trinket3");
            ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sd.GetComponent<SpriteRenderer>().sprite), sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit); ;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Trinket3");
            sd.GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
    }
}
