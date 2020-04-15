using UnityEngine;
using UnityEngine.UI;

namespace CustomKnight.Canvas
{
    public class CanvasText
    {
        private GameObject textObj;
        private Vector2 size;

        public bool active;

        public CanvasText(GameObject parent, Vector2 pos, Vector2 sz, Font font, string text, int fontSize = 13, FontStyle style = FontStyle.Normal, TextAnchor alignment = TextAnchor.UpperLeft)
        {
            if (sz.x == 0 || sz.y == 0)
            {
                size = new Vector2(1920f, 1080f);
            }
            else
            {
                size = sz;
            }

            textObj = new GameObject("Canvas Text - " + text);
            textObj.AddComponent<CanvasRenderer>();
            RectTransform textTransform = textObj.AddComponent<RectTransform>();
            textTransform.sizeDelta = size;

            CanvasGroup group = textObj.AddComponent<CanvasGroup>();
            group.interactable = false;
            group.blocksRaycasts = false;

            Text t = textObj.AddComponent<Text>();
            t.text = text;
            t.font = font;
            t.fontSize = fontSize;
            t.fontStyle = style;
            t.alignment = alignment;

            Outline outline = textObj.AddComponent<Outline>();
            outline.effectColor = Color.black;

            textObj.transform.SetParent(parent.transform, false);

            Vector2 position = new Vector2((pos.x + size.x / 2f) / 1920f, (1080f - (pos.y + size.y / 2f)) / 1080f);
            textTransform.anchorMin = position;
            textTransform.anchorMax = position;

            Object.DontDestroyOnLoad(textObj);

            active = true;
        }

        public void SetPosition(Vector2 pos)
        {
            if (textObj != null)
            {
                RectTransform textTransform = textObj.GetComponent<RectTransform>();

                Vector2 position = new Vector2((pos.x + size.x / 2f) / 1920f, (1080f - (pos.y + size.y / 2f)) / 1080f);
                textTransform.anchorMin = position;
                textTransform.anchorMax = position;
            }
        }

        public Vector2 GetPosition()
        {
            if (textObj != null)
            {
                Vector2 anchor = textObj.GetComponent<RectTransform>().anchorMin;

                return new Vector2(anchor.x * 1920f - size.x / 2f, 1080f - anchor.y * 1080f - size.y / 2f);
            }

            return Vector2.zero;
        }

        public void UpdateText(string text)
        {
            if (textObj != null)
            {
                textObj.GetComponent<Text>().text = text;
            }
        }

        public void SetActive(bool a)
        {
            active = a;

            if (textObj != null)
            {
                textObj.SetActive(active);
            }
        }

        public void MoveToTop()
        {
            if (textObj != null)
            {
                textObj.transform.SetAsLastSibling();
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

        public void Destroy()
        {
            Object.Destroy(textObj);
        }
    }
}