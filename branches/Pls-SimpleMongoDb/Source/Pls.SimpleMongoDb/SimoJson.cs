using System;
using System.Collections.Generic;
using Newtonsoft.Json2;

namespace Pls.SimpleMongoDb
{
    [Serializable]
    public class SimoJson
        : Dictionary<string, object>
    {
        public SimoJson(string json)
        {
            var keyValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
          
            foreach (var keyValue in keyValues)
            {
                Add(keyValue.Key, keyValue.Value);
            }
        }

        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        public override string ToString()
        {
            return ToJson();
        }
        
        public static implicit operator string(SimoJson simoJson)
        {
            return simoJson.ToJson();
        }

        public static implicit operator SimoJson(string json)
        {
            return new SimoJson(json);
        }
    }
}