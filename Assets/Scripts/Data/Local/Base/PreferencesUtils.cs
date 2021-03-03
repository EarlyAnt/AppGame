using AppGame.Util;
using UnityEngine;

namespace AppGame.Data.Local
{
    public class PreferenceUtils : IPreferencesUtils
    {
        [Inject]
        public IJsonUtil JsonUtils { get; set; }

        public T GetObject<T>(string key, object objDefault)
        {
            if (typeof(T).Name == typeof(string).Name)
            {
                object info = (object)PlayerPrefs.GetString(key: key, defaultValue: (string)objDefault);
                return (T)info;
            }
            else if (typeof(T).Name == typeof(float).Name)
            {
                object info = (object)PlayerPrefs.GetFloat(key: key, defaultValue: (float)objDefault);
                return (T)info;
            }
            else if (typeof(T).Name == typeof(int).Name)
            {
                object info = (object)PlayerPrefs.GetInt(key: key, defaultValue: (int)objDefault);
                return (T)info;
            }
            else if (typeof(T).Name == typeof(bool).Name)
            {
                int info = PlayerPrefs.GetInt(key + "bool", defaultValue: ((bool)objDefault) ? 1 : 0);
                bool result;

                if (info == 1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                return (T)((object)result);
            }
            else
            {
                string info = PlayerPrefs.GetString(key, "");
                if ("".Equals(info))
                {
                    return default(T);
                }
                T obj = JsonUtils.String2Json<T>(info);

                return obj;
            }

        }
        public T GetObject<T>(string key)
        {
            if (typeof(T).Name == typeof(string).Name)
            {
                object info = (object)PlayerPrefs.GetString(key: key, defaultValue: "");
                return (T)info;
            }
            else if (typeof(T).Name == typeof(float).Name)
            {
                object info = (object)PlayerPrefs.GetFloat(key: key, defaultValue: 0);
                return (T)info;
            }
            else if (typeof(T).Name == typeof(int).Name)
            {
                object info = (object)PlayerPrefs.GetInt(key: key, defaultValue: 0);
                return (T)info;
            }
            else if (typeof(T).Name == typeof(bool).Name)
            {
                int info = PlayerPrefs.GetInt(key + "bool", defaultValue: 0);
                bool result;

                if (info == 1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                return (T)((object)result);
            }
            else
            {
                string info = PlayerPrefs.GetString(key, "");
                if ("".Equals(info))
                {
                    return default(T);
                }
                T obj = JsonUtils.String2Json<T>(info);

                return obj;
            }

        }

        public void SaveObject<T>(string key, T t)
        {
            if (t == null)
            {
                return;
            }
            if (typeof(T).Name == typeof(string).Name)
            {
                object result = t;
                PlayerPrefs.SetString(key, (string)result);
            }
            else if (typeof(T).Name == typeof(float).Name)
            {
                object result = t;
                PlayerPrefs.SetFloat(key, (float)result);
            }
            else if (typeof(T).Name == typeof(int).Name)
            {
                object result = t;
                PlayerPrefs.SetInt(key, (int)result);
            }
            else if (typeof(T).Name == typeof(bool).Name)
            {
                bool status = (bool)((object)t);
                int saveResult;

                if (status)
                {
                    saveResult = 1;
                }
                else
                {
                    saveResult = 0;
                }
                PlayerPrefs.SetInt(key + "bool", saveResult);
            }
            else
            {
                string info = JsonUtils.Json2String(t);
                PlayerPrefs.SetString(key, info);
            }
            PlayerPrefs.Save();
        }

        public void DeleteAll()
        {
            Debug.LogWarning("<><PreferenceUtils.DeleteAll>+ + + + +");
            PlayerPrefs.DeleteAll();
        }
        public void DeleteObject(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}