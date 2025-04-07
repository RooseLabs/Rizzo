using UnityEngine;

namespace RooseLabs.Generics
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        private static readonly object m_lock = new();
        private static bool m_applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (m_applicationIsQuitting) return null;
                if (m_instance != null) return m_instance;
                m_instance = (T)FindAnyObjectByType(typeof(T));
                if (m_instance != null) return m_instance;
                lock (m_lock)
                {
                    GameObject singleton = new() {
                        name = typeof(T).Name
                    };
                    m_instance = singleton.AddComponent<T>();
                    return m_instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            m_applicationIsQuitting = true;
        }
    }
}