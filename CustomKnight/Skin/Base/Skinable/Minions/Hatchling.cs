using static Satchel.FsmUtil;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class Hatchling : Skinable_Tk2d
    {
        public static string NAME = "Hatchling";
        public Hatchling() : base(NAME) { }
        public override Material GetMaterial()
        {
            GameObject hc = HeroController.instance.gameObject;
            GameObject charmEffects = hc.FindGameObjectInChildren("Charm Effects");

            PlayMakerFSM hatchlingSpawn = charmEffects.LocateMyFSM("Hatchling Spawn");
            GameObject hatchling = hatchlingSpawn.GetFirstActionOfType<SpawnObjectFromGlobalPool>("Hatch").gameObject.Value;
            var _wombMat = hatchling.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            return _wombMat;
        }

    }
}