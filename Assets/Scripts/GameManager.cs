using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SAGE.Framework.Core.Addressable;
using SAGE.Framework.UI;
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
        [SerializeField] private MagnetBlock magnetA;
        [SerializeField] private MagnetBlock magnetB;
        [SerializeField] private StateGame _stateGame = StateGame.Playing;
        [SerializeField] ILevel ILevel;

        private bool _isTurnComplete = false;

        public bool IsPlaying() => _stateGame == StateGame.Playing;

        private async void Start()
        {
            ILevel = GetComponent<ILevel>();
            await ILevel.StartLevel(magnetA, magnetB);
            _stateGame = StateGame.Playing;
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