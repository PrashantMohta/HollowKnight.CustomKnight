using UnityEngine.UI;

namespace CustomKnight.NewUI
{
    public class UIButton
    {
        public string name, displayName;
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
        public UIButton(GameObject parent, string name, string displayName, Action<UIButton> callback)
        {
            this.name = name;
            this.parent = parent;
            this.callback = callback;
            this.displayName = displayName;
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

            if (sprite != null)
            {
                var img = self.AddComponent<Image>();
                img.raycastTarget = true;
                img.sprite = sprite;
                buttonTransform.sizeDelta = new Vector2(130f, 125f);
            }
            if (displayName != null)
            {
                var text = self.GetAddComponent<Text>();
                text.text = displayName;
                text.fontStyle = FontStyle.Bold;
                text.alignment = TextAnchor.MiddleCenter;
                text.fontSize = 15;
                text.color = Color.white;
                text.font = UIController.trajanBold ?? UIController.arial;
                buttonTransform.sizeDelta = new Vector2(500f, 35f);
            }

            buttonTransform.anchorMin = new Vector2(0f, 0f);
            buttonTransform.anchorMax = new Vector2(1f, 1f);
            //buttonTransform.sizeDelta = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

        }

        private void ButtonClicked()
        {
            callback(this);
        }

    }

}
