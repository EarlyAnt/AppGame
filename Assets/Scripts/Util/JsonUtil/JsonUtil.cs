using Newtonsoft.Json;

namespace AppGame.Util
{
    public class JsonUtils : IJsonUtils
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();

        public T String2Json<T>(string jsonStr)
        {
            T json = JsonConvert.DeserializeObject<T>(jsonStr);
            return json;
        }

        public string Json2String(object json)
        {
            this.settings.NullValueHandling = NullValueHandling.Ignore;
            string jsonStr = JsonConvert.SerializeObject(json, this.settings);
            return jsonStr;
        }
    }
}