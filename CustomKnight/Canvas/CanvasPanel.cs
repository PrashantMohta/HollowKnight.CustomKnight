using UnityEngine.Events;

namespace CustomKnight.Canvas
{
    internal class CanvasPanel
    {
        private GameObject canvas;
        private Vector2 position;
        private Vector2 size;
        private Dictionary<string, CanvasButton> buttons = new Dictionary<string, CanvasButton>();
        private Dictionary<string, CanvasPanel> panels = new Dictionary<string, CanvasPanel>();
        private Dictionary<string, CanvasText> texts = new Dictionary<string, CanvasText>();

        public bool active = true;

        public CanvasPanel(GameObject parent, Vector2 pos, Vector2 sz, Rect bgSubSection)
        {
            if (parent == null) return;

            if (sz.x == 0 || sz.y == 0)
            {
                size = new Vector2(bgSubSection.width, bgSubSection.height);
            }
            else
            {
                size = sz;
            }

            position = pos;
            canvas = parent;
        }

        public CanvasButton AddButton(string name, Vector2 pos, Vector2 sz, UnityAction<string> func, Rect bgSubSection, Font font = null, string text = null, int fontSize = 13)
        {
            CanvasButton button = new CanvasButton(canvas, name, position + pos, size + sz, bgSubSection, font, text, fontSize);
            button.AddClickEvent(func);

            buttons.Add(name, button);

            return button;
        }

        public CanvasText AddText(string name, string text, Vector2 pos, Vector2 sz, Font font, int fontSize = 13, FontStyle style = FontStyle.Normal, TextAnchor alignment = TextAnchor.UpperLeft)
        {
            CanvasText t = new CanvasText(canvas, position + pos, sz, font, text, fontSize, style, alignment);

            texts.Add(name, t);

            return t;
        }

        public CanvasButton GetButton(string buttonName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetButton(buttonName);
            }

            if (buttons.ContainsKey(buttonName))
            {
                return buttons[buttonName];
            }

            return null;
        }

        public CanvasText GetText(string textName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetText(textName);
            }

            if (texts.ContainsKey(textName))
            {
                return texts[textName];
            }

            return null;
        }

        public void SetPosition(Vector2 pos)
        {

            Vector2 deltaPos = position - pos;
            position = pos;

            foreach (CanvasButton button in buttons.Values)
            {
                button.SetPosition(button.GetPosition() - deltaPos);
            }

            foreach (CanvasText text in texts.Values)
            {
                text.SetPosition(text.GetPosition() - deltaPos);
            }

            foreach (CanvasPanel panel in panels.Values)
            {
                panel.SetPosition(panel.GetPosition() - deltaPos);
            }
        }


        public void SetActive(bool b, bool panel)
        {

            foreach (CanvasButton button in buttons.Values)
            {
                button.SetActive(b);
            }

            foreach (CanvasText t in texts.Values)
            {
                t.SetActive(b);
            }

            if (panel)
            {
                foreach (CanvasPanel p in panels.Values)
                {
                    p.SetActive(b, false);
                }
            }

            active = b;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void FixRenderOrder()
        {
            foreach (CanvasText t in texts.Values)
            {
                t.MoveToTop();
            }

            foreach (CanvasButton button in buttons.Values)
            {
                button.MoveToTop();
            }

            foreach (CanvasPanel panel in panels.Values)
            {
                panel.FixRenderOrder();
            }

        }

        public void Destroy()
        {

            foreach (CanvasButton button in buttons.Values)
            {
                button.Destroy();
            }

            foreach (CanvasText t in texts.Values)
            {
                t.Destroy();
            }

            foreach (CanvasPanel p in panels.Values)
            {
                p.Destroy();
            }
        }
    }
}