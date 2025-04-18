using UnityEngine;

namespace RooseLabs.Generics
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T s_instance;
        private static readonly object s_lock = new();

        public static T Instance
        {
            get
            {
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
            if (!Application.isPlaying) return;
            if (s_instance == null)
            {
                s_instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (s_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
