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

        public Skinable(string name){
            this.name = name;
        }
        public CustomKnightTexture _ckTex;
        
        /// <summary>
        ///     A CustomKnightTexture that holds the state of the skin for this skinable.
        /// </summary>
        public CustomKnightTexture ckTex {
             get{ 
                if(_ckTex == null){
                    _ckTex = new CustomKnightTexture(name + ".png", false, null, null);
                }
                return _ckTex;
            } 
            set{
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
        public virtual void prepare(){}

        
        /// <summary>
        ///     Wrapper Method for SaveDefaultTexture, used for logging.
        /// </summary>
        public void SaveTexture(){
            DebugLogger.Log($"SaveTexture skinable {name}");
            SaveDefaultTexture();
        }

        /// <summary>
        ///     Wrapper Method for ApplyTexture, used for logging.
        /// </summary>
        public void Apply(){
            DebugLogger.Log($"Apply skinable {name}");
            ApplyTexture(ckTex.currentTexture);
        }

        /// <summary>
        ///     Wrapper Method for ApplyTexture, used for Resetting the skin to default texture.
        /// </summary>
        public virtual void Reset(){
            DebugLogger.Log($"Reset skinable {name}");
            ApplyTexture(ckTex.defaultTex);
        }
        

    }
    
    /// <summary>
    ///     The abstract Class that represents a <c>Skinable</c> that uses a single Material
    /// </summary>
    public abstract class Skinable_Single : Skinable
    {
        public Skinable_Single(string name) : base(name){}
        private Material _material;
        public Material material {
            get{ 
                if(_material == null){
                    try{
                        _material = GetMaterial();
                    } catch (Exception e){
                        CustomKnight.Instance.Log(e.ToString());
                    }
                }
                return _material;
            } 
            set{
                _material = value;
            }
        }
        
        /// <summary>
        ///     An abstract Method that returns the Material for this <c>Skinable</c>
        /// </summary>
        public abstract Material GetMaterial();
        public override void prepare(){
            try
            {
                material = GetMaterial();
            }
            catch (Exception e)
            {
                CustomKnight.Instance.Log(e.ToString());
            }
        }

    }
    
    /// <summary>
    ///     The abstract Class that represents a <c>Skinable</c> that uses a multiple Materials
    /// </summary>
    public abstract class Skinable_Multiple : Skinable
    {

        public Skinable_Multiple(string name) : base(name){}
        private List<Material> _materials;
        public List<Material> materials {
            get{ 
                if(_materials == null){
                    try{
                        _materials = GetMaterials();
                    } catch (Exception e){
                        CustomKnight.Instance.Log(e.ToString());
                    }
                }
                return _materials;
            } 
            set{
                _materials = value;
            }
        }

        /// <summary>
        ///     An abstract Method that returns the List{Material} for this <c>Skinable</c>
        /// </summary>
        public abstract List<Material> GetMaterials();
        public override void prepare(){
            var m = GetMaterials();
            if(m != null && !(m.Exists(i => i == null))){
                materials = m;
            }
        }

    }
    
    /// <summary>
    ///     The abstract Class that represents a <c>Skinable</c> that uses a Sprite
    /// </summary>
    public abstract class Skinable_Sprite : Skinable
    {
        public Skinable_Sprite(string name) : base(name){}

        public override void ApplyTexture(Texture2D tex){
            if(!ckTex.missing){
                ApplySprite(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), ckTex.defaultSprite.pixelsPerUnit));
            } else {
                DebugLogger.Log($"Missing Sprite for skinable {name}");
                Reset();
            }
        }
        
        /// <summary>
        ///     An abstract Method that Applies a Sprite to this <c>Skinable</c>
        /// </summary>
        public abstract void ApplySprite(Sprite sprite);
        public override void Reset(){
            DebugLogger.Log($"Reset skinable {name}");
            ApplySprite(ckTex.defaultSprite);
        }
    }

    public abstract class Skinable_Tk2d : Skinable_Single
    {
        public Skinable_Tk2d(string name) : base(name){}

        public override void SaveDefaultTexture(){
            if(material != null && material.mainTexture != null){
                ckTex.defaultTex = material.mainTexture as Texture2D;
            } else {
                CustomKnight.Instance.Log($"skinable {name} : material is null");
            }
        }
        public override void ApplyTexture(Texture2D tex){
            if(material == null) { return; }
            if(ckTex.defaultTex == null){
                //incase we do not have the default texture save it.
                ckTex.defaultTex = material.mainTexture as Texture2D;
            }
            material.mainTexture = tex;
        }
    }

    public abstract class Skinable_Tk2ds : Skinable_Multiple
    {
        public Skinable_Tk2ds(string name) : base(name){}

        public override void SaveDefaultTexture(){
            if(materials != null && materials[0].mainTexture != null){
                ckTex.defaultTex = materials[0].mainTexture as Texture2D;
            } else {
                CustomKnight.Instance.Log($"skinable {name} : material is null");
            }
        }
        public override void ApplyTexture(Texture2D tex){
            foreach(var mat in materials){
                mat.mainTexture = tex;
            }
        }
    }

    public abstract class Skinable_noCache : Skinable
    {
        public Skinable_noCache(string name) : base(name){}

        public abstract Material GetMaterial();

        public override void SaveDefaultTexture(){
            var material = GetMaterial();
            if(material != null && material.mainTexture != null){
                ckTex.defaultTex = material.mainTexture as Texture2D;
            } else {
                CustomKnight.Instance.Log($"skinable {name} : material is null");
            }
        }
        public override void ApplyTexture(Texture2D tex){
            var material = GetMaterial();
            if(ckTex.defaultTex == null){
                //incase we do not have the default texture save it.
                ckTex.defaultTex = material.mainTexture as Texture2D;
            }
            material.mainTexture = tex;
        }

    }

}