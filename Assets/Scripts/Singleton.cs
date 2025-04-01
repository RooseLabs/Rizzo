using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting) return null;
            if (_instance != null) return _instance;
            _instance = (T)FindAnyObjectByType(typeof(T));
            if (_instance != null) return _instance;
            lock (_lock)
            {
                GameObject singleton = new() {
                    name = typeof(T).Name
                };
                _instance = singleton.AddComponent<T>();
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        _applicationIsQuitting = true;
    }
}