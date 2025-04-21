using UnityEngine;

namespace RooseLabs.Generics
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : Component
    {
        private static T s_instance;
        private static readonly object s_lock = new();
        private static bool s_isQuitting;

        public static T Instance
        {
            get
            {
                if (s_isQuitting)
                {
                    Debug.LogWarning($"Instance of {typeof(T)} will not be returned because the application is quitting.");
                    return null;
                }
                lock (s_lock)
                {
                    if (s_instance != null) return s_instance;
                    s_instance = (T)FindAnyObjectByType(typeof(T));
                    if (s_instance != null) return s_instance;
                    GameObject singleton = new() {
                        name = typeof(T).Name
                    };
                    s_instance = singleton.AddComponent<T>();
                    return s_instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }
            s_instance = this as T;
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit()
        {
            s_isQuitting = true;
        }
    }
}
