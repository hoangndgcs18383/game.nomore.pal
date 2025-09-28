using UnityEngine;
using UnityEngine.EventSystems;

namespace NoMorePals
{
    public class DraggableItem : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        //[SerializeField] private QuestTrigger dragIconWorld;
        
        private Vector3 _startPosition;
        private Transform _originalParent;
        private Canvas _parentCanvas;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private QuestTrigger _dragIconDrag;
        
        protected SlotData _data; 

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _parentCanvas = GetComponentInParent<Canvas>();
        }

        public void SetData(SlotData data)
        {
            _data = data;
            gameObject.name = data.id;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_parentCanvas == null)
                return;

            /*Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint);
            _rectTransform.localPosition = localPoint;*/
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            if (_dragIconDrag) _dragIconDrag.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 0f;
            GameManager.Instance.CompleteTurn();
            if (_dragIconDrag) _dragIconDrag.Active();
            /*if (_originalParent != null)
            {
                transform.SetParent(_originalParent);
                _rectTransform.anchoredPosition = Vector2.zero;
            }*/
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPosition = _rectTransform.position;
            _originalParent = transform.parent;
            //transform.SetParent(_parentCanvas.transform);
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.6f;
            SpawnDragIconWorld();
        }

        public void SpawnDragIconWorld()
        {
            if (_data.questTrigger != null)
            {
                _dragIconDrag = Instantiate(_data.questTrigger, Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    Quaternion.identity);
                _dragIconDrag.Initialize(_data);
            }
        }
    }
}