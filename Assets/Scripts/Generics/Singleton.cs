using UnityEngine;

namespace RooseLabs.Generics
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance;
        private static readonly object s_lock = new();
        private static bool s_applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (s_applicationIsQuitting) return null;
                if (s_instance != null) return s_instance;
                s_instance = (T)FindAnyObjectByType(typeof(T));
                if (s_instance != null) return s_instance;
                lock (s_lock)
                {
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
            if (s_instance == null)
            {
                s_instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            s_applicationIsQuitting = true;
        }
    }
}
