namespace CustomKnight
{

    /// <summary>
    ///     The Class that represents the state of a <c>Skinable's</c> texture.
    /// </summary>
    public class CustomKnightTexture
    {
        /// <summary>
        /// is the texture missing
        /// </summary>
        public bool missing;
        /// <summary>
        /// filename of the texture
        /// </summary>
        public string fileName;
        /// <summary>
        /// defaultSprite of the texture pulled from game
        /// </summary>
        public Sprite defaultSprite;
        /// <summary>
        /// default texture of the CKT pulled from the game
        /// </summary>
        public Texture2D defaultTex;
        /// <summary>
        /// the loaded texture
        /// </summary>
        public Texture2D tex;
        /// <summary>
        /// is a sprite or a texture
        /// </summary>
        public bool IsSprite => defaultSprite != null;
        /// <summary>
        /// Gets the tex or defaultTex depending on availablilty
        /// </summary>
        public Texture2D currentTexture
        {
            get
            {
                if (missing)
                {
                    return defaultTex;
                }
                return tex;
            }
            set
            {
                tex = value;
            }
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="missing"></param>
        /// <param name="defaultTex"></param>
        /// <param name="tex"></param>
        public CustomKnightTexture(string fileName, bool missing, Texture2D defaultTex, Texture2D tex)
        {
            this.fileName = fileName;
            this.missing = missing;
            this.defaultTex = defaultTex;
            this.tex = tex;
        }

    }
}