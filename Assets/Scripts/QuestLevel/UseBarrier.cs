using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoMorePals
{
    public class UseBarrier : QuestTrigger
    {
        public List<MagnetBlock> allMagnetBlocks = new List<MagnetBlock>();

        private bool canRotate = true;
        private Vector3 currentRotation = Vector3.zero;

        public override void Initialize(SlotData slotData)
        {
            _slotData = slotData;
            transform.position = Vector3.zero;
            allMagnetBlocks = new List<MagnetBlock>(FindObjectsOfType<MagnetBlock>());
            canRotate = GameManager.Instance.GetLevelIndex() == 3;
            _isDragging = true;
            
        }

        public override void Drag()
        {
        }

        private void Update()
        {
            if (canRotate && _isDragging)
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_WEBGL
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    currentRotation.z += 100 * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    currentRotation.z -= 100 * Time.deltaTime;
                }
#elif UNITY_IOS || UNITY_ANDROID
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Moved)
                    {
                        currentRotation.z += touch.deltaPosition.x * 0.5f * Time.deltaTime;
                    }
                }
#endif
                transform.rotation = Quaternion.Euler(currentRotation);
            }
        }

        public override void Active()
        {
            base.Active();
            if (allMagnetBlocks.All(b => b.IsPushing())) GameManager.Instance.SetQuestComplete(Constants.UseBarrier);
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && col.gameObject.layer == LayerMask.NameToLayer("Block"))
            {
                Rigidbody2D rb = col.attachedRigidbody;
                if (rb != null)
                {
                    Vector3 pushDirection = col.transform.position - transform.position;
                    MagnetBlock block = col.GetComponentInParent<MagnetBlock>();
                    if (block)
                    {
                        block.SetPullPosition(pushDirection);
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && col.gameObject.layer == LayerMask.NameToLayer("Block"))
            {
                Rigidbody2D rb = col.attachedRigidbody;
                if (rb != null)
                {
                    Vector3 pushDirection = col.transform.position - transform.position;
                    MagnetBlock block = col.GetComponentInParent<MagnetBlock>();
                    if (block)
                    {
                        block.SetPullPosition(pushDirection);
                    }
                }
            }
        }
    }
}