using CustomKnight;
public class LoveKey : Skinable_Sprite
{
    public static string Name = "LoveKey";
    public LoveKey() : base(Name) { }
    public override void SaveDefaultTexture()
    {
        GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Love Key");
        ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sd.GetComponent<SpriteRenderer>().sprite), sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit); ;
    }
    public override void ApplySprite(Sprite sprite)
    {
        GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Love Key");
        sd.GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
    }
}