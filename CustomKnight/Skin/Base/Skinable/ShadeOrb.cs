using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class ShadeOrb : Skinable_Tk2ds
    {
        public static string NAME = "ShadeOrb";
        public ShadeOrb() : base(ShadeOrb.NAME) { }
        public override List<Material> GetMaterials()
        {
            SceneManager sm = GameManager.instance.GetSceneManager().GetComponent<SceneManager>();
            return new List<Material>{
                sm.hollowShadeObject.FindGameObjectInChildren("Shade Particles").GetComponent<ParticleSystemRenderer>().material,
                sm.hollowShadeObject.FindGameObjectInChildren("Reform Particles").GetComponent<ParticleSystemRenderer>().material,
                sm.hollowShadeObject.FindGameObjectInChildren("Retreat Particles").GetComponent<ParticleSystemRenderer>().material,
                sm.hollowShadeObject.FindGameObjectInChildren("Charge Particles").GetComponent<ParticleSystemRenderer>().material,
                sm.hollowShadeObject.FindGameObjectInChildren("Depart Particles").GetComponent<ParticleSystemRenderer>().material,
                sm.hollowShadeObject.FindGameObjectInChildren("Quake Particles").GetComponent<ParticleSystemRenderer>().material
            };
        }

    }
}