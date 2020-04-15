using System.Collections.Specialized;
using ModCommon;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomKnight.Canvas
{
    public class Scroller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private BoxCollider2D _scrollArea;
        private bool _inScrollArea;
        private float _scrollSpeed = 250;

        private void Awake()
        {
            _scrollArea = gameObject.AddComponent<BoxCollider2D>();
            _scrollArea.isTrigger = true;
            Physics.queriesHitTriggers = true;
        }
        
        private void Start()
        {
            _scrollArea.size = new Vector2(300, Screen.height);
            _scrollArea.offset = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
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
                GameObject changeSkinText = gameObject.FindGameObjectInChildren("Canvas Text - Change Skin");
                foreach (Transform childTransform in gameObject.transform)
                {
                    GameObject uiElement = childTransform.gameObject;
                    float clampMin = Screen.height - 120;
                    float clampMax = Screen.height - 120 + SkinSwapperPanel.Y;
                    if ((scroll > 0 && changeSkinText.transform.position.y >= clampMin) || (scroll < 0 && changeSkinText.transform.position.y <= clampMax))
                    {
                        uiElement.transform.position += Vector3.down * scroll * _scrollSpeed;                               
                    }
                }
            }
        }

        private void Log(object message) => Modding.Logger.Log("[Scroller] " + message);
    }
}