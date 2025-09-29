using System;
using SAGE.Framework.Core.Addressable;
using SAGE.Framework.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoMorePals
{
    public enum StateGame
    {
        Playing,
        OutOfTurns,
        Validate,
        Win,
        Lose
    }

    public class GameManager : PresSingleton<GameManager>
    {
        public event Action<int, ILevel> OnTurnChanged;
        public event Action<StateGame> OnStateGameChanged;

        [SerializeField] private MagnetBlock magnetA;
        [SerializeField] private MagnetBlock magnetB;
        [SerializeField] private StateGame _stateGame = StateGame.Playing;

        private ILevel ILevel;
        private int _turns = 0;
        private bool _isRandomTryFindDoor = false;

        public bool IsPlaying() => _stateGame == StateGame.Playing;
        public bool IsOutOfTurns() => _stateGame == StateGame.OutOfTurns;
        public bool CanMagnetize() => IsPlaying() || IsOutOfTurns();
        public bool IsWin() => ILevel.AreAllQuestsComplete();
        public int GetLevelIndex() => ILevel.LevelIndex;

        private async void Start()
        {
            ILevel = GetComponent<ILevel>();
            _turns = ILevel.GetLevelTurns();
            _isRandomTryFindDoor = false;
            await ILevel.StartLevel(magnetA, magnetB);
            OnTurnChanged?.Invoke(_turns, ILevel);
            _stateGame = StateGame.Playing;
        }

        public void SetQuestComplete(string questID)
        {
            ILevel.SetQuestComplete(questID);
        }

        public void CompleteTurn()
        {
            _turns--;
            OnTurnChanged?.Invoke(_turns, ILevel);

            Debug.Log($"Turn {_turns}/{ILevel.GetLevelTurns()} completed.");

            if (_turns <= 0)
            {
                ChangeStateGame(StateGame.OutOfTurns);
            }
        }

        public void ValidateGameplay()
        {
            ChangeStateGame(StateGame.Validate);
        }

        public void CheckWin()
        {
            ChangeStateGame(IsWin() ? StateGame.Win : StateGame.Lose);
        }

        public void ChangeStateGame(StateGame newState)
        {
            if (_stateGame == newState) return;
            _stateGame = newState;
            OnStateGameChanged?.Invoke(_stateGame);

            switch (_stateGame)
            {
                case StateGame.Win:
                    Debug.Log("You Win!");
                    ShowWin();
                    break;
                case StateGame.Lose:
                    Debug.Log("You Lose!");
                    ShowLose();
                    break;
                default:
                    break;
            }
        }

        public async void ShowWin()
        {
            UICompleteData data = new UICompleteData
            {
                title = "Level Complete", message = "Congratulations! You've completed the level.",
                color = Color.green, win = true, onNextLevel = LoadNextLevel, onRetry = ResetGame
            };
            await UIManager.Instance.ShowAndLoadScreenAsync<UIComplete>(BaseScreenAddress.UICOMPLETE, data);
        }

        public async void ShowLose()
        {
            UICompleteData loseData = new UICompleteData
            {
                title = "Level Failed", message = "Try again! You didn't complete all the quests.",
                color = Color.red, win = false, onNextLevel = LoadNextLevel, onRetry = ResetGame
            };
            await UIManager.Instance.ShowAndLoadScreenAsync<UIComplete>(BaseScreenAddress.UICOMPLETE, loseData);
        }

        public void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void RandomTryFindDoor()
        {
            if (_isRandomTryFindDoor) return;

            _isRandomTryFindDoor = true;
            
            if (magnetA.TryToFindDoor())
            {
                magnetB.SetPushable(false);
                magnetB.SetState(MagnetState.Idle);
                return;
            }

            if (magnetB.TryToFindDoor())
            {
                magnetA.SetPushable(false);
                magnetA.SetState(MagnetState.Idle);
                return;
            }
            
            int random = UnityEngine.Random.Range(0, 100);

            if (random < 50)
            {
                magnetA.TryToFindDoor();
                magnetB.SetPushable(false);
                magnetB.SetState(MagnetState.Idle);
            }
            else
            {
                magnetA.SetPushable(false);
                magnetA.SetState(MagnetState.Idle);
                magnetB.TryToFindDoor();
            }
        }

        public void IECompleteLevel3(bool isWin = true)
        {
            if (isWin)
            {
                magnetA.SetState(MagnetState.Crying);
                magnetB.SetState(MagnetState.Crying);
                magnetB.WaitForCompleteLevel();
            }
            else
            {
                StartCoroutine(magnetA.ManualMove());
                StartCoroutine(magnetB.ManualMove());
            }
        }
        
    }
}