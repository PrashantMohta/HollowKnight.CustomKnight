
namespace CustomKnight
{
    public class InvClaw : Skinable_Sprite
    {
        public static string Name = "Claw";
        public InvClaw() : base(Name) { }
        public override void SaveDefaultTexture()
        {
            GameObject cw = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Mantis Claw");
            ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(cw.GetComponent<SpriteRenderer>().sprite), cw.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit); ;
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject cw = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment").FindGameObjectInChildren("Mantis Claw");
            cw.GetComponent<SpriteRenderer>().sprite = CreateSpritewithppu(sprite.texture, cw.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
        }
    }
}
