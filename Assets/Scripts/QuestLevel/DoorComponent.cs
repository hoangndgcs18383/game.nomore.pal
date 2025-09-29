using System;
using DG.Tweening;
using UnityEngine;

namespace NoMorePals
{
    public class DoorComponent : MonoBehaviour
    {
        // [SerializeField] private DOTweenAnimation doorAnimation;
        [SerializeField] private float rotate = 130f;
        [SerializeField] private GameObject handIcon;
        [SerializeField] private Transform doorTransform;

        public void DoAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            handIcon.SetActive(true);
            sequence.Append(transform.DORotate(new Vector3(0, 0, rotate), 1f).SetEase(Ease.OutQuad));
            sequence.AppendCallback(() => handIcon.SetActive(false));
            sequence.Play();
        }
    }
}