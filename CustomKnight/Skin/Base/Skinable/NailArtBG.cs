
namespace CustomKnight
{
    public class NailArtBG:Skinable_Sprite
    {
        public static string Name = "ArtBG";
        public NailArtBG() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject bg = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren("Art Backboard");
            ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(bg.GetComponent<InvNailArtBackboard>().activeSprite), bg.GetComponent<InvNailArtBackboard>().activeSprite.pixelsPerUnit);
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject bg = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren("Art Backboard");
            bg.GetComponent<InvNailArtBackboard>().activeSprite = CreateSpritewithppu(sprite.texture, ckTex.defaultSprite.pixelsPerUnit);
        }
    }
}
