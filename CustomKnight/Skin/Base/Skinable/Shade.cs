namespace CustomKnight
{
    public class Shade : Skinable_Tk2d
    {
        public static string NAME = "Shade";
        public Shade() : base(Shade.NAME){}
        public override Material GetMaterial(){
            SceneManager sm = GameManager.instance.GetSceneManager().GetComponent<SceneManager>();
            tk2dSpriteAnimator shadeAnim = sm.hollowShadeObject.GetComponent<tk2dSpriteAnimator>();
            return shadeAnim.GetClipByName("Idle").frames[0].spriteCollection.spriteDefinitions[0].material;
        }

    }
}