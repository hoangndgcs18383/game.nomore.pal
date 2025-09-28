using System;
using UnityEngine;

namespace NoMorePals
{
    public class HandController : MonoBehaviour
    {
        [SerializeField] private MagnetBlock magnetBlock;
        [SerializeField] private Transform rootHand;
        [SerializeField] private Transform targetHand;
        [Range(0f, 1f)] [SerializeField] private float percentMove = 0.5f;
        [Range(0f, 1f)] [SerializeField] private float percentStop = 0.5f;
        [SerializeField] private float speedMove = 10f;

        private void Update()
        {
            //if (!magnetBlock.IsCurrentSelected()) return;
            if (targetHand.gameObject.activeInHierarchy)
            {
                MoveToPointBetween(rootHand.position, targetHand.position,
                    !magnetBlock.IsCurrentSelected() ? percentStop : percentMove);
            }
            else
            {
                Vector3 rootPos = rootHand.position;
                MoveToPointBetween(transform.position, rootPos,
                    Time.deltaTime * speedMove);
            }
        }

        public void MoveToPointBetween(Vector3 pointA, Vector3 pointB, float t)
        {
            Vector3 targetPos = Vector3.Lerp(pointA, pointB, t);
            transform.position = targetPos;
        }
    }
}