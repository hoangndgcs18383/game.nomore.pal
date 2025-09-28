using SAGE.Framework.Extensions;
using UnityEngine;

namespace NoMorePals
{
    public class QuestTrigger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private string questID;
        [SerializeField] private Transform enterPoint;
        [SerializeField] private GameObject triggerEffect;
        public bool CanTrigger(string id) => id == _slotData.id;

        public bool Triggered() => _triggered;

        private bool _triggered;
        private SlotData _slotData;

        public virtual void Initialize(SlotData slotData)
        {
            _slotData = slotData;
            icon.sprite = slotData.sprite;
            gameObject.GetOrAddComponent<PolygonCollider2D>();
        }

        public virtual void Active()
        {
           // triggerEffect.SetActive(true);
            _triggered = true;
        }

        public virtual void Enter(MagnetBlock block)
        {
            block.transform.position = enterPoint.position;
        }
    }
}