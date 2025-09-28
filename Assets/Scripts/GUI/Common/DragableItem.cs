using UnityEngine;
using UnityEngine.EventSystems;

namespace NoMorePals
{
    public abstract class DraggableItem : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        private Vector3 _startPosition;
        private Transform _originalParent;
        private Canvas _parentCanvas;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _parentCanvas = GetComponentInParent<Canvas>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_parentCanvas == null)
                return;

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint);
            _rectTransform.localPosition = localPoint;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            if (_originalParent != null)
            {
                transform.SetParent(_originalParent);
            }

            _rectTransform.anchoredPosition = Vector2.zero;
            CheckRaycastWorldCurrentMouse();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPosition = _rectTransform.position;
            _originalParent = transform.parent;
            transform.SetParent(_parentCanvas.transform);
            _canvasGroup.blocksRaycasts = false;
        }

        public void CheckRaycastWorldCurrentMouse()
        {
            Camera cam = Camera.main;
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Quest"));

            if (hit.collider != null)
            {
                TriggerEvent(GetQuestID(), hit.collider);
            }
        }

        public abstract string GetQuestID();
        public abstract bool IsValid();

        public virtual bool TriggerEvent(string questID, Collider2D collider = null)
        {
            if (collider == null) return false;
            QuestTrigger trigger = collider.GetComponent<QuestTrigger>();
            
            if (trigger != null && trigger.CanTrigger(questID))
            {
                trigger.Active();
                gameObject.SetActive(IsValid());
                return true;
            }
            
            return false;
        }
    }
}