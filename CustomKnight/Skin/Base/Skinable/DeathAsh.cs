namespace CustomKnight
{
    public class DeathAsh : Skinable_Tk2ds
    {
        public static string NAME = "DeathAsh";
        public DeathAsh() : base(DeathAsh.NAME) { }
        public override List<Material> GetMaterials()
        {
            GameObject hd = HeroController.instance.transform.Find("Hero Death").gameObject;
            var M = new List<Material>{
                hd.Find("Ash L").GetComponent<ParticleSystemRenderer>().material,
                hd.Find("Ash R").GetComponent<ParticleSystemRenderer>().material
            };
            //CustomKnight.dumpManager.SaveTextureByPath("Debug", "DeathAsh", (Texture2D)M[0].mainTexture);
            return M;
        }

    }
}
