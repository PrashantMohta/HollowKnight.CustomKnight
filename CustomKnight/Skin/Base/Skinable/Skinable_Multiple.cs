namespace CustomKnight
{
    /// <summary>
    ///     The abstract Class that represents a <c>Skinable</c> that uses a multiple Materials
    /// </summary>
    public abstract class Skinable_Multiple : Skinable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"> Name of your skinable </param>
        public Skinable_Multiple(string name) : base(name) { }

        private List<Material> _materials;

        /// <summary>
        /// List of materials that we must skin
        /// </summary>
        public List<Material> materials
        {
            get
            {
                if (_materials == null)
                {
                    try
                    {
                        _materials = GetMaterials();
                    }
                    catch (Exception e)
                    {
                        CustomKnight.Instance.Log(e.ToString());
                    }
                }
                return _materials;
            }
            set
            {
                _materials = value;
            }
        }

        /// <summary>
        ///     An abstract Method that returns the List{Material} for this <c>Skinable</c>
        /// </summary>
        public abstract List<Material> GetMaterials();

        /// <summary>
        ///     A Method that is called to so that any preparations for the Skinable can be done. for example, saving the material that will be used.
        /// </summary>
        public override void Prepare()
        {
            var m = GetMaterials();
            if (m != null && !(m.Exists(i => i == null)))
            {
                materials = m;
            }
        }

    }

}