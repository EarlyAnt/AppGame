using Newtonsoft.Json;

namespace AppGame.Util
{
    public interface IJsonUtils
    {
        T String2Json<T>(string jsonStr);

        string Json2String(object json);
    }
}