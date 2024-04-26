namespace CustomKnight
{
    internal class FlowerBreak : Skinable_Tk2ds
    {
        public static string NAME = "FlowerBreak";
        public FlowerBreak() : base(NAME) { }
        public override List<Material> GetMaterials()
        {
            GameObject hd = HeroController.instance.transform.Find("White_Flower_Break").gameObject;
            var M = new List<Material>{
                hd.Find("white_petal_break_particle").GetComponent<ParticleSystemRenderer>().material
            };
            return M;
        }

    }
}
