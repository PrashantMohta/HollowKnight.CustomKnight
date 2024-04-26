using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    internal class Liquid : Skinable_Tk2d
    {
        public static string NAME = "Liquid";
        public Liquid() : base(NAME) { }
        public override Material GetMaterial()
        {
            return GameCameras.instance.hudCanvas.FindGameObjectInChildren("Liquid").GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
        }

    }
}