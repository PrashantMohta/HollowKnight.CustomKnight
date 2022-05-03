namespace CustomKnight.Canvas
{
    public class SkinSwapperPanel
    {
        public static CanvasPanel Panel;

        public static void hidePanel(string bn){
            CustomKnight.GlobalSettings.showMovedText = false;
            if(Panel != null) {
                Panel.SetActive(false, true);
            }
        }
        public static void BuildMenu(GameObject canvas)
        {
            float currentElementPosY = 100f;
            int PanelWidth = 500;

            int OptionSize = 25;
            int fontSize = (int)(OptionSize * 0.85f);
            int headingSize = 50;
            int headingFontSize = (int)(OptionSize * 0.85f);


            Panel = new CanvasPanel(
                canvas,
                new Vector2(0, currentElementPosY), 
                Vector2.zero,
                new Rect(0, 0, PanelWidth, 60)
            );

            Panel.AddText(
                "Change Skin Text",
                "Looking for Custom Knight? \n Check the Mods Menu under Options",
                new Vector2(0, currentElementPosY),
                new Vector2(PanelWidth, headingSize), 
                GUIController.Instance.trajanNormal,
                headingFontSize,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
            currentElementPosY += headingSize;

            GC.Collect();


            Panel.AddButton(
                "help button",
                new Vector2(0, currentElementPosY),
                Vector2.zero,
                hidePanel,
                new Rect(0, currentElementPosY, PanelWidth, OptionSize),
                GUIController.Instance.trajanNormal,
                "Okay",
                fontSize
            );
            currentElementPosY += OptionSize;

            Panel.SetActive(false, true);
            
            Vector2 newPanelSize = new Vector2(PanelWidth, currentElementPosY);
            
            On.HeroController.Pause += OnPause;
            On.HeroController.UnPause += OnUnpause;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
        }

        
        
        private static void OnPause(On.HeroController.orig_Pause orig, HeroController hc)
        {
            if(CustomKnight.GlobalSettings.showMovedText){
                Panel.SetActive(true, false);
            }
            orig(hc);
        }
        
        private static void OnUnpause(On.HeroController.orig_UnPause orig, HeroController hc)
        {
            Panel.SetActive(false, true);
            orig(hc);
        }

        private static void OnSceneChange(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "Menu_Title")
            {
                Panel.SetActive(false, true);
            }
        }

    }
}