namespace CustomKnight
{
    public class Preload : Skinable_Single
    {
        public Func<GameObject> preloadGet;
        public Preload(string PreloadName,Func<GameObject> preloadGet) : base(PreloadName){
            this.preloadGet = preloadGet;
        }
        public override Material GetMaterial(){
            return this.preloadGet()?.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
        }
        //
        public override void SaveDefaultTexture(){
            if(!CustomKnight.GlobalSettings.Preloads){ return;}
            if(material != null && material.mainTexture != null){
                ckTex.defaultTex = material.mainTexture as Texture2D;
            } else {
                CustomKnight.Instance.Log($"skinable {name} : material is null");
            }
        }
        public override void ApplyTexture(Texture2D tex){
            if(!CustomKnight.GlobalSettings.Preloads){ return;}
            if(ckTex.defaultTex == null){
                //incase we do not have the default texture save it.
                ckTex.defaultTex = material.mainTexture as Texture2D;
            }
            material.mainTexture = tex;
        }
    }
}