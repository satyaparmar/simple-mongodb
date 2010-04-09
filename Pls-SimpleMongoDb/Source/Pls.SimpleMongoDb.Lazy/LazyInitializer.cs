using System;
using System.Reflection;
using LinFu.DynamicProxy;

namespace Pls.SimpleMongoDb
{

    /// <summary>
    /// Interceptor for lazy initialization of entities.
    /// </summary>
    public class LazyInitializer : IInterceptor
    {
        private readonly TypeRegistry _typeRegistry;
        private object _identifier;
        private readonly TypeInfo _typeInfo;
        private static readonly object InvokeImplementation = new object();
        private readonly bool _overridesEquals;
        private bool _initialized;
        private object _target;

        public LazyInitializer(TypeRegistry typeRegistry, Type type, object id) {
            _typeRegistry = typeRegistry;
            _identifier = id;
            _typeInfo = _typeRegistry.GetMeta(type);
            _overridesEquals = false;
        }

        public object Intercept(InvocationInfo info) {
            var returnValue = InternalInvoke(info.TargetMethod, info.Arguments, info.Target);

            // If the target has been handled return that value.
            if (returnValue != InvokeImplementation)
                return returnValue;

            var target = GetImplementation();
            returnValue = info.TargetMethod.Invoke(target, info.Arguments);

            return returnValue;
        }

        /// <summary>
        /// Dictacte whether the invocation should target special methods or
        /// belongs to the proxied entity.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        private object InternalInvoke(MethodInfo method, object[] args, object proxy) {
            var methodName = method.Name;
            var paramCount = method.GetParameters().Length;

            switch (paramCount) {
                case 0:
                    if (!_overridesEquals && methodName == "GetHashCode") {
                        //Todo: Write the HashCode provider
                    }

                    if (!_initialized && IsEqualToIdentifierMethod(method))
                        return _identifier;

                    if (methodName == "Dispose")
                        return null;

                    break;
                case 1:
                    if (!_overridesEquals && methodName == "Equals") {
                        //Todo: Write the equality comparar                       
                    }

                    if (method.Name.Equals(_typeInfo.Id.GetSetMethod().Name)) {
                        Initialize();
                        _identifier = args[0];
                        return InvokeImplementation;
                    }
                    break;
            }

            return InvokeImplementation;

        }

        /// <summary>
        /// in the case of inherited identifier methods (from a base class or an iterface) the
        /// passed in MethodBase object is not equal to the getIdentifierMethod instance that we
        /// have... but if their names and return types are identical, then it is the correct 
        /// identifier method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private bool IsEqualToIdentifierMethod(MethodInfo method) {
            if (_typeInfo.Id == null)
                return false;

            var pam = _typeInfo.Id.GetGetMethod();
            return method.Name.Equals(pam.Name) && method.ReturnType.Equals(pam.ReturnType);
        }

        /// <summary>
        /// Retrieve the referenced lazy entity.
        /// </summary>        
        private object GetImplementation() {
            Initialize();
            return _target;
        }

        /// <summary>
        /// Initialize the target entity from the data store.
        /// </summary>
        private void Initialize() {
            if (_initialized)
                return;

            _target = _typeRegistry.Load(_typeInfo, _identifier);
            _initialized = true;
        }
        
    }
}