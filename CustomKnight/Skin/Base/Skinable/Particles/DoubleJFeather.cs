using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class DoubleJFeather : Skinable_Tk2d
    {
        public static string NAME = "DoubleJFeather";
        public DoubleJFeather() : base(NAME) { }
        public override Material GetMaterial()
        {
            return HeroController.instance.gameObject.FindGameObjectInChildren("Double J Feather").GetComponent<ParticleSystemRenderer>().material;
        }

    }
}