using static Satchel.FsmUtil;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    internal class Weaver : Skinable_Tk2d
    {
        public static string NAME = "Weaver";
        public Weaver() : base(NAME) { }
        public override Material GetMaterial()
        {
            GameObject hc = HeroController.instance.gameObject;
            GameObject charmEffects = hc.FindGameObjectInChildren("Charm Effects");
            PlayMakerFSM weaverlingControl = charmEffects.LocateMyFSM("Weaverling Control");
            GameObject weaver = weaverlingControl.GetAction<SpawnObjectFromGlobalPool>("Spawn", 0).gameObject.Value;
            var _weaverMat = weaver.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            return _weaverMat;
        }

    }
}