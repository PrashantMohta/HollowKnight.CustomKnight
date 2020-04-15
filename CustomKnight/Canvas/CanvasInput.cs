using UnityEngine;
using UnityEngine.UI;

namespace CustomKnight.Canvas
{
    public class CanvasInput
    {
        private readonly GameObject _inputObject;
        private readonly GameObject _textObject;
        private readonly GameObject _placeholderObj;
        private readonly InputField _inputField;
        private string _inputName;
        
        private bool _active;

        public CanvasInput(GameObject parent, string name, Texture2D texture, Vector2 position, Vector2 size, Rect bgSubSection, Font font = null, string inputText = "", string placeholderText = "", int fontSize = 13)
        {
            if (size.x == 0 || size.y == 0)
            {
                size = new Vector2(bgSubSection.width, bgSubSection.height);
            }
            
            _inputName = name;
            
            Log("Creating inputObject");
            _inputObject = new GameObject();
            _inputObject.AddComponent<CanvasRenderer>();
            RectTransform inputTransform = _inputObject.AddComponent<RectTransform>();
            inputTransform.sizeDelta = new Vector2(bgSubSection.width, bgSubSection.height);
            Image image = _inputObject.AddComponent<Image>();
            image.sprite = Sprite.Create(
                texture, 
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(texture.width / 2, texture.height / 2)
            );
            image.type = Image.Type.Sliced;
            _inputField = _inputObject.AddComponent<InputField>();

            Log("Setting inputObject Transform");
            _inputObject.transform.SetParent(parent.transform, false);
            inputTransform.SetScaleX(size.x / bgSubSection.width);
            inputTransform.SetScaleY(size.y / bgSubSection.height);
            
            Log("Setting inputObject Anchor");
            Vector2 pos = new Vector2((position.x + ((size.x / bgSubSection.width) * bgSubSection.width) / 2f) / 1920f, (1080f - (position.y + ((size.y / bgSubSection.height) * bgSubSection.height) / 2f)) / 1080f);
            inputTransform.anchorMin = pos;
            inputTransform.anchorMax = pos;
            
            Object.DontDestroyOnLoad(_inputObject);
            
            Log("Creating Text Object");
            _textObject = new GameObject("Text");
            _textObject.AddComponent<RectTransform>().sizeDelta = new Vector2(bgSubSection.width, bgSubSection.height);
            Text textTxt = _textObject.AddComponent<Text>();
            textTxt.text = inputText;
            textTxt.fontSize = fontSize;
            textTxt.color = Color.black;
            textTxt.alignment = TextAnchor.MiddleCenter;
            _textObject.transform.SetParent(_inputObject.transform, false);
            Object.DontDestroyOnLoad(_textObject);
            
            Log("Creating Placeholder Object");
            _placeholderObj = new GameObject("Placeholder");
            _placeholderObj.AddComponent<RectTransform>().sizeDelta = new Vector2(bgSubSection.width, bgSubSection.height);
            Text placeholderTxt = _placeholderObj.AddComponent<Text>();
            placeholderTxt.text = placeholderText;
            placeholderTxt.fontSize = fontSize;
            placeholderTxt.fontStyle = FontStyle.Italic;
            placeholderTxt.color = new Color(0, 0, 0, 0.5f);
            placeholderTxt.alignment = TextAnchor.MiddleCenter;
            _placeholderObj.transform.SetParent(_inputObject.transform, false);
            Object.DontDestroyOnLoad(_placeholderObj);
            
            _active = true;
        }
        
        public void SetActive(bool active)
        {
            if (_inputObject != null)
            {
                _inputObject.SetActive(active);
                _active = active;
            }
        }

        public Vector2 GetPosition()
        {
            if (_inputObject != null)
            {
                Vector2 anchor = _inputObject.GetComponent<RectTransform>().anchorMin;
                Vector2 size = _inputObject.GetComponent<RectTransform>().sizeDelta;

                return new Vector2(anchor.x * 1920f - size.x / 2f, 1080f - anchor.y * 1080f - size.y / 2f);
            }

            return Vector2.zero;
        }

        public void SetPosition(Vector2 pos)
        {
            if (_inputObject != null)
            {
                Vector2 sz = _inputObject.GetComponent<RectTransform>().sizeDelta;
                Vector2 position = new Vector2((pos.x + sz.x / 2f) / 1920f, (1080f - (pos.y + sz.y / 2f)) / 1080f);
                _inputObject.GetComponent<RectTransform>().anchorMin = position;
                _inputObject.GetComponent<RectTransform>().anchorMax = position;
            }
        }
        
        public string GetText()
        {
            if (_inputObject != null)
            {
                return _textObject.GetComponent<Text>().text;
            }

            return null;
        }

        public void Focus()
        {
            _inputField.Select();
            _inputField.ActivateInputField();
        }

        public void ChangePlaceholder(string text)
        {
            _placeholderObj.GetComponent<Text>().text = text;
        }
        
        public void Destroy()
        {
            Object.Destroy(_inputObject);
            Object.Destroy(_textObject);
        }

        private void Log(object message) => Modding.Logger.Log("[Canvas Input] " + message);
    }
}