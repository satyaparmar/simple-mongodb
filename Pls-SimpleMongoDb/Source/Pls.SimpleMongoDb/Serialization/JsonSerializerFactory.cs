using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Pls.SimpleMongoDb.Serialization
{
    public static class JsonSerializerFactory
    {
        private static readonly JsonConverterCollection Converters = new JsonConverterCollection();

        public static JsonConverterCollection Add(JsonConverter converter) {
            Converters.Add(converter);
            return Converters;
        }

        public static JsonConverterCollection Add(IEnumerable<JsonConverter> converters) {
            foreach (var converter in converters)
                Converters.Add(converter);
            return Converters;
        }

        public static IContractResolver ContractResolver { get; set; }

        public static JsonSerializer Create() {
            var cvrt = new JsonSerializerSettings {
                Converters = Converters,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = ContractResolver ?? new DefaultContractResolver()
            };

            return JsonSerializer.Create(cvrt);
        }
    }
}
