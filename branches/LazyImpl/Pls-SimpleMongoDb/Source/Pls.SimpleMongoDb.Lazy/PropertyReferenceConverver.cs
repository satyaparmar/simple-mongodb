using System;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace Pls.SimpleMongoDb
{
    /// <summary>
    /// Performs the de/serialization of property memebers
    /// </summary>
    public class PropertyReferenceConverver : JsonConverter
    {
        private readonly TypeRegistry _typeRegistry;
        private readonly MemberInfo _memberInfo;
        private readonly TypeInfo _metaInfo;

        public PropertyReferenceConverver(TypeRegistry typeRegistry, MemberInfo memberInfo) {
            _typeRegistry = typeRegistry;
            _memberInfo = memberInfo;
            var prop = (PropertyInfo)memberInfo;
            _metaInfo = _typeRegistry.GetMeta(prop.PropertyType);
        }

        /// <summary>
        /// Write the relation as an <see cref="EntityReference"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (value == null)
                return;

            var type = _typeRegistry.GetMeta(value.GetType());

            var dic = new EntityReference {
                C = _memberInfo.Name,
                I = type.Id.GetValue(value, null)
            };

            serializer.Serialize(writer, dic);
        }

        /// <summary>
        /// In the case of an <see cref="EntityReference"/>
        /// create the proxy.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (reader.TokenType != JsonToken.Null) {
                var er = new EntityReference();
                serializer.Populate(reader, er);

                var idProp = _metaInfo.Id;
                var cvrt = TypeDescriptor.GetConverter(idProp.PropertyType);
                var id = cvrt.ConvertFrom(er.I);
                var proxy = new InternalProxyFactory(_typeRegistry).GetProxy(objectType, id);
                return proxy;
            }

            return null;
        }

        /// <summary>
        /// Intercept entities declared in the <see cref="TypeRegistry"/>.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType) {
            return _typeRegistry.IsTypeRegistered(objectType);
        }

    }
}