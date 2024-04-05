using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class Deathpt : Skinable_Tk2d
    {
        public static string NAME = "Deathpt";
        public Deathpt() : base(NAME) { }
        public override Material GetMaterial()
        {
            return HeroController.instance.heroDeathPrefab.FindGameObjectInChildren("Particle Wave").GetComponent<ParticleSystemRenderer>().material;
        }
    }
}
