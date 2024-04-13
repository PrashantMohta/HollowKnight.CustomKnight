using UnityEngine.UI;

namespace CustomKnight.Canvas
{
    internal class DumpUIMain
    {
        private static CanvasPanel DumpingUpdatePanel;
        private static GameObject canvas;

        private static Font trajanNormal;

        private static void LoadResources()
        {
            foreach (Font font in Resources.FindObjectsOfTypeAll<Font>())
            {
                if (font != null && font.name == "TrajanPro-Regular")
                {
                    trajanNormal = font;
                }
            }

        }
        private static void BuildMenus()
        {
            if (!GameObject.Find("Custom Knight Dump UI"))
            {
                Log("Building Dump UI");
                LoadResources();
                canvas = new GameObject("Custom Knight Dump UI");
                canvas.AddComponent<UnityEngine.Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920f, 1080f);
                canvas.AddComponent<GraphicRaycaster>();
                DumpUIMain.BuildDumpingUpdatePanel(canvas);
                GameObject.DontDestroyOnLoad(canvas);
            }
        }

        private static void Log(object message) => Modding.Logger.Log("[GUI Controller] " + message);

        private static void BuildDumpingUpdatePanel(GameObject canvas)
        {
            float currentElementPosY = 150f;
            int PanelWidth = 500;

            int OptionSize = 25;
            int headingSize = 50;
            int headingFontSize = (int)(OptionSize * 0.85f);


            DumpingUpdatePanel = new CanvasPanel(
                canvas,
                new Vector2(0, currentElementPosY),
                Vector2.zero,
                new Rect(0, 0, PanelWidth, 60)
            );

            DumpingUpdatePanel.AddText(
                "SpriteDumpText",
                "Dumping Sprites \n 0%",
                new Vector2(0, currentElementPosY),
                new Vector2(PanelWidth, headingSize),
                DumpUIMain.trajanNormal,
                headingFontSize,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );

            DumpingUpdatePanel.SetActive(false, true);
        }

        internal static void UpdateDumpProgressText(float detected, float done)
        {
            DumpUIMain.BuildMenus();
            if (done < detected - 1)
            {
                DumpingUpdatePanel.SetActive(true, true);
            }
            else
            {
                DumpingUpdatePanel.SetActive(false, true);
            }
            var text = DumpingUpdatePanel.GetText("SpriteDumpText");
            text.UpdateText($"Dumping Sprites \n {100f * done / detected:0.0}%");
        }

    }
}