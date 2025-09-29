
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
            await UniTask.WaitForSeconds(0.1f);
            SceneManager.LoadScene("Level2");
        }
    }
}