using UnityEngine;

namespace NoMorePals
{
    public class PresSingleton<T> : MonoBehaviour where T : PresSingleton<T>
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public class AdvanceSingleton<T> : PresSingleton<T> where T : AdvanceSingleton<T>
    {
        protected override void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public class Singleton<T> where T : Singleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = System.Activator.CreateInstance<T>();
                    _instance.Initialize();
                }

                return _instance;
            }
            set => _instance = value;
        }

        public virtual void Initialize()
        {
        }
    }
}