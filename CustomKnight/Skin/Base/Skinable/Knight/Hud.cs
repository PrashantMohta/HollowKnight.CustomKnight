namespace CustomKnight
{
    public class Hud : Skinable_noCache
    {
        public static string NAME = "Hud";
        public Hud() : base(NAME) { }

        public override Material GetMaterial()
        {
            Material _hudMat = null;
            foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
            {
                if (i.name == "Health 1")
                {
                    _hudMat = i.GetCurrentSpriteDef().material;
                    break;
                }
            }
            return _hudMat;
        }

    }
}