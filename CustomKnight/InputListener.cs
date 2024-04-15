using CustomKnight.NewUI;

namespace CustomKnight
{
    internal static class InputListener
    {
        private static Coroutine inputCoroutine;

        internal static void Start()
        {
            inputCoroutine = CoroutineHelper.GetRunner().StartCoroutine(ListenForInput());
        }

        private static IEnumerator ListenForInput()
        {
            while (true)
            {
                if (GameManager.instance.isPaused)
                {
                    if (CustomKnight.GlobalSettings.Keybinds.OpenSkinList.WasPressed)
                    {
                        UIController.ToggleSkinList();
                    }
                }
                
                if (CustomKnight.GlobalSettings.Keybinds.ReloadSkins.WasPressed)
                {
                    BetterMenu.ReloadSkins();
                }

                yield return new WaitForEndOfFrame();
            }

        }

    }
}
