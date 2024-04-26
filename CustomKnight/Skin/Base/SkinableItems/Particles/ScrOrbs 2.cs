using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    internal class ScrOrbs2 : Skinable_Tk2d
    {
        public static string NAME = "ScrOrbs2";
        public ScrOrbs2() : base(NAME) { }
        public override Material GetMaterial()
        {
            return HeroController.instance.gameObject.FindGameObjectInChildren("Scr Orbs 2").GetComponent<ParticleSystemRenderer>().material;
        }

    }
}