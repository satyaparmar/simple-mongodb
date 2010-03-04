using System;
using System.Runtime.Serialization;
using Newtonsoft.Json2;
using Pls.SimpleMongoDb.Resources;

namespace Pls.SimpleMongoDb
{
    [Serializable]
    public class SimoObjectIdJsonConverter
        : JsonConverter
    {
        private readonly static Type AcceptedType = typeof (SimoObjectId);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var oid = value as SimoObjectId;

            if (SimoObjectId.IsEmpty(oid))
                throw new SerializationException(Exceptions.SimoObjectIdJsonConverter_InvalidId);

// ReSharper disable PossibleNullReferenceException
            writer.WriteOidValue(oid.Value);
// ReSharper restore PossibleNullReferenceException
        }

        public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            var idBytes = serializer.Deserialize<byte[]>(reader);

            return idBytes == null ? null : new SimoObjectId(idBytes);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == AcceptedType;
        }
    }
}