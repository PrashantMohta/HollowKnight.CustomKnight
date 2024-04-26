namespace CustomKnight
{

    /// <summary>
    ///     The abstract Class that represents a single Skinable resource in the game.
    ///     These can be used to add support for custom objects to be skinned as if they were defined within Custom Knight
    /// </summary>
    public abstract class Skinable
    {
        /// <summary>
        ///     The name of the Skinable resource, is used as an identifier and the file expected is name.png.
        /// </summary>
        public string name;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"> Name of the skinable </param>
        public Skinable(string name)
        {
            this.name = name;
        }

        private CustomKnightTexture _ckTex;

        /// <summary>
        ///     A CustomKnightTexture that holds the state of the skin for this skinable.
        /// </summary>
        public CustomKnightTexture ckTex
        {
            get
            {
                if (_ckTex == null)
                {
                    _ckTex = new CustomKnightTexture(name + ".png", false, null, null);
                }
                return _ckTex;
            }
            set
            {
                _ckTex = value;
            }
        }


        /// <summary>
        ///     A Method that is called to create a backup of the default texture of the skinable, used when unloading or as a fallback.
        ///     this must save the defaultTexture in ckTex for seamless integration.
        /// </summary>
        public abstract void SaveDefaultTexture();

        /// <summary>
        ///     A Method that is called to Apply a Texture to the current Skinable.
        /// </summary>
        /// <param name="tex">A <c>Texture2D</c> that is to be applied</param>
        public abstract void ApplyTexture(Texture2D tex);

        /// <summary>
        ///     A Method that is called to so that any preparations for the Skinable can be done. for example, saving the material that will be used.
        /// </summary>
        public virtual void Prepare() { }


        /// <summary>
        ///     Wrapper Method for SaveDefaultTexture, used for logging.
        /// </summary>
        public void SaveTexture()
        {
            CustomKnight.Instance.LogFine($"SaveTexture skinable {name}");
            SaveDefaultTexture();
        }

        /// <summary>
        ///     Wrapper Method for ApplyTexture, used for logging.
        /// </summary>
        public void Apply()
        {
            CustomKnight.Instance.LogFine($"Apply skinable {name}");
            ApplyTexture(ckTex.currentTexture);
        }

        /// <summary>
        ///     Wrapper Method for ApplyTexture, used for Resetting the skin to default texture.
        /// </summary>
        public virtual void Reset()
        {
            CustomKnight.Instance.LogFine($"Reset skinable {name}");
            ApplyTexture(ckTex.defaultTex);
        }

        /// <summary>
        ///     Method that Dumps the Default Texture to the disk
        /// </summary>
        public void DumpDefaultTexture()
        {
            SaveDefaultTexture();
            if (ckTex.defaultTex != null)
            {
                DefaultSkin.Save(ckTex.defaultTex, $"{name}.png");
            }
            else if (ckTex.defaultSprite != null)
            {
                Texture2D tex = AssemblyUtils.GetTextureFromResources($"{name.Replace('/', '.')}.png");
                if (tex == null)
                {
                    tex = SpriteUtils.ExtractTextureFromSprite(ckTex.defaultSprite);
                }
                else
                {
                    CustomKnight.Instance.Log($"Extracting {name} from Assembly Resource");
                }
                DefaultSkin.Save(tex, $"{name}.png");
            }
            else
            {
                CustomKnight.Instance.Log($"Failed to generate default skin for {name}");
            }
        }


    }

}