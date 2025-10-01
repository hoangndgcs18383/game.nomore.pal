using System.Collections.Generic;
using NoMorePals;
using SAGE.Framework.Core.Addressable;
using UnityEngine.SceneManagement;

namespace SAGE.Framework.Core
{
    using Cysharp.Threading.Tasks;
    //using Addressable;
    using SAGE.Framework.Extensions;
    using SAGE.Framework.UI;
    using UnityEngine;

    public class AppManager : BehaviorSingleton<AppManager>
    {
        [SerializeField] private int _targetFrameRate = 60;
        [SerializeField] private BlackBorderController _blackBorderController;

        public async void Start()
        {
            await DOInitialize().AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        }

        public async UniTask DOInitialize()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = _targetFrameRate;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
#else
            Application.targetFrameRate = 300;
#endif
            await UIManager.Instance.DoInitializeAsync();
            AudioManager.Instance.PlayBackgroundMusic("Intro");
#if !UNITY_EDITOR
            await UIManager.Instance.ShowAndLoadScreenAsync<UIIntro>(BaseScreenAddress.UIINTRO);
            await UniTask.WaitForSeconds(17f);
#endif
            await SceneManager.LoadSceneAsync("Level3");
            await UniTask.WaitForSeconds(1f);
            AudioManager.Instance.PlayBackgroundMusic("InGame");
        }
    }
}