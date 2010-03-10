using System;
using System.Collections.Generic;
using Newtonsoft.Json2;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.SimpleMongoDb.Serialization
{
    [Serializable]
    internal class SimoReferenceJsonConverter
        : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var r = (SimoReference)value;
            var dic = new Dictionary<string, object>
                      {
                          {"$ref", r.CollectionName},
                          {"$id", r.Id}
                      };

            serializer.Serialize(writer, dic);
        }

        public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            var r = new SimoReference();

            serializer.Populate(reader, r);
            
            return r;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}