namespace CustomKnight
{
    public class BrummShield : Skinable_Tk2ds
    {
        public static string NAME = "BrummShield";
        public BrummShield() : base(NAME) { }
        public override List<Material> GetMaterials()
        {
            GameObject hd = HeroController.instance.transform.Find("Brumm_shield").gameObject;
            var M = new List<Material>{
                hd.Find("grimm_flame_particle").GetComponent<ParticleSystemRenderer>().material
            };
            CustomKnight.dumpManager.SaveTextureByPath("Debug", "BrummShield", (Texture2D)M[0].mainTexture);
            return M;
        }

    }
}
