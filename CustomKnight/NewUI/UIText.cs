using UnityEngine.UI;

namespace CustomKnight.NewUI
{

    internal class UIText
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

}
