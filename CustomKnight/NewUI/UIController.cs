using System.Linq;
using CustomKnight.Next.Skin;
using CustomKnight.Next.Skin.Enum;
using UnityEngine.UI;

namespace CustomKnight.NewUI
{
    internal static class UIController
    {
        private static GameObject UI, content, viewport;
        private static ScrollRect scrollRect;

        internal static Font arial;
        internal static Font perpetua;
        internal static Font trajanBold;
        internal static Font trajanNormal;
        private static float lastScrollPosition;

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
        public static void EnableMenu()
        {
            CustomKnight.GlobalSettings.EnablePauseMenu = true;
            if (UI == null)
            {
                GenerateMenu();
            }
            On.HeroController.Pause += OnPause;
            On.HeroController.UnPause += OnUnpause;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
        }

        public static void DisableMenu()
        {
            CustomKnight.GlobalSettings.EnablePauseMenu = false;
            hideMenu();
            On.HeroController.Pause -= OnPause;
            On.HeroController.UnPause -= OnUnpause;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChange;
        }
        public static void GenerateMenu()
        {
            UI = new GameObject("UI Parent");
            var cv = UI.AddComponent<UnityEngine.Canvas>();
            cv.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = UI.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            UI.AddComponent<GraphicRaycaster>();
            var rt = UI.GetAddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(300f, 1080f);
            rt.pivot = new Vector2(1f, 0f);


            scrollRect = UI.AddComponent<ScrollRect>();


            viewport = new GameObject("Viewport");
            viewport.transform.SetParent(UI.transform, false);
            var viewrt = viewport.GetAddComponent<RectTransform>();
            viewrt.sizeDelta = new Vector2(300f, 1080f);
            viewrt.anchorMin = new Vector2(1f, 0f);
            viewrt.anchorMax = new Vector2(1f, 0f);
            viewrt.pivot = new Vector2(1f, 0f);
            var mask = viewport.AddComponent<Mask>();
            var bg = viewport.GetAddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.7f);

            CreateUpdateGUI();

            scrollRect.viewport = viewrt;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.scrollSensitivity = 25f;
            scrollRect.verticalNormalizedPosition = 1f;

            GameObject.DontDestroyOnLoad(UI);
            hideMenu();
        }
        public static void hideMenu()
        {
            UI.SetActive(false);
        }
        public static void showMenu()
        {
            if (!CustomKnight.GlobalSettings.EnablePauseMenu) { return; }
            UI.SetActive(true);
        }

        public static Texture2D GetSkinIcon(ISkin skin)
        {
            var hasIcon = skin.GetFeatures().Contains(Feature.Icon);
            if (hasIcon)
            {
                return skin.GetIcon();
            }
            return Texture2D.blackTexture;
        }

        public static void ApplySkin(ISkin skin)
        {
            UIManager.instance.TogglePauseGame();
            CustomKnight.GlobalSettings.AddRecentSkin(skin.GetId());
            UIController.CreateUpdateGUI();
            HeroController.instance?.GetComponent<SpriteFlash>().flashFocusHeal();
            CoroutineHelper.WaitForFramesBeforeInvoke(10, () =>
            {
                scrollRect.verticalNormalizedPosition = 1f;
                SkinManager.SetSkinById(skin.GetId());
            });
        }

        public static void CreateUpdateGUI()
        {
            if (UI == null)
            {
                return;
            }
            if (content != null)
            {
                GameObject.DestroyImmediate(content);
            }
            // create new content object

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
            group.childAlignment = TextAnchor.UpperCenter;
            group.padding = new RectOffset() { top = 100, bottom = 300 };
            group.childControlHeight = false;

            scrollRect.content = contentrt;

            var title = new UIText(content, "Title", "Skin List");
            title.text.fontSize = 25;
            var favSkins = new List<string>();
            favSkins.AddRange(CustomKnight.GlobalSettings.FavoriteSkins);
            favSkins.AddRange(CustomKnight.GlobalSettings.RecentSkins);
            for (var index = 0; favSkins.Count < Math.Min(CustomKnight.GlobalSettings.MaxSkinCache, SkinManager.SkinsList.Count); index++)
            {
                var id = SkinManager.SkinsList[index].GetId();
                if (!favSkins.Contains(id))
                {
                    favSkins.Add(id);
                }
            }
            foreach (var skinId in favSkins)
            {
                if (!SkinManager.SkinExistsById(skinId))
                {
                    continue;
                }
                var skin = SkinManager.GetSkinById(skinId);
                var tex = GetSkinIcon(skin);
                var skinName = skin.GetName();
                skinName = skinName.Length > CustomKnight.GlobalSettings.NameLength ? skinName.Substring(0, CustomKnight.GlobalSettings.NameLength) : skinName;

                if (tex != Texture2D.blackTexture)
                {
                    var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    var btn = content.AddButton(skinName, sprite, (e) => { ApplySkin(skin); });
                }
                var txtbtn = content.AddButton($"Name_{skinName}", skinName, (e) => { ApplySkin(skin); });
            }
            foreach (var skin in SkinManager.SkinsList)
            {
                if (favSkins.Contains(skin.GetId()))
                {
                    continue;
                }
                var skinName = skin.GetName();
                skinName = skinName.Length > CustomKnight.GlobalSettings.NameLength ? skinName.Substring(0, CustomKnight.GlobalSettings.NameLength) : skinName;

                var btn = content.AddButton(skinName, skinName, (e) => { ApplySkin(skin); });
            }
        }


        static UIButton AddButton(this GameObject parent, string name, Sprite sprite, Action<UIButton> callback)
        {
            return new UIButton(parent, name, sprite, callback);
        }

        static UIButton AddButton(this GameObject parent, string name, string displayName, Action<UIButton> callback)
        {
            return new UIButton(parent, name, displayName, callback);
        }

        internal static void ToggleSkinList()
        {
            if (CustomKnight.GlobalSettings.EnablePauseMenu)
            {
                DisableMenu();
                lastScrollPosition = scrollRect.verticalNormalizedPosition;
            }
            else
            {
                EnableMenu();
                showMenu();
                scrollRect.verticalNormalizedPosition = lastScrollPosition;
            }
        }
    }
}
