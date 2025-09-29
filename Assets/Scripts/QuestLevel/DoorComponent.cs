using System;
using DG.Tweening;
using UnityEngine;

namespace NoMorePals
{
    public class DoorComponent : MonoBehaviour
    {
        // [SerializeField] private DOTweenAnimation doorAnimation;
        public string DoorId;
        public Transform doorTransform;
        public Transform triggerTransform;
        public Transform spawnTeleport;
        public bool isReversed = false;
        [SerializeField] private DoorComponent contactDoor;

        [SerializeField] private float rotate = 130f;
        [SerializeField] private GameObject handIcon;

        public void DoAnimation(MagnetBlock block)
        {
            Vector3 spawnTelePos = Camera.main.WorldToScreenPoint(contactDoor.spawnTeleport.position);
            spawnTelePos.z = 0;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(spawnTelePos);
            worldPos.z = 0;

            Sequence sequence = DOTween.Sequence();
            handIcon.SetActive(true);
            sequence.Append(doorTransform.DORotate(new Vector3(0, 0, rotate), 1f).SetEase(Ease.OutQuad));
            sequence.AppendCallback(() => handIcon.SetActive(false));
            sequence.Append(block.transform.DOMoveY(isReversed ? -2f : 2f, 1f).SetRelative(true));
            sequence.Join(doorTransform.DORotate(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutQuad));
            sequence.AppendCallback(() =>
                block.transform.position = new Vector3(worldPos.x, worldPos.y, block.transform.position.z));
            sequence.Append(contactDoor.doorTransform.DORotate(new Vector3(0, 0, -rotate), 1f).SetEase(Ease.OutQuad));
            sequence.Append(block.transform.DOMoveY(isReversed ? -2f : 2f, 1f).SetRelative(true));
            sequence.Join(contactDoor.doorTransform.DORotate(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutQuad));
            sequence.AppendCallback(() =>
            {
                GameManager.Instance.IECompleteLevel3(!block.CheckCanSeeContact());
            });
            sequence.Play();
        }
    }
}