using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoMorePals
{
    public class UseBarrier : QuestTrigger
    {
        public List<MagnetBlock> allMagnetBlocks = new List<MagnetBlock>();

        public override void Initialize(SlotData slotData)
        {
            _slotData = slotData;
            transform.position = Vector3.zero;
            allMagnetBlocks = new List<MagnetBlock>(FindObjectsOfType<MagnetBlock>());
        }

        public override void Drag()
        {
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