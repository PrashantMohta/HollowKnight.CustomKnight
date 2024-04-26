namespace CustomKnight
{
    /// <summary>
    /// The abstract Class that represents a <c>Skinable</c> that uses a Sprite and requires a hook
    /// </summary>
    public abstract class Skinable_Hook : Skinable_Sprite
    {
        /// <summary>
        /// The sprite to apply in your hook
        /// </summary>
        public Sprite sprite;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"> Name of the skinable </param>
        public Skinable_Hook(string name) : base(name)
        {
            CustomKnight.OnInit += CustomKnight_OnInit;
            CustomKnight.OnUnload += CustomKnight_OnUnload;
        }

        private void CustomKnight_OnInit(object sender, EventArgs e)
        {
            CustomKnight.Instance.LogDebug($"Hook {name}");
            Hook();
        }

        private void CustomKnight_OnUnload(object sender, EventArgs e)
        {

            CustomKnight.Instance.LogDebug($"UnHook {name}");
            UnHook();
        }
        /// <summary>
        /// Function you should use to subscribe to your hooks
        /// </summary>
        public abstract void Hook();

        /// <summary>
        /// Function you should use to cleanup your hooks
        /// </summary>
        public abstract void UnHook();

        /// <summary>
        /// Function that can set the sprite to be applied in the hook
        /// </summary>
        /// <param name="newSprite"></param>
        public override void ApplySprite(Sprite newSprite)
        {
            sprite = newSprite;
        }

    }

}