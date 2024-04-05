using UnityEngine.UI;

namespace CustomKnight.NewUI
{
    public class UIText
    {
        public string name, value;
        public GameObject parent;
        public GameObject self;
        public Text text;
        public UIText(GameObject parent, string name, string value)
        {
            this.name = name;
            this.parent = parent;
            this.value = value;
            this.Build();
        }

        private void Build()
        {
            self = new GameObject(name);
            self.transform.SetParent(parent.transform, false);
            self.AddComponent<CanvasRenderer>();
            RectTransform textTransform = self.AddComponent<RectTransform>();
            text = self.AddComponent<Text>();
            text.text = this.value;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 15;
            text.color = Color.white;
            text.font = UIController.trajanBold ?? UIController.arial;
            textTransform.anchorMin = new Vector2(0f, 0f);
            textTransform.anchorMax = new Vector2(1f, 1f);

        }
    }
    public class UIButton
    {
        public string name;
        public GameObject parent;
        public GameObject self;
        public Sprite sprite;
        private Action<UIButton> callback;
        public UIButton(GameObject parent, string name, Sprite sprite, Action<UIButton> callback)
        {
            this.name = name;
            this.parent = parent;
            this.callback = callback;
            this.sprite = sprite;
            this.Build();
        }

        private void Build()
        {
            self = new GameObject(name);
            self.transform.SetParent(parent.transform, false);
            self.AddComponent<CanvasRenderer>();
            RectTransform buttonTransform = self.AddComponent<RectTransform>();
            var btn = self.AddComponent<Button>();
            btn.onClick.AddListener(ButtonClicked);
            var img = self.AddComponent<Image>();
            img.sprite = sprite;
            img.raycastTarget = true;
            buttonTransform.anchorMin = new Vector2(0f, 0f);
            buttonTransform.anchorMax = new Vector2(1f, 1f);
            //buttonTransform.sizeDelta = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

        }

        private void ButtonClicked()
        {
            callback(this);
        }

    }
    public static class UIController
    {
        public static GameObject UI, content, viewport;
        public static ScrollRect scrollRect;

        public static Font arial;
        public static Font perpetua;
        public static Font trajanBold;
        public static Font trajanNormal;

        private static void LoadResources()
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
        static UIController()
        {
            LoadResources();
            UI = new GameObject("UI Parent");
            var cv = UI.AddComponent<UnityEngine.Canvas>();
            cv.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = UI.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            UI.AddComponent<GraphicRaycaster>();
            var rt = UI.GetAddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(200f, 1080f);
            rt.pivot = new Vector2(1f, 0f);


            scrollRect = UI.AddComponent<ScrollRect>();


            viewport = new GameObject("Viewport");
            viewport.transform.SetParent(UI.transform, false);
            var viewrt = viewport.GetAddComponent<RectTransform>();
            viewrt.sizeDelta = new Vector2(200f, 1080f);
            viewrt.anchorMin = new Vector2(1f, 0f);
            viewrt.anchorMax = new Vector2(1f, 0f);
            viewrt.pivot = new Vector2(1f, 0f);
            var mask = viewport.AddComponent<Mask>();



            content = new GameObject("content");
            var bg = content.GetAddComponent<Image>();
            bg.raycastTarget = true;
            bg.color = Color.clear;

            content.transform.SetParent(viewport.transform, false);
            var contentrt = content.GetAddComponent<RectTransform>();
            contentrt.anchorMin = Vector2.zero;
            contentrt.anchorMax = new Vector2(1f, 0f);
            contentrt.pivot = new Vector2(1f, 0.5f);
            var group = content.AddComponent<VerticalLayoutGroup>();
            content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            group.childForceExpandWidth = false;
            group.childAlignment = TextAnchor.MiddleCenter;
            group.padding = new RectOffset() { top = 1000, bottom = 100 };

            scrollRect.viewport = viewrt;
            scrollRect.content = contentrt;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.scrollSensitivity = 25f;
            scrollRect.verticalNormalizedPosition = 1f;

            GameObject.DontDestroyOnLoad(UI);
            On.HeroController.Pause += OnPause;
            On.HeroController.UnPause += OnUnpause;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
            hideMenu();
        }

        public static void hideMenu()
        {
            scrollRect.verticalNormalizedPosition = 1f;
            UI.SetActive(false);
        }

        public static void showMenu()
        {
            if (!CustomKnight.GlobalSettings.EnablePauseMenu) { return; }
            scrollRect.verticalNormalizedPosition = 1f;
            UI.SetActive(true);
        }
        private static void OnSceneChange(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "Menu_Title")
            {
                hideMenu();
            }
        }

        private static void OnUnpause(On.HeroController.orig_UnPause orig, HeroController self)
        {
            hideMenu();
            orig(self);
        }

        private static void OnPause(On.HeroController.orig_Pause orig, HeroController self)
        {
            showMenu();
            orig(self);
        }


        public static void CreateGUI()
        {
            var title = new UIText(content, "Title", "Skin List");
            title.text.fontSize = 25;
            foreach (var skin in SkinManager.SkinsList)
            {
                var btnFileName = "OrbFull.png";
                if (!skin.Exists(btnFileName))
                {
                    continue;
                }
                var tex = skin.GetTexture(btnFileName);//.GetCropped(new Rect(500f,500f,300f,100f));
                var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                var skinName = skin.GetName();
                skinName = skinName.Length > CustomKnight.GlobalSettings.NameLength ? skinName.Substring(0, CustomKnight.GlobalSettings.NameLength) : skinName;
                var btn = content.AddButton(skinName, sprite, (e) =>
                {
                    SkinManager.SetSkinById(skin.GetId());
                });
                new UIText(content, $"skin_{skinName}", skinName);
            }
        }

        static UIButton AddButton(this GameObject parent, string name, Sprite sprite, Action<UIButton> callback)
        {
            return new UIButton(parent, name, sprite, callback);
        }

    }
}
