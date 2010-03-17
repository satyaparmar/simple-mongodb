using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.SimpleMongoDb.Serialization
{
    [Serializable]
    internal class SimoReferenceJsonConverter
        : JsonConverter
    {
        private static readonly Type SupportedType = typeof (SimoReference);

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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var r = new SimoReference();

            serializer.Populate(reader, r);

            return r;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(SupportedType);
        }
    }
}