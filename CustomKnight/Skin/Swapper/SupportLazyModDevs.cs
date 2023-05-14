using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomKnight.Skin.Swapper
{
    internal class SupportLazyModDevs
    {
        public static void Hook()
        {
            On.tk2dSprite.Awake += Tk2dSprite_Awake;
        }

        public static void Unhook()
        {
            On.tk2dSprite.Awake -= Tk2dSprite_Awake;
        }

        public static void Enable()
        {
            SwapManager.OnApplySkinUsingProxy += SwapManager_OnApplySkinUsingProxy;
        }

        public static void Disable()
        {
            SwapManager.OnApplySkinUsingProxy -= SwapManager_OnApplySkinUsingProxy;
        }

        private static void SwapManager_OnApplySkinUsingProxy(object sender, SwapEvent e)
        {
            var marker = e.go.GetComponent<globalSwapMarker>();
            var tk2d = e.go.GetComponent<tk2dSprite>();
            if (tk2d != null && marker != null)
            {
                Debug.Log("Swapping by path " + marker.originalPath);
                CustomKnight.swapManager.applyGlobalTk2dByPath(marker.originalPath, tk2d);
            }
        }

        private static void Tk2dSprite_Awake(On.tk2dSprite.orig_Awake orig, tk2dSprite tk)
        {
            orig(tk);
            var path = tk.gameObject.scene.name + "/" + tk.gameObject.GetPath(true);
            var marker = tk.gameObject.GetAddComponent<globalSwapMarker>();
            marker.originalPath = path;
        }
    }
}
