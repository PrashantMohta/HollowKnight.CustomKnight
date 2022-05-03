namespace CustomKnight
{
    public class VS : Skinable_noCache
    {
        public static string NAME = "VS";
        public VS() : base(VS.NAME){}

        public override Material GetMaterial(){
            Material VS = null ;
            foreach (Transform child in HeroController.instance.gameObject.transform)
            {
                if (child.name == "Focus Effects")
                {
                    foreach (Transform focusChild in child)
                    {
                        if (focusChild.name == "Heal Anim")
                        {
                            VS = focusChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                            break;
                        }
                    }
                }
            }
            return VS;
        }

    }
}