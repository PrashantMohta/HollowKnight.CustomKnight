namespace CustomKnight
{
    /// <summary>
    /// Used for Skinning Tk2d sprites by material
    /// </summary>
    public abstract class Skinable_Tk2d : Skinable_Single
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        public Skinable_Tk2d(string name) : base(name) { }
        /// <summary>
        /// A Method that is called to create a backup of the default texture
        /// </summary>
        public override void SaveDefaultTexture()
        {
            if (material != null && material.mainTexture != null)
            {
                ckTex.defaultTex = material.mainTexture as Texture2D;
            }
            else
            {
                CustomKnight.Instance.Log($"skinable {name} : material is null");
            }
        }
        /// <summary>
        /// A Method that is called to Apply a Texture to the current Skinable.
        /// </summary>
        /// <param name="tex"></param>
        public override void ApplyTexture(Texture2D tex)
        {
            if (material == null) { return; }
            if (ckTex.defaultTex == null)
            {
                //incase we do not have the default texture save it.
                ckTex.defaultTex = material.mainTexture as Texture2D;
            }
            material.mainTexture = tex;
        }
    }

}