using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    internal class ScrOrbs : Skinable_Tk2d
    {
        public static string NAME = "ScrOrbs";
        public ScrOrbs() : base(NAME) { }
        public override Material GetMaterial()
        {
            return HeroController.instance.gameObject.FindGameObjectInChildren("Scr Orbs").GetComponent<ParticleSystemRenderer>().material;
        }

    }
}