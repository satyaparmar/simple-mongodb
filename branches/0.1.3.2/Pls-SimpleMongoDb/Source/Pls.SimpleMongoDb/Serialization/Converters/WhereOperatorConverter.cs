using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pls.SimpleMongoDb.Operators;

namespace Pls.SimpleMongoDb.Serialization.Converters
{
    [Serializable]
    public class WhereOperatorConverter
        : JsonConverter
    {
        private static readonly Type SupportedType = typeof(WhereOperator);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var op = (WhereOperator)value;

            var operatorDictionary = new Dictionary<string, string>
                                     {
                                         {op.Key, op.Expression}
                                     };
            serializer.Serialize(writer, operatorDictionary);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(SupportedType);
        }
    }
}