namespace CustomKnight
{
    internal class Grubberfly : Skinable_Multiple
    {
        public static string NAME = "Beam";
        public Grubberfly() : base(Grubberfly.NAME) { }
        public override List<Material> GetMaterials()
        {
            List<Material> mats = new List<Material>{
                // get all materials here
                HeroController.instance.grubberFlyBeamPrefabD_fury.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material,
                HeroController.instance.grubberFlyBeamPrefabD.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material,
                HeroController.instance.grubberFlyBeamPrefabU_fury.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material,
                HeroController.instance.grubberFlyBeamPrefabU.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material,
                HeroController.instance.grubberFlyBeamPrefabL_fury.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material,
                HeroController.instance.grubberFlyBeamPrefabL.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material,
                HeroController.instance.grubberFlyBeamPrefabR_fury.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material,
                HeroController.instance.grubberFlyBeamPrefabR.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material
            };
            return mats;
        }
        public override void SaveDefaultTexture()
        {
            if (materials != null && materials[0].mainTexture != null)
            {
                ckTex.defaultTex = materials[0].mainTexture as Texture2D;
            }
            else
            {
                CustomKnight.Instance.Log($"skinable {name} : material is null");
            }
        }
        public override void ApplyTexture(Texture2D tex)
        {
            foreach (var mat in materials)
            {
                mat.mainTexture = tex;
            }
        }

    }
}