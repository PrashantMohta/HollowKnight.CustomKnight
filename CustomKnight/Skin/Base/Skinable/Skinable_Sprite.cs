namespace CustomKnight
{
    /// <summary>
    ///     The abstract Class that represents a <c>Skinable</c> that uses a Sprite
    /// </summary>
    public abstract class Skinable_Sprite : Skinable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        public Skinable_Sprite(string name) : base(name) { }

        /// <summary>
        /// Applies a texture
        /// </summary>
        /// <param name="tex"></param>
        public override void ApplyTexture(Texture2D tex)
        {
            if (!ckTex.missing)
            {
                ApplySprite(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), ckTex.defaultSprite.pixelsPerUnit));
            }
            else
            {
                CustomKnight.Instance.Log($"Missing Sprite for skinable {name}");
                Reset();
            }
        }

        /// <summary>
        ///     An abstract Method that Applies a Sprite to this <c>Skinable</c>
        /// </summary>
        public abstract void ApplySprite(Sprite sprite);

        /// <summary>
        /// Used for Resetting the skin to default texture
        /// </summary>
        public override void Reset()
        {
            CustomKnight.Instance.LogFine($"Reset skinable {name}");
            ApplySprite(ckTex.defaultSprite);
        }
    }

}