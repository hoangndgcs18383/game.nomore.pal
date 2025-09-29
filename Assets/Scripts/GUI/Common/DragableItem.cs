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
        private QuestTrigger _dragIconDrag;
        protected bool _isDragging;

        protected CanvasGroup _canvasGroup;
        protected SlotData _data;
        private Camera _main;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _parentCanvas = GetComponentInParent<Canvas>();
            _main = Camera.main;
            GameManager.Instance.OnStateGameChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(StateGame state)
        {
            if (state == StateGame.OutOfTurns)
            {
                _canvasGroup.blocksRaycasts = false;
            }
        }

        public virtual void SetData(SlotData data)
        {
            _data = data;
            gameObject.name = data.id;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_parentCanvas == null)
                return;

            _isDragging = true;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            if (_dragIconDrag)
            {
                _dragIconDrag.Drag();
                Vector3 screenPos = _main.ScreenToWorldPoint(mousePos);

                _dragIconDrag.Move(screenPos);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1;
            _isDragging = false;
            Ray ray = _main.ScreenPointToRay(Input.mousePosition);
            if (Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("PointUp")))
            {
                GameManager.Instance.CompleteTurn();
                if (_dragIconDrag) _dragIconDrag.Active();
                OnPointUp();
            }
            else
            {
                Destroy(_dragIconDrag.gameObject);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPosition = _rectTransform.position;
            _originalParent = transform.parent;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.6f;
            _isDragging = true;

            SpawnDragIconWorld();
            //transform.SetParent(_parentCanvas.transform);
        }

        public virtual void OnPointUp()
        {
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