using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    internal class Leak : Skinable_Tk2ds
    {
        public static string NAME = "Leak";
        public Leak() : base(NAME) { }
        public override List<Material> GetMaterials()
        {
            GameObject hc = HeroController.instance.gameObject;
            return new List<Material>{
                hc.FindGameObjectInChildren("Leak").GetComponent<ParticleSystemRenderer>().material,
                hc.FindGameObjectInChildren("Low Health Leak").GetComponent<ParticleSystemRenderer>().material
            };
        }

    }
}