namespace CustomKnight
{
    public class FlowerBreak : Skinable_Tk2ds
    {
        public static string NAME = "FlowerBreak";
        public FlowerBreak() : base(NAME) { }
        public override List<Material> GetMaterials()
        {
            GameObject hd = HeroController.instance.transform.Find("White_Flower_Break").gameObject;
            var M = new List<Material>{
                hd.Find("white_petal_break_particle").GetComponent<ParticleSystemRenderer>().material
            };
            CustomKnight.dumpManager.SaveTextureByPath("Debug", "FlowerBreak", (Texture2D)M[0].mainTexture);
            return M;
        }

    }
}
