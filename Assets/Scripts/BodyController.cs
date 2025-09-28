using UnityEngine;

namespace NoMorePals
{
    public class BodyController : MonoBehaviour
    {
        [SerializeField] private Transform rootHand;
        [SerializeField] private Transform targetHand;
        [SerializeField] private bool isReverse;

        private void Update()
        {
            Vector3 dir = targetHand.position - rootHand.position;
            if (isReverse) dir = -dir;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rootHand.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}