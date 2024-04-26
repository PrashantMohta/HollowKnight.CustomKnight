using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    internal class ShadowDashBlobs : Skinable_Tk2d
    {
        public static string NAME = "ShadowDashBlobs";
        public ShadowDashBlobs() : base(NAME) { }
        public override Material GetMaterial()
        {
            return HeroController.instance.gameObject.FindGameObjectInChildren("Shadow Dash Blobs").GetComponent<ParticleSystemRenderer>().material;
        }

    }
}