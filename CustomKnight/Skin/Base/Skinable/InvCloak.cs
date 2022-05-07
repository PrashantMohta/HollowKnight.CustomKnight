
namespace CustomKnight
{
    public class InvCloak:Skinable_Sprite
    {
        public static string BaseName = "Cloak";
        public int level;
        public InvCloak(int Level) : base(BaseName+$"_{Level}") {
            this.level = Level;
        }
        public override void SaveDefaultTexture()
        {
            GameObject eq = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment");
            PlayMakerFSM eqfsm = eq.LocateMyFSM("Build Equipment List");
           if(level==1&&!PlayerData.instance.hasShadowDash)//I hate this
            {
                ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(eq.FindGameObjectInChildren("Dash Cloak").GetComponent<SpriteRenderer>().sprite), eq.FindGameObjectInChildren("Dash Cloak").GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
            }
           if(level==2)
            {
               Sprite sprite= eqfsm.GetAction<SetSpriteRendererSprite>("Dash", 16).sprite.Value as Sprite;
                ckTex.defaultSprite = CreateSpritewithppu(SpriteUtils.ExtractTextureFromSpriteLegacy(sprite), sprite.pixelsPerUnit);
            }
        }
        public override void ApplySprite(Sprite sprite)
        {
            GameObject eq = GameCameras.instance.hudCamera.gameObject.FindGameObjectInChildren("Inventory").FindGameObjectInChildren("Inv").FindGameObjectInChildren("Equipment");
            PlayMakerFSM eqfsm = eq.LocateMyFSM("Build Equipment List");
            if (level == 1&&!PlayerData.instance.hasShadowDash)
            {
                eq.FindGameObjectInChildren("Dash Cloak").GetComponent<SpriteRenderer>().sprite=CreateSpritewithppu(sprite.texture,eq.FindGameObjectInChildren("Dash Cloak").GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
            }
            if (level == 2)
            {
                eqfsm.GetAction<SetSpriteRendererSprite>("Dash", 16).sprite.Value=CreateSpritewithppu(sprite.texture,ckTex.defaultSprite.pixelsPerUnit);
            }
        }
    }
}
