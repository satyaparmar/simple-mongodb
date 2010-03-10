using System;
using System.Runtime.Serialization;
using Newtonsoft.Json2;
using Pls.SimpleMongoDb.DataTypes;
using Pls.SimpleMongoDb.Resources;

namespace Pls.SimpleMongoDb.Serialization
{
    [Serializable]
    internal class SimoIdJsonConverter
        : JsonConverter
    {
        private readonly static Type AcceptedType = typeof (SimoId);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var oid = value as SimoId;

            if (SimoId.IsEmpty(oid))
                throw new SerializationException(ExceptionMessages.SimoObjectIdJsonConverter_InvalidId);

// ReSharper disable PossibleNullReferenceException
            writer.WriteOidValue(oid.Value);
// ReSharper restore PossibleNullReferenceException
        }

        public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            var idBytes = serializer.Deserialize<byte[]>(reader);

            return idBytes == null ? null : new SimoId(idBytes);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == AcceptedType;
        }
    }
}