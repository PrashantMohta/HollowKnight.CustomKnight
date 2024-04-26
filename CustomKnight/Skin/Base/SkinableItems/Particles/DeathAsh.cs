namespace CustomKnight
{
    internal class DeathAsh : Skinable_Tk2ds
    {
        public static string NAME = "DeathAsh";
        public DeathAsh() : base(NAME) { }
        public override List<Material> GetMaterials()
        {
            GameObject hd = HeroController.instance.transform.Find("Hero Death").gameObject;
            var M = new List<Material>{
                hd.Find("Ash L").GetComponent<ParticleSystemRenderer>().material,
                hd.Find("Ash R").GetComponent<ParticleSystemRenderer>().material
            };
            return M;
        }

    }
}
