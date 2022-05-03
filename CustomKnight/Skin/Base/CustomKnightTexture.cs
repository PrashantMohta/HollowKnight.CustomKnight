namespace CustomKnight
{

    /// <summary>
    ///     The Class that represents the state of a <c>Skinable's</c> texture.
    /// </summary>
    public class CustomKnightTexture
    {
        public bool missing;
        public string fileName;
        public Sprite defaultSprite;
        public Texture2D defaultTex;
        public Texture2D tex;
        public Texture2D currentTexture{
            get{
                if(missing){
                    return defaultTex;
                }
                return tex;
            }
            set{
                tex = value;
            }
        }

        public CustomKnightTexture(string fileName, bool missing, Texture2D defaultTex, Texture2D tex)
        {
            this.fileName = fileName;
            this.missing = missing;
            this.defaultTex = defaultTex;
            this.tex = tex;
        }

    }
}