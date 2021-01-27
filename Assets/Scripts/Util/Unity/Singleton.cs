using System;
using UnityEngine;

namespace AppGame.Util
{
    public abstract class Singleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    try
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        instance = go.AddComponent<T>();
#if !UNITY_EDITOR
                    DontDestroyOnLoad(go);
#endif
                    }
                    catch (Exception ex)
                    {
                        Debug.LogErrorFormat("<><Singleton.Instance>Error: {0}", ex.Message);
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void OnDestroy()
        {
            Destroy();
        }

        public virtual void Initialize()
        {

        }

        protected virtual void Destroy()
        {
            instance = null;
        }
    }
}
