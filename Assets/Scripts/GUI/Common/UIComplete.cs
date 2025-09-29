using System;
using DG.Tweening;
using SAGE.Framework.Core.Addressable;
using SAGE.Framework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoMorePals
{
    public struct UICompleteData : IUIData
    {
        public string title;
        public string message;
        public Color color;
        public bool win;
        public Action onRetry;
        public Action onNextLevel;
    }

    public class UIComplete : BaseScreen
    {
        [SerializeField] private CanvasGroup cgGUI;
        [SerializeField] private CanvasGroup cgRetry;
        [SerializeField] private CanvasGroup cgNextLevel;
        [SerializeField] private TMP_Text txtTitle;
        [SerializeField] private TMP_Text txtMessage;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button retryButton;

        private UICompleteData _data;

        public override void Initialize()
        {
            base.Initialize();

            nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
            retryButton.onClick.AddListener(OnRetryButtonClicked);
        }

        public override void SetData(IUIData data = null)
        {
            base.SetData(data);
            if (data is UICompleteData completeData)
            {
                _data = completeData;
            }
        }

        public override void Show()
        {
            base.Show();
            DoAnimationShow();
        }

        private void DoAnimationShow()
        {
            float initialAlpha = 0;
            float targetAlpha = 1f;
            float heightOffset = 200f;
            txtTitle.text = _data.title;
            txtTitle.color = _data.color;
            txtTitle.alpha = initialAlpha;
            txtMessage.text = _data.message;
            txtMessage.alpha = initialAlpha;
            cgGUI.alpha = initialAlpha;
            cgRetry.alpha = initialAlpha;
            cgNextLevel.alpha = initialAlpha;
            cgRetry.blocksRaycasts = false;
            cgNextLevel.blocksRaycasts = false;

            Sequence seq = DOTween.Sequence();
            seq.Append(cgGUI.DOFade(targetAlpha, 0.5f));
            seq.Append(txtTitle.DOFade(targetAlpha, 0.5f));
            seq.Join(txtTitle.transform.DOLocalMoveY(heightOffset, 0.5f).From().SetEase(Ease.OutBack));
            seq.Append(txtMessage.DOFade(targetAlpha, 0.5f));
            seq.Join(txtMessage.transform.DOLocalMoveY(heightOffset, 0.5f).From().SetEase(Ease.OutBack));
            if (GameManager.Instance.GetLevelIndex() == 3 && _data.win)
            {
                seq.AppendCallback(() =>
                {
                    Hide();
                    UIManager.Instance.ShowAndLoadScreenAsync<UIOuttro>(BaseScreenAddress.UIOUTTRO);
                });
            }
            else
            {
                if (_data.win)
                {
                    seq.Append(cgNextLevel.DOFade(targetAlpha, 0.5f));
                    seq.Join(cgNextLevel.transform.DOLocalMoveY(heightOffset, 0.5f).From().SetEase(Ease.OutBack));
                }
                else
                {
                    seq.Append(cgRetry.DOFade(targetAlpha, 0.5f));
                    seq.Join(cgRetry.transform.DOLocalMoveY(heightOffset, 0.5f).From().SetEase(Ease.OutBack));
                }
            }

            seq.Play();
            seq.OnComplete(() =>
            {
                if (_data.win)
                {
                    cgNextLevel.blocksRaycasts = true;
                }
                else
                {
                    cgRetry.blocksRaycasts = true;
                }

                if (GameManager.Instance.GetLevelIndex() == 3 && _data.win) WaitForQuit();
            });
        }

        public async void WaitForQuit()
        {
            await System.Threading.Tasks.Task.Delay(18000);
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
            Application.Quit();
#elif UNITY_WEBGL
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
#endif
        }

        private void OnNextLevelButtonClicked()
        {
            _data.onNextLevel?.Invoke();
            Hide();
        }

        private void OnRetryButtonClicked()
        {
            _data.onRetry?.Invoke();
            Hide();
        }
    }
}