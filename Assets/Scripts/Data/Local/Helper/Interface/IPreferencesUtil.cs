using System;

namespace AppGame.Data.Local
{
    public interface IPreferenceHelper
    {
        T GetObject<T>(string key, object defaultObj);
        T GetObject<T>(string key);

        void SaveObject<T>(string key, T t);

        void DeleteAll();
        void DeleteObject(string key);
    }
}