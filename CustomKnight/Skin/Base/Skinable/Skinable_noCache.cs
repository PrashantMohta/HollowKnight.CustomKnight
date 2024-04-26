namespace CustomKnight
{
    /// <summary>
    /// Skinable that does not cache the material
    /// </summary>
    public abstract class Skinable_noCache : Skinable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"> Name of the skinable</param>
        public Skinable_noCache(string name) : base(name) { }

        /// <summary>
        /// Function you must implement to return the material to be skinned
        /// </summary>
        /// <returns></returns>
        public abstract Material GetMaterial();

        /// <summary>
        ///     A Method that is called to create a backup of the default texture of the skinable, used when unloading or as a fallback.
        /// </summary>
        public override void SaveDefaultTexture()
        {
            var material = GetMaterial();
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
        ///     A Method that is called to Apply a Texture to the current Skinable.
        /// </summary>
        /// <param name="tex">A <c>Texture2D</c> that is to be applied</param>
        public override void ApplyTexture(Texture2D tex)
        {
            var material = GetMaterial();
            if (ckTex.defaultTex == null)
            {
                //incase we do not have the default texture save it.
                ckTex.defaultTex = material.mainTexture as Texture2D;
            }
            material.mainTexture = tex;
        }

    }

}