using static Satchel.FsmUtil;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    internal class Fluke : Skinable_Tk2d
    {
        public static string NAME = "Fluke";
        public Fluke() : base(NAME) { }
        public override Material GetMaterial()
        {
            GameObject hc = HeroController.instance.gameObject;
            GameObject charmEffects = hc.FindGameObjectInChildren("Charm Effects");
            PlayMakerFSM poolFlukes = charmEffects.LocateMyFSM("Pool Flukes");
            GameObject fluke = poolFlukes.GetAction<CreateGameObjectPool>("Pool Normal", 0).prefab.Value;
            var _flukeMat = fluke.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            return _flukeMat;
        }

    }
}