using UnityEngine;

namespace NoMorePals
{
    public class MagnetBlock : MonoBehaviour
    {
        [SerializeField] MagnetConfig config;
        [SerializeField] private Transform magnetContact;
        [SerializeField] private bool isMagnetizing = true;

        private float timer;
        private float speedIncreaseRate = 2f;

        public bool IsMagnetizing()
        {
            return isMagnetizing;
        }

        private void Start()
        {
            speedIncreaseRate = config.magnetSpeedDefault;
            timer = 0f;
            TryMagnetToContact();
        }

        private void Update()
        {
            if (isMagnetizing && magnetContact != null && GameManager.Instance.IsPlaying())
            {
                //Vector3 dir = (magnetContact.position - transform.position).normalized;
                //Vector3 targetPos = magnetContact.position - dir * config.stopOffset;
                speedIncreaseRate += Time.deltaTime;
                speedIncreaseRate = Mathf.Min(speedIncreaseRate, config.magnetSpeedMax);

                if (Vector3.Distance(transform.position, magnetContact.position) < config.stopOffset)
                {
                    isMagnetizing = false;
                    speedIncreaseRate = config.magnetSpeedDefault;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        magnetContact.position,
                        speedIncreaseRate * Time.deltaTime
                    );
                }
            }
        }

        public void TryMagnetToContact()
        {
            float dist = Vector3.Distance(transform.position, magnetContact.position);
            if (dist <= config.magnetRange && !isMagnetizing)
            {
                isMagnetizing = true;
                timer = 0f;
            }
        }
    }
}