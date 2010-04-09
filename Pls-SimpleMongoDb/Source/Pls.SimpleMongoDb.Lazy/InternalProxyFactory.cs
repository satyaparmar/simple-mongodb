using System;
using LinFu.DynamicProxy;

namespace Pls.SimpleMongoDb
{
    /// <summary>
    /// Simple proxy factory.
    /// </summary>
    public class InternalProxyFactory
    {
        private readonly TypeRegistry _typeRegistry;

        public InternalProxyFactory(TypeRegistry typeRegistry) {
            _typeRegistry = typeRegistry;
        }

        private static readonly ProxyFactory Factory = new ProxyFactory();

        public object GetProxy(Type type, object id) {
            var initializer = new LazyInitializer(_typeRegistry, type, id);
            var proxyInstance = Factory.CreateProxy(type, initializer, typeof(IProxiedEntity));
            return proxyInstance;
        }

    }
}