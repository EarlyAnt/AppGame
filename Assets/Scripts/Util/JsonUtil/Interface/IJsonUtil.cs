using LitJson;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppGame.Util
{
    public interface IJsonUtil
    {
        T String2Json<T>(string jsonString);
        string Json2String(object jsonObject);

        JsonData JsonStr2JsonData(string jsonString);

        string Dictionary2String(Dictionary<string, string> dictionary);
    }
}