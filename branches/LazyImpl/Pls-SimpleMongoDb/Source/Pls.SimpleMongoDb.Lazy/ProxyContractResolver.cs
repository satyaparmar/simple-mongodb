using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Pls.SimpleMongoDb
{

    /// <summary>
    /// Contact resolver for Proxied entities.
    /// </summary>
    public class ProxyContractResolver : DefaultContractResolver
    {
        private readonly TypeRegistry _typeRegistry;

        public ProxyContractResolver(TypeRegistry typeRegistry) {
            _typeRegistry = typeRegistry;
        }

        protected override JsonProperty CreateProperty(JsonObjectContract contract, MemberInfo member) {
            var prop = member as PropertyInfo;

            if (prop == null || !_typeRegistry.IsTypeRegistered(prop.PropertyType))
                return base.CreateProperty(contract, member);

            var converter = new PropertyReferenceConverver(_typeRegistry, member);

            var property = new JsonProperty {
                ValueProvider = CreateMemberValueProvider(member),
                DefaultValue = null,
                PropertyType = prop.PropertyType,
                Converter = converter,
                MemberConverter = converter,
                PropertyName = ResolvePropertyName(member.Name),
                Required = Required.Default,
                Readable = prop.CanRead,
                Writable = prop.CanWrite
            };

            return property;
        }

        protected override JsonContract CreateContract(Type objectType) {
            // For proxied entities we must return an object contract.
            // Otherwise perform the starndar contract creation.
            return typeof(IProxiedEntity).IsAssignableFrom(objectType) ? CreateObjectContract(objectType) : base.CreateContract(objectType);
        }

    }
}