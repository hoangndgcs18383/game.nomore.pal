using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace NoMorePals
{
    public enum MagnetState
    {
        Idle,
        Walking,
        Drag,
        Crying,
        IsHappyDance
    }

    public class MagnetBlock : MonoBehaviour
    {
        [SerializeField] MagnetConfig config;
        [SerializeField] private Transform magnetContact;
        [SerializeField] private bool isMagnetizing = true;

        private MagnetState currentState = MagnetState.Idle;
        private AnimationHandler animationHandler;

        private float speedIncreaseRate = 2f;
        private bool isCurrentSelected = false;
        private bool hasEnteredLevel1 = false;
        private bool isPushing = false;
        private bool canPush = true;
        private float pullSpeed = 0f;
        private Transform targetTransform;
        private Vector3 pullDir;

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

        public bool IsPushing()
        {
            return isPushing;
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
            TryMagnetToContact();
        }


        public void Selected()
        {
            isCurrentSelected = true;
        }

        public void Drag()
        {
            currentState = MagnetState.Drag;
        }

        public void Deselect()
        {
            isCurrentSelected = false;
        }


        private void Update()
        {
            HandleMovementMagnetize();
            HandleAnimation();
            HandlePush();
        }

        private void HandleAnimation()
        {
            if (animationHandler != null)
            {
                animationHandler.SetBool("IsWalk", currentState == MagnetState.Walking);
                animationHandler.SetBool("IsCry", currentState == MagnetState.Crying);
                animationHandler.SetBool("IsDrag", currentState == MagnetState.Drag);
                animationHandler.SetBool("IsHappyDance", currentState == MagnetState.IsHappyDance);
            }
        }

        private void HandleMovementMagnetize()
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
                }
            }
        }

        private void HandlePush()
        {
            if (!canPush) return;
            if (pullSpeed > 0.01f && isPushing)
            {
                currentState = MagnetState.Drag;
                pullSpeed -= Time.deltaTime * 10f;
                transform.position += pullDir * pullSpeed * Time.deltaTime;
                //Debug.Log("Pushing to " + pullDir + " with speed " + pullSpeed);
            }
            else if (pullSpeed < 0.01f && isPushing)
            {
                if (Vector3.Distance(transform.position, magnetContact.position) < config.stopOffset)
                {
                    currentState = MagnetState.Idle;
                    isPushing = false;
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

        public bool TryToFindDoor()
        {
            /*List<DoorTrigger> allDoors = new List<DoorTrigger>(FindObjectsOfType<DoorTrigger>());
            if (allDoors.Count == 0) return false;
            Vector3 closestDoor = allDoors
                .OrderBy(door => Vector3.Distance(transform.position, door.transform.position))
                .First().transform.position;

            Vector3 dirToClosestDoor = (closestDoor - transform.position).normalized;
            int layer = LayerMask.GetMask("Door") | LayerMask.GetMask("Quest");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToClosestDoor, Mathf.Infinity, layer);

            if (hit.collider != null)
            {
                DoorTrigger door = hit.collider.GetComponent<DoorTrigger>();
                if (door != null)
                {
                    canPush = false;
                    isCurrentSelected = false;
                    targetTransform = door.transform;
                    currentState = MagnetState.Walking;
                    MoveToDoor(door);
                    return true;
                }
            }*/

            for (int i = 0; i < 360; i++)
            {
                float angle = i * 1;
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                int layer = LayerMask.GetMask("Door") | LayerMask.GetMask("Quest");
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 15, layer);

                if (hit.collider != null)
                {
                    DoorTrigger door = hit.collider.GetComponent<DoorTrigger>();
                    if (door != null)
                    {
                        canPush = false;
                        isCurrentSelected = false;
                        targetTransform = door.transform;
                        currentState = MagnetState.Walking;
                        //Debug.Log("CheckCanSeeContact " + CheckCanSeeContact());
                        MoveToDoor(door);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CheckCanSeeContact()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, magnetContact.position - transform.position,
                Vector3.Distance(transform.position, magnetContact.position), LayerMask.GetMask("Block"));

            return hit.collider != null;
        }

        public void MoveToDoor(DoorTrigger doorTrigger)
        {
            Sequence seq = DOTween.Sequence();
            bool isDoAnimation = false;
            seq.Append(transform.DOMove(targetTransform.position, 1f));
            seq.AppendCallback(() => { doorTrigger.doorComponent.DoAnimation(this); });
        }

        public void SetPullPosition(Vector3 dir)
        {
            //if (isPushing) return;
            pullDir = dir;
            pullSpeed = config.pushForce;
            isPushing = true;
        }

        public void TryMagnetToContact()
        {
            float dist = Vector3.Distance(transform.position, magnetContact.position);
            if (dist <= config.magnetRange && !isMagnetizing)
            {
                isMagnetizing = true;
            }
        }

        public IEnumerator ManualMove()
        {
            while (true)
            {
                speedIncreaseRate += Time.deltaTime;
                speedIncreaseRate = Mathf.Min(speedIncreaseRate, config.magnetSpeedMax);

                if (Vector3.Distance(transform.position, magnetContact.position) < config.stopOffset)
                {
                    isMagnetizing = false;
                    Deselect();
                    speedIncreaseRate = config.magnetSpeedDefault;
                    currentState = MagnetState.IsHappyDance;
                    GameManager.Instance.ChangeStateGame(StateGame.Lose);
                    yield break;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        magnetContact.position,
                        speedIncreaseRate * Time.deltaTime
                    );
                }

                yield return null;
            }
        }

        public void SetPushable(bool canPush)
        {
            this.canPush = canPush;
        }

        public void SetState(MagnetState state)
        {
            currentState = state;
        }

        public void OnGameStateChanged(StateGame newState)
        {
            if (newState == StateGame.Validate)
            {
                switch (GameManager.Instance.GetLevelIndex())
                {
                    case 1:
                        HandleCompleteLevel1();
                        break;
                    case 2:
                        HandleCompleteLevel2();
                        break;
                    case 3:
                        HandleCompleteLevel3();
                        break;
                }
            }
        }

        private void HandleCompleteLevel1()
        {
            if (!HasEnteredLevel1())
            {
                currentState = MagnetState.Crying;
                WaitForCompleteLevel();
            }

            if (!GameManager.Instance.IsWin())
            {
                currentState = MagnetState.IsHappyDance;
            }
        }

        private void HandleCompleteLevel2()
        {
            canPush = false;
            currentState = !GameManager.Instance.IsWin() ? MagnetState.IsHappyDance : MagnetState.Crying;
            WaitForCompleteLevel();
        }

        private void HandleCompleteLevel3()
        {
            if (!GameManager.Instance.IsWin())
            {
                StartCoroutine(ManualMove());
            }
            else
            {
                GameManager.Instance.RandomTryFindDoor();
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