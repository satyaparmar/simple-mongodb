using System;
using System.Collections.Generic;
using Newtonsoft.Json2;

namespace Pls.SimpleMongoDb.DataTypes
{
    [Serializable]
    public class SimoJson
    {
        public string Json { get; private set; }

        public SimoJson(string json)
        {
            Json = json;
        }

        public IDictionary<string, object> ToKeyValue()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(Json);
        }

        public override string ToString()
        {
            return Json;
        }

        public static implicit operator string(SimoJson simoJson)
        {
            return simoJson.Json;
        }

        public static implicit operator SimoJson(string json)
        {
            return new SimoJson(json);
        }

        public static implicit operator SimoJson(Dictionary<string, object> keyValueJson)
        {
            var s = new JsonSerializer();
            return JsonConvert.SerializeObject(keyValueJson, Formatting.None);
        }
    }
}