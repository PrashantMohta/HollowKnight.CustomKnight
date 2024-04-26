namespace CustomKnight
{
    /// <summary>
    /// Class to Skin multiple Tk2ds by their materials
    /// </summary>
    public abstract class Skinable_Tk2ds : Skinable_Multiple
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        public Skinable_Tk2ds(string name) : base(name) { }

        /// <summary>
        /// A Method that is called to create a backup of the default texture of the skinable
        /// </summary>
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
        /// <summary>
        /// A Method that is called to Apply a Texture to the current Skinable.
        /// </summary>
        /// <param name="tex"></param>
        public override void ApplyTexture(Texture2D tex)
        {
            foreach (var mat in materials)
            {
                mat.mainTexture = tex;
            }
        }
    }

}