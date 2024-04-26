using static Satchel.FsmUtil;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    internal class Grimm : Skinable_Tk2d
    {
        public static string NAME = "Grimm";
        public Grimm() : base(NAME) { }
        public override Material GetMaterial()
        {
            GameObject hc = HeroController.instance.gameObject;
            GameObject charmEffects = hc.FindGameObjectInChildren("Charm Effects");
            PlayMakerFSM spawnGrimmchild = charmEffects.LocateMyFSM("Spawn Grimmchild");
            GameObject grimm = spawnGrimmchild.GetAction<SpawnObjectFromGlobalPool>("Spawn", 2).gameObject.Value;
            var _grimmMat = grimm.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            return _grimmMat;
        }

    }
}