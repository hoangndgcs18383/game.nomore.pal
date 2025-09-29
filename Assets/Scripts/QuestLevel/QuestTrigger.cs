using SAGE.Framework.Extensions;
using UnityEngine;

namespace NoMorePals
{
    public class QuestTrigger : MonoBehaviour
    {
        [SerializeField] private bool freezeY;
        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private GameObject animtion;
        public bool CanTrigger(string id) => id == _slotData.id;

        public bool Triggered() => _triggered;

        private bool _triggered;
        protected SlotData _slotData;
        private Vector3 _initialPosition;

        public virtual void Initialize(SlotData slotData)
        {
            _slotData = slotData;
            icon.sprite = slotData.sprite;
            _initialPosition = transform.position;
            gameObject.GetOrAddComponent<PolygonCollider2D>();
        }

        public void Move(Vector3 position)
        {
            if (freezeY)
            {
                position.y = _initialPosition.y;
            }
            transform.position = position;
        }
        
        public virtual void Drag()
        {
        }

        public virtual void Active()
        {
            // triggerEffect.SetActive(true);
            _triggered = true;
        }

        public virtual void Enter(MagnetBlock block)
        {
            block.gameObject.SetActive(false);
            if(animtion) animtion.gameObject.SetActive(true);
        }
    }
}