using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    public class QOrbs : Skinable_Tk2d
    {
        public static string NAME = "QOrbs";
        public QOrbs() : base(QOrbs.NAME) { }
        public override Material GetMaterial()
        {
            return HeroController.instance.gameObject.FindGameObjectInChildren("Q Orbs").GetComponent<ParticleSystemRenderer>().material;
        }

    }
}