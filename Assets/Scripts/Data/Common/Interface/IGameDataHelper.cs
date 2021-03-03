namespace AppGame.Data.Common
{
    public interface IGameDataHelper
    {
        T GetObject<T>(string key);
        T GetObject<T>(string key, object defaultObj);
        void SaveObject<T>(string key, T t);        
        void Clear(string key);
    }
}