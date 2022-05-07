using CustomKnight;
public class Brand : Skinable_Sprite
{
    public static string Name = "Brand";
    public Brand() : base(Name) { }
    public override void SaveDefaultTexture()
    {
        GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Kings Brand");
        ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sd.GetComponent<SpriteRenderer>().sprite), sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
    }
    public override void ApplySprite(Sprite sprite)
    {
        GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Kings Brand");
        sd.GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
    }
}
