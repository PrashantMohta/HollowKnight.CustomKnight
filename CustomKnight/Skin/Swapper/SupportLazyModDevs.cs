namespace CustomKnight.Skin.Swapper
{
    internal class SupportLazyModDevs
    {
        internal static void Hook()
        {
            On.tk2dSprite.Awake += Tk2dSprite_Awake;
        }

        internal static void Unhook()
        {
            On.tk2dSprite.Awake -= Tk2dSprite_Awake;
        }

        internal static void Enable()
        {
            SwapManager.OnApplySkinUsingProxy += SwapManager_OnApplySkinUsingProxy;
        }

        internal static void Disable()
        {
            SwapManager.OnApplySkinUsingProxy -= SwapManager_OnApplySkinUsingProxy;
        }

        private static void SwapManager_OnApplySkinUsingProxy(object sender, SwapEvent e)
        {
            var marker = e.go.GetComponent<GlobalSwapMarker>();
            var tk2d = e.go.GetComponent<tk2dSprite>();
            if (tk2d != null && marker != null && !marker.optOut)
            {
                Debug.Log("Swapping by path " + marker.originalPath);
                CustomKnight.swapManager.applyGlobalTk2dByPath(marker.originalPath, tk2d);
            }
        }

        private static void Tk2dSprite_Awake(On.tk2dSprite.orig_Awake orig, tk2dSprite tk)
        {
            orig(tk);
            var path = tk.gameObject.scene.name + "/" + tk.gameObject.GetPath(true);
            var marker = tk.gameObject.GetAddComponent<GlobalSwapMarker>();
            marker.originalPath = path;
        }
    }
}
