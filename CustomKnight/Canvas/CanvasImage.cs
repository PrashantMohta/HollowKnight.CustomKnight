using UnityEngine;
using UnityEngine.UI;

namespace CustomKnight.Canvas
{
     public class CanvasImage
    {
        private GameObject imageObj;
        private Vector2 sz;
        private Rect sub;

        public bool active;

        public CanvasImage(GameObject parent, Texture2D tex, Vector2 pos, Vector2 size, Rect subSprite)
        {
            if (size.x == 0 || size.y == 0)
            {
                size = new Vector2(subSprite.width, subSprite.height);
            }

            sz = size;
            sub = subSprite;

            imageObj = new GameObject();
            imageObj.AddComponent<CanvasRenderer>();
            RectTransform imageTransform = imageObj.AddComponent<RectTransform>();
            imageTransform.sizeDelta = new Vector2(subSprite.width, subSprite.height);
            imageObj.AddComponent<Image>().sprite = Sprite.Create(tex, new Rect(subSprite.x, tex.height - subSprite.height, subSprite.width, subSprite.height), Vector2.zero);

            CanvasGroup group = imageObj.AddComponent<CanvasGroup>();
            group.interactable = false;
            group.blocksRaycasts = false;

            imageObj.transform.SetParent(parent.transform, false);

            Vector2 position = new Vector2((pos.x + ((size.x / subSprite.width) * subSprite.width) / 2f) / 1920f, (1080f - (pos.y + ((size.y / subSprite.height) * subSprite.height) / 2f)) / 1080f);
            imageTransform.anchorMin = position;
            imageTransform.anchorMax = position;
            imageTransform.SetScaleX(size.x / subSprite.width);
            imageTransform.SetScaleY(size.y / subSprite.height);

            Object.DontDestroyOnLoad(imageObj);

            active = true;
        }

        public void UpdateImage(Texture2D tex, Rect subSection)
        {
            if (imageObj != null)
            {
                imageObj.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(subSection.x, tex.height - subSection.height, subSection.width, subSection.height), Vector2.zero);
            }
        }

        public float GetWidth()
        {
            return sz.x;
        }

        public float GetHeight()
        {
            return sz.y;
        }

        public void SetWidth(float width)
        {
            if (imageObj != null)
            {
                sz = new Vector2(width, sz.y);
                imageObj.GetComponent<RectTransform>().SetScaleX(width / imageObj.GetComponent<RectTransform>().sizeDelta.x);
            }
        }

        public void SetHeight(float height)
        {
            if (imageObj != null)
            {
                sz = new Vector2(sz.x, height);
                imageObj.GetComponent<RectTransform>().SetScaleY(height / imageObj.GetComponent<RectTransform>().sizeDelta.y);
            }
        }

        public void SetPosition(Vector2 pos)
        {
            if (imageObj != null)
            {
                Vector2 position = new Vector2((pos.x + ((sz.x / sub.width) * sub.width) / 2f) / 1920f, (1080f - (pos.y + ((sz.y / sub.height) * sub.height) / 2f)) / 1080f);
                imageObj.GetComponent<RectTransform>().anchorMin = position;
                imageObj.GetComponent<RectTransform>().anchorMax = position;
            }
        }

        public void SetActive(bool b)
        {
            if (imageObj != null)
            {
                imageObj.SetActive(b);
                active = b;
            }
        }

        public void SetRenderIndex(int idx)
        {
            imageObj.transform.SetSiblingIndex(idx);
        }

        public void Destroy()
        {
            Object.Destroy(imageObj); ;
        }
    }
}