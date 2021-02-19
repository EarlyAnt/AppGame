using LitJson;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppGame.Util
{
    public class JsonUtils : IJsonUtils
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();

        public T String2Json<T>(string jsonString)
        {
            T json = JsonConvert.DeserializeObject<T>(jsonString);
            return json;
        }
        public string Json2String(object jsonObject)
        {
            this.settings.NullValueHandling = NullValueHandling.Ignore;
            string jsonStr = JsonConvert.SerializeObject(jsonObject, this.settings);
            return jsonStr;
        }

        public JsonData JsonStr2JsonData(string jsonString)
        {
            JsonData data = JsonMapper.ToObject(jsonString);
            return data;
        }

        public string Dictionary2String(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return "";
            }
            JsonData data = new JsonData();

            foreach (KeyValuePair<string, string> info in dictionary)
            {
                data[info.Key] = info.Value;
            }
            string json = data.ToJson();
            return json;
        }
    }
}