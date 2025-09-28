using Cysharp.Threading.Tasks;
using SAGE.Framework.Core.Addressable;
using SAGE.Framework.UI;
using UnityEngine;

namespace NoMorePals
{
    public enum MagnetState
    {
        Idle,
        Walking,
        Crying
    }

    public class MagnetBlock : MonoBehaviour
    {
        [SerializeField] MagnetConfig config;
        [SerializeField] private Transform magnetContact;
        [SerializeField] private bool isMagnetizing = true;

        private MagnetState currentState = MagnetState.Idle;
        private AnimationHandler animationHandler;

        private float timer;
        private float speedIncreaseRate = 2f;
        private bool isCurrentSelected = false;
        private bool hasEnteredLevel1 = false;

        public bool HasEnteredLevel1()
        {
            return hasEnteredLevel1;
        }

        public bool IsCurrentSelected()
        {
            return isCurrentSelected;
        }

        public bool IsMagnetizing()
        {
            return isMagnetizing;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            GameManager.Instance.OnStateGameChanged += OnGameStateChanged;
            animationHandler = GetComponentInChildren<AnimationHandler>();
            speedIncreaseRate = config.magnetSpeedDefault;
            timer = 0f;
            TryMagnetToContact();
        }

        public void Selected()
        {
            isCurrentSelected = true;
        }

        public void Deselect()
        {
            isCurrentSelected = false;
        }


        private void Update()
        {
            HandleMovement();
            HandleAnimation();
        }

        private void HandleAnimation()
        {
            if (animationHandler != null)
            {
                animationHandler.SetBool("IsWalk", currentState == MagnetState.Walking);
                animationHandler.SetBool("IsCry", currentState == MagnetState.Crying);
            }
        }

        private void HandleMovement()
        {
            if (isMagnetizing && magnetContact != null && (GameManager.Instance.CanMagnetize()))
            {
                speedIncreaseRate += Time.deltaTime;
                speedIncreaseRate = Mathf.Min(speedIncreaseRate, config.magnetSpeedMax);

                if (Vector3.Distance(transform.position, magnetContact.position) < config.stopOffset)
                {
                    isMagnetizing = false;
                    Deselect();
                    speedIncreaseRate = config.magnetSpeedDefault;
                    currentState = MagnetState.Idle;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        magnetContact.position,
                        speedIncreaseRate * Time.deltaTime
                    );
                    currentState = MagnetState.Walking;
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

        public void OnGameStateChanged(StateGame newState)
        {
            if (newState == StateGame.Validate)
            {
                if (!HasEnteredLevel1())
                {
                    currentState = MagnetState.Crying;
                    WaitForCompleteLevel();
                }
            }
        }

        public async void WaitForCompleteLevel()
        {
            await UniTask.Delay(2000);
            GameManager.Instance.CheckWin();
        }

        public void EnterLevel1()
        {
            hasEnteredLevel1 = true;
        }
    }
}