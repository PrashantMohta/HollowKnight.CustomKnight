using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomKnight.Canvas
{
    public class Scroller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private float _clampMin;
        private float _clampMax;
        
        private bool _inScrollArea;
        private BoxCollider2D _scrollArea;
        private float _scrollSpeed;

        private GameObject _lastChild;

        private void Awake()
        {
            _scrollArea = gameObject.AddComponent<BoxCollider2D>();
            _scrollArea.isTrigger = true;
        }
        
        private void Start()
        {
            _scrollArea.size = new Vector2(300, Screen.height);
            _scrollArea.offset = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
            
            _lastChild = gameObject.transform.GetChild(transform.childCount - 1).gameObject;
            _clampMin = _lastChild.transform.position.y;
            _clampMax = _clampMin + 500;
            _scrollSpeed = _clampMax - _clampMin;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _inScrollArea = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _inScrollArea = false;
        }

        private void Update()
        {
            if (_inScrollArea)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                foreach (Transform childTransform in gameObject.transform)
                {
                    if ((scroll > 0 && _lastChild.transform.position.y > _clampMin) || (scroll < 0 && _lastChild.transform.position.y < _clampMax))
                    {
                        GameObject uiElement = childTransform.gameObject;
                        uiElement.transform.position += Vector3.down * scroll * _scrollSpeed;                       
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void Log(object message) => Modding.Logger.Log("[Scroller] " + message);
    }
}