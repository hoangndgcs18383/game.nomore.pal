using UnityEngine;

namespace NoMorePals
{
    public class InputManager : AdvanceSingleton<InputManager>
    {
        [SerializeField] private LayerMask questLayer;

        private MagnetBlock currentSelectedBlock;
        private Camera _main;
        private bool _isDragging = false;
        private bool _isMagnet = true;
        private Transform _draggedObject;

        private void Start()
        {
            _main = Camera.main;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsPlaying()) return;

            OnBlockSelected();

            OnBlockDragging();

            OnBlockDeselect();
        }

        private void OnBlockDeselect()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                if (currentSelectedBlock)
                {
                    RaycastHit2D hit = Physics2D.Raycast(currentSelectedBlock.transform.position, Vector2.zero
                        , Mathf.Infinity, questLayer);
                    if (hit.collider != null)
                    {
                        QuestTrigger trigger = hit.collider.GetComponent<QuestTrigger>();
                        Debug.Log("Hit: " + hit.collider.name);
                        if (trigger != null && trigger.Triggered())
                        {
                            trigger.Enter(currentSelectedBlock);
                            GameManager.Instance.ChangeStateGame(StateGame.Win);
                        }
                        else
                        {
                            currentSelectedBlock.TryMagnetToContact();
                        }
                    }
                    else
                    {
                        currentSelectedBlock.TryMagnetToContact();
                    }
                }

                currentSelectedBlock = null;
                _draggedObject = null;
            }
        }

        private void OnBlockDragging()
        {
            if (_isDragging && currentSelectedBlock != null)
            {
                Ray ray = _main.ScreenPointToRay(Input.mousePosition);
                Vector3 worldPoint = ray.origin;
                _draggedObject.position =
                    new Vector3(worldPoint.x, worldPoint.y, currentSelectedBlock.transform.position.z);
            }
        }

        private void OnBlockSelected()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _main.ScreenPointToRay(Input.mousePosition);

                if (Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity))
                {
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                    if (hit.collider != null)
                    {
                        MagnetBlock block = hit.collider.GetComponent<MagnetBlock>();
                        if (block != null)
                        {
                            currentSelectedBlock = block;
                            _draggedObject = currentSelectedBlock.transform;
                            _isDragging = true;
                        }
                    }
                }
            }
        }
    }
}