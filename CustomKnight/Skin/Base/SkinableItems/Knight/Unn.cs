namespace CustomKnight
{
    internal class Unn : Skinable_Tk2d
    {
        public static string NAME = "Unn";
        public Unn() : base(NAME) { }
        public override Material GetMaterial()
        {
            return HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>().GetClipByName("Slug Up").frames[0].spriteCollection.spriteDefinitions[0].material;
        }

    }
}