using System;
using UnityEngine;

namespace NoMorePals
{
    public enum StateGame
    {
        Playing,
        Win,
        Lose
    }

    public class GameManager : PresSingleton<GameManager>
    {
        public event Action<int, int> OnTurnComplete;

        [SerializeField] private MagnetBlock magnetA;
        [SerializeField] private MagnetBlock magnetB;
        [SerializeField] private StateGame _stateGame = StateGame.Playing;

        private ILevel ILevel;
        private int _turns = 0;

        public bool IsPlaying() => _stateGame == StateGame.Playing;

        private async void Start()
        {
            ILevel = GetComponent<ILevel>();
            _turns = 0;
            await ILevel.StartLevel(magnetA, magnetB);
            CompleteTurn();
            _stateGame = StateGame.Playing;
        }
        
        public void SetQuestComplete(string questID)
        {
            ILevel.SetQuestComplete(questID);
        }

        public void CompleteTurn()
        {
            OnTurnComplete?.Invoke(_turns++, ILevel.GetLevelTurns());
            
            if (_turns > ILevel.GetLevelTurns())
            {
                ILevel.EndLevel(_turns, ILevel.GetLevelTurns());
            }
        }

        public void ChangeStateGame(StateGame newState)
        {
            if (_stateGame != StateGame.Playing) return;

            _stateGame = newState;
            switch (_stateGame)
            {
                case StateGame.Win:
                    Debug.Log("You Win!");
                    break;
                case StateGame.Lose:
                    Debug.Log("You Lose!");
                    break;
                default:
                    break;
            }
        }
    }
}