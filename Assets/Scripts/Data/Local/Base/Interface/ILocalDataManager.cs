using System;

namespace AppGame.Data.Local
{
    public interface ILocalDataManager
    {
        void SaveObject<T>(string key, T t);
        void RemoveObject(string key);

        T GetObject<T>(string key, object defaultObj);
        T GetObject<T>(string key);

        void DeleteAll();
        void DeleteAll(bool isRealDeleteAll);
    }
}