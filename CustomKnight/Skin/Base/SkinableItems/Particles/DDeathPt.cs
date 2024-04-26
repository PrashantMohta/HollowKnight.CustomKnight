using static Satchel.GameObjectUtils;
namespace CustomKnight
{
    internal class DDeathpt : Skinable_Tk2d
    {
        public static string NAME = "DDeathpt";
        public DDeathpt() : base(NAME) { }
        public override Material GetMaterial()
        {
            return HeroController.instance.heroDeathPrefab.FindGameObjectInChildren("Dream Burst Pt").GetComponent<ParticleSystemRenderer>().material;
        }
    }
}
