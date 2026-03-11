using UnityEngine;

namespace FriedEggs.Singletons
{
    public class RegulatorSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance;

        public static bool HasInstance => Instance != null;

        public float InitializationTime { get; private set; }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();
                    if (_instance == null)
                    {
                        var go = new GameObject(typeof(T).Name + " Auto-Generated")
                        {
                            hideFlags = HideFlags.HideAndDontSave
                        };
                        _instance = go.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;
            InitializationTime = Time.time;
            DontDestroyOnLoad(gameObject);

            T[] oldInstances = FindObjectsByType<T>(FindObjectsSortMode.None);
            foreach (T old in oldInstances)
            {
                if (old.GetComponent<RegulatorSingleton<T>>().InitializationTime < InitializationTime)
                {
                    Destroy(old.gameObject);
                }
            }


            if (_instance == null)
            {
                _instance = this as T;
            }
        }
    }
}