using System;
using Newtonsoft.Json2;
using Pls.SimpleMongoDb.Serialization;

namespace Pls.SimpleMongoDb.DataTypes
{
    [Serializable, JsonConverter(typeof(SimoReferenceJsonConverter))]
    public class SimoReference
    {
        [JsonProperty(PropertyName = "$ref")]
        public string CollectionName { get; set; }

        [JsonProperty(PropertyName = "$id")]
        public SimoId Id { get; set; }
    }
}