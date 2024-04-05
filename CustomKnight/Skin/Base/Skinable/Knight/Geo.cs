namespace CustomKnight
{
    public class Geo : Skinable_Tk2d
    {
        public static string NAME = "Geo";
        public Geo() : base(NAME) { }

        public tk2dSpriteDefinition spriteDefinition;

        public void SetGeoDefaultTexture(GameObject geo)
        {
            spriteDefinition = geo.GetComponent<tk2dSprite>().GetCurrentSpriteDef();
            material = spriteDefinition.material;

            //save default texture because we dont have a copy
            if (ckTex.defaultTex == null)
            {
                ckTex.defaultTex = (Texture2D)material.mainTexture;
            }
        }
        public void GeoControl_Start(On.GeoControl.orig_Start orig, GeoControl self)
        {
            SetGeoDefaultTexture(self.gameObject);
            var geoTexture = ckTex.currentTexture;
            if (geoTexture != null && material != null)
            {
                material.mainTexture = geoTexture;
            }
            On.GeoControl.Start -= GeoControl_Start;
            orig(self);
        }

        public override Material GetMaterial()
        {
            return spriteDefinition?.material;
        }

    }
}