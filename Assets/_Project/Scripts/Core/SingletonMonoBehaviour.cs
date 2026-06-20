using UnityEngine;

namespace EscapeDays.Core
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static bool HasInstance => _instance != null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError($"[Singleton] {typeof(T).Name} instance not found in scene!");
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            // Mencegah adanya duplikat Manager di scene yang sama
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning($"[Singleton] Duplicate {typeof(T).Name} destroyed on {gameObject.name}.");
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}