using UnityEngine;

namespace NoMorePals
{
    public class QuestTrigger : MonoBehaviour
    {
        [SerializeField] private string questID;
        [SerializeField] private Transform enterPoint;
        [SerializeField] private GameObject triggerEffect;
        public bool CanTrigger(string id) => id == questID;

        public bool Triggered() => _triggered;

        private bool _triggered;

        public virtual void Active()
        {
            triggerEffect.SetActive(true);
            _triggered = true;
        }

        public virtual void Enter(MagnetBlock block)
        {
            block.transform.position = enterPoint.position;
        }
    }
}