using UnityEngine;

namespace Project.Scripts.Tool
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        private static readonly object Lock = new ();

        public static T Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        // 只在场景中查找实例，不创建新实例
                        //_instance = FindObjectOfType<T>();
                        _instance = FindAnyObjectByType<T>();
                        if (_instance == null)
                        {
                            var go = new GameObject(typeof(T).Name);
                            _instance = go.AddComponent<T>();
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            lock (Lock)
            {
                if (_instance == null)
                {
                    _instance = (T)this;
                    DontDestroyOnLoad(gameObject);
                }
                else if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
        private void OnApplicationQuit()
        {
            _instance = null;
        }
    }
}