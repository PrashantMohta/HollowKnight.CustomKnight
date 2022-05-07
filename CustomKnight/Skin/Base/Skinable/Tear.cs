namespace CustomKnight
{
    public class Tear : Skinable_Sprite
{
    public static string Name = "Tear";
    public Tear() : base(Name) { }
    public override void SaveDefaultTexture()
    {
        GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Acid Armour");
        ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sd.GetComponent<SpriteRenderer>().sprite), sd.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
    public override void ApplySprite(Sprite sprite)
    {
        GameObject sd = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Acid Armour");
        sd.GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
    }
}
}