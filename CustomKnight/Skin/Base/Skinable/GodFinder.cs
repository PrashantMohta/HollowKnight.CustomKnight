
namespace CustomKnight
{
    public class GodFinder:Skinable_Sprite
    {
        public string Name;
        public GodFinder(string name) : base(name)
        {
            Name = name;
        }
        public override void SaveDefaultTexture()
        {
            GodfinderInvIcon invIcon= GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren("Godfinder").GetComponent<GodfinderInvIcon>();
            if (Name.EndsWith("0"))
            {
                ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(invIcon.normalSprite), invIcon.normalSprite.pixelsPerUnit);
            }
            if (Name.EndsWith("1"))
            {
                ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(invIcon.newBossSprite), invIcon.newBossSprite.pixelsPerUnit);
            }
            if (Name.EndsWith("2"))
            {
                ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(invIcon.allBossesSprite), invIcon.allBossesSprite.pixelsPerUnit);
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            GodfinderInvIcon invIcon = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Inv_Items").FindGameObjectInChildren("Godfinder").GetComponent<GodfinderInvIcon>();
            if (Name.EndsWith("0"))
            {
                invIcon.normalSprite=CreateSpritewithppu(sprite.texture,invIcon.normalSprite.pixelsPerUnit);
            }
            if (Name.EndsWith("1"))
            {
                invIcon.newBossSprite= CreateSpritewithppu(sprite.texture, invIcon.newBossSprite.pixelsPerUnit);
            }

        }
    }
}
