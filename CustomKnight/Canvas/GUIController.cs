using UnityEngine.UI;

namespace CustomKnight.Canvas
{
    public class GUIController : MonoBehaviour
    {
        public GameObject canvas;
        private static GUIController _instance;
        
        public void BuildMenus()
        {
            if (!GameObject.Find("Custom Knight Canvas"))
            {
                Log("Building Help Text");
                
                LoadResources();
                
                canvas = new GameObject("Custom Knight Canvas");
                canvas.AddComponent<UnityEngine.Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920f, 1080f);
                canvas.AddComponent<GraphicRaycaster>();
                if(CustomKnight.GlobalSettings.showMovedText){
                    SkinSwapperPanel.BuildMenu(canvas);
                }
                SkinSwapperPanel.BuildDumpingUpdatePanel(canvas);

                DontDestroyOnLoad(canvas);
            }
        }

        public static GUIController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GUIController>();
                    if (_instance == null)
                    {
                        GameObject GUIController = new GameObject("GUI Controller");
                        _instance = GUIController.AddComponent<GUIController>();
                        DontDestroyOnLoad(GUIController);
                    }
                }
                return _instance;
            }
        }

        public Font arial;
        public Font perpetua;
        public Font trajanBold;
        public Font trajanNormal;

        private void LoadResources()
        {
            foreach (Font font in Resources.FindObjectsOfTypeAll<Font>())
            {
                if (font != null && font.name == "TrajanPro-Bold")
                {
                    trajanBold = font;
                }

                if (font != null && font.name == "TrajanPro-Regular")
                {
                    trajanNormal = font;
                }

                //Just in case for some reason the computer doesn't have arial
                if (font != null && font.name == "Perpetua")
                {
                    perpetua = font;
                }

                foreach (string fontName in Font.GetOSInstalledFontNames())
                {
                    if (fontName.ToLower().Contains("arial"))
                    {
                        arial = Font.CreateDynamicFontFromOSFont(fontName, 13);
                        break;
                    }
                }
            }
        
        }
        
        private void Log(object message) => Modding.Logger.Log("[GUI Controller] " + message);
    }
}