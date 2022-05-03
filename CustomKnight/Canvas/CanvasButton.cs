using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomKnight.Canvas
{
    public class CanvasButton
    {
        private GameObject buttonObj;
        private GameObject textObj;
        private Button button;
        private UnityAction<string> clicked;
        private string buttonName;

        public bool active;

        public CanvasButton(GameObject parent, string name, Vector2 pos, Vector2 size, Rect bgSubSection, Font font = null, string text = null, int fontSize = 13)
        {
            if (size.x == 0 || size.y == 0)
            {
                size = new Vector2(bgSubSection.width, bgSubSection.height);
            }

            buttonName = name;

            buttonObj = new GameObject("Canvas Button - " + name);
            buttonObj.AddComponent<CanvasRenderer>();
            RectTransform buttonTransform = buttonObj.AddComponent<RectTransform>();
            buttonTransform.sizeDelta = new Vector2(bgSubSection.width, bgSubSection.height);
            button = buttonObj.AddComponent<Button>();

            buttonObj.transform.SetParent(parent.transform, false);

            buttonTransform.SetScaleX(size.x / bgSubSection.width);
            buttonTransform.SetScaleY(1.0f);//size.y / bgSubSection.height);

            Vector2 position = new Vector2((pos.x + ((size.x / bgSubSection.width) * bgSubSection.width) / 2f) / 1920f, (1080f - (pos.y + ((size.y / bgSubSection.height) * bgSubSection.height) / 2f)) / 1080f);
            buttonTransform.anchorMin = position;
            buttonTransform.anchorMax = position;

            UnityEngine.Object.DontDestroyOnLoad(buttonObj);

            if (font != null && text != null)
            {
                textObj = new GameObject("Canvas Button Text - " + name);
                textObj.AddComponent<RectTransform>().sizeDelta = new Vector2(bgSubSection.width, bgSubSection.height);
                Text t = textObj.AddComponent<Text>();
                t.text = text;
                t.font = font;
                t.fontSize = fontSize;
                t.alignment = TextAnchor.MiddleCenter;
                Outline outline = textObj.AddComponent<Outline>();
                outline.effectColor = Color.black;
                textObj.transform.SetParent(buttonObj.transform, false);

                UnityEngine.Object.DontDestroyOnLoad(textObj);
            }

            active = true;
        }

        public void AddClickEvent(UnityAction<string> action)
        {
            if (buttonObj != null)
            {
                clicked = action;
                buttonObj.GetComponent<Button>().onClick.AddListener(ButtonClicked);
            }
        }

        private void ButtonClicked()
        {
            if (clicked != null && buttonName != null) clicked(buttonName);
        }

        public void UpdateText(string text)
        {
            if (textObj != null)
            {
                textObj.GetComponent<Text>().text = text;
            }
        }

        public void SetWidth(float width)
        {
            if (buttonObj != null)
            {
                buttonObj.GetComponent<RectTransform>().SetScaleX(width / buttonObj.GetComponent<RectTransform>().sizeDelta.x);
            }
        }

        public void SetHeight(float height)
        {
            if (buttonObj != null)
            {
                buttonObj.GetComponent<RectTransform>().SetScaleY(height / buttonObj.GetComponent<RectTransform>().sizeDelta.y);
            }
        }

        public void SetPosition(Vector2 pos)
        {
            if (buttonObj != null)
            {
                Vector2 sz = buttonObj.GetComponent<RectTransform>().sizeDelta;
                Vector2 position = new Vector2((pos.x + sz.x / 2f) / 1920f, (1080f - (pos.y + sz.y / 2f)) / 1080f);
                buttonObj.GetComponent<RectTransform>().anchorMin = position;
                buttonObj.GetComponent<RectTransform>().anchorMax = position;
            }
        }

        public void SetActive(bool b)
        {
            if (buttonObj != null)
            {
                buttonObj.SetActive(b);
                active = b;
            }
        }


        public void SetRenderIndex(int idx)
        {
            buttonObj.transform.SetSiblingIndex(idx);
        }

        public Vector2 GetPosition()
        {
            if (buttonObj != null)
            {
                Vector2 anchor = buttonObj.GetComponent<RectTransform>().anchorMin;
                Vector2 sz = buttonObj.GetComponent<RectTransform>().sizeDelta;

                return new Vector2(anchor.x * 1920f - sz.x / 2f, 1080f - anchor.y * 1080f - sz.y / 2f);
            }

            return Vector2.zero;
        }

        public void MoveToTop()
        {
            if (buttonObj != null)
            {
                buttonObj.transform.SetAsLastSibling();
            }
        }

        public void SetTextColor(Color color)
        {
            if (textObj != null)
            {
                Text t = textObj.GetComponent<Text>();
                t.color = color;
            }
        }

        public string GetText()
        {
            if (textObj != null)
            {
                return textObj.GetComponent<Text>().text;
            }

            return null;
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(buttonObj);
            UnityEngine.Object.Destroy(textObj);
        }
    }
}