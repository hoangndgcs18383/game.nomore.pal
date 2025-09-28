using System;
using Cysharp.Threading.Tasks;
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
        public event Action<int, int> OnTurnComplete;
        public event Action<StateGame> OnStateGameChanged;

        [SerializeField] private MagnetBlock magnetA;
        [SerializeField] private MagnetBlock magnetB;
        [SerializeField] private StateGame _stateGame = StateGame.Playing;

        private ILevel ILevel;
        private int _turns = 0;

        public bool IsPlaying() => _stateGame == StateGame.Playing;
        public bool IsOutOfTurns() => _stateGame == StateGame.OutOfTurns;
        public bool CanMagnetize() => IsPlaying() || IsOutOfTurns();
        public bool IsWin() =>  ILevel.AreAllQuestsComplete();
        
        private async void Start()
        {
            ILevel = GetComponent<ILevel>();
            _turns = 0;
            await ILevel.StartLevel(magnetA, magnetB);
            OnTurnComplete?.Invoke(_turns, ILevel.GetLevelTurns());
            _stateGame = StateGame.Playing;
        }

        public void SetQuestComplete(string questID)
        {
            ILevel.SetQuestComplete(questID);
        }

        public void CompleteTurn()
        {
            _turns++;
            OnTurnComplete?.Invoke(_turns, ILevel.GetLevelTurns());

            Debug.Log($"Turn {_turns}/{ILevel.GetLevelTurns()} completed.");

            if (_turns >= ILevel.GetLevelTurns())
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
    }
}