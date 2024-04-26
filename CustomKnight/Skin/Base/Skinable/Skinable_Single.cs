namespace CustomKnight
{
    /// <summary>
    ///     The abstract Class that represents a <c>Skinable</c> that uses a single Material
    /// </summary>
    public abstract class Skinable_Single : Skinable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        public Skinable_Single(string name) : base(name) { }
        private Material _material;
        /// <summary>
        /// Current Material to skin
        /// </summary>
        public Material material
        {
            get
            {
                if (_material == null)
                {
                    try
                    {
                        _material = GetMaterial();
                    }
                    catch (Exception e)
                    {
                        CustomKnight.Instance.Log(e.ToString());
                    }
                }
                return _material;
            }
            set
            {
                _material = value;
            }
        }

        /// <summary>
        ///     An abstract Method that returns the Material for this <c>Skinable</c>
        /// </summary>
        public abstract Material GetMaterial();

        /// <summary>
        ///     A Method that is called to so that any preparations for the Skinable can be done. for example, saving the material that will be used.
        /// </summary>
        public override void Prepare()
        {
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

}