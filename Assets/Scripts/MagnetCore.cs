namespace NoMorePals
{
    using UnityEngine;

    public class MagnetCore : MonoBehaviour
    {
        [SerializeField] private Transform magnetA;
        [SerializeField] private Transform magnetB;

        [SerializeField] private float duration = 1f;
        [SerializeField] private float meetOffset = 0.5f;

        private float _timer = 0f;
        private bool _isMagnet = false;

        private Vector3 _startPosA, _startPosB;
        private Vector3 _targetPosA, _targetPosB;
        
        private void OnBlockStateChanged(bool isSelected)
        {
            if (!isSelected)
            {
                DoMagnetize();
            }
        }

        private void Update()
        {
            if (_isMagnet)
            {
                _timer += Time.deltaTime;
                float t = Mathf.Clamp01(_timer / duration);
                t = t * t * (3f - 2f * t);
                
                magnetA.position = Vector3.Lerp(_startPosA, _targetPosA, t);
                magnetB.position = Vector3.Lerp(_startPosB, _targetPosB, t);

                if (t >= 1f)
                {
                    _isMagnet = false;
                }
            }
        }

        public void DoMagnetize()
        {
            if (magnetA == null || magnetB == null)
            {
                Debug.Log("magnetA or magnetB is null");
                return;
            }
            
            _timer = 0f;
            _isMagnet = true;

            _startPosA = magnetA.position;
            _startPosB = magnetB.position;

            Vector3 midPoint = (magnetA.position + magnetB.position) / 2f;

            Vector3 directionA = (midPoint - magnetA.position).normalized;
            Vector3 directionB = (midPoint - magnetB.position).normalized;

            _targetPosA = midPoint - directionA * meetOffset;
            _targetPosB = midPoint - directionB * meetOffset;
        }
    }
}