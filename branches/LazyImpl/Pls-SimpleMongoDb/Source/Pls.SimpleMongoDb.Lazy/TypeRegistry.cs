using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pls.SimpleMongoDb.Commands;

namespace Pls.SimpleMongoDb
{

    /// <summary>
    /// Type bookkeeping for lazy entities.
    /// </summary>
    public class TypeRegistry
    {
        public Func<ISimoConnection> Connection { get; set; }

        /// <summary>
        /// The connection is a delegate which can create a <see cref="ISimoConnection"/>.
        /// </summary>
        /// <param name="connection"></param>
        public TypeRegistry(Func<ISimoConnection> connection) {
            Connection = connection;
        }

        private readonly List<TypeInfo> _types = new List<TypeInfo>();

        /// <summary>
        /// Customize how the Type should be handled.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="customizeAction"></param>
        public void Class<TEntity>(Action<ITypeCustomizer<TEntity>> customizeAction) where TEntity : class {
            var ti = new TypeInfo { ClassType = typeof(TEntity) };
            var customizer = new TypeCustomizer<TEntity>(ti);
            customizeAction(customizer);

            _types.Add(ti);
        }

        /// <summary>
        /// Performs a check to verify if the type is registered.
        /// If the type is a <see cref="IProxiedEntity"/>, the 
        /// base type will be used.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsTypeRegistered(Type type) {
            var t = typeof(IProxiedEntity).IsAssignableFrom(type) ? type.BaseType : type;
            return _types.Count(x => x.ClassType == t) != 0;
        }

        /// <summary>
        /// Retrieve the <see cref="TypeInfo"/> for the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public TypeInfo GetMeta(Type type) {
            return _types.FirstOrDefault(x => x.ClassType == type);
        }

        /// <summary>
        /// Helper method to retrieve an entity by its identifier.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public object Load(TypeInfo type, object identifier) {
            var m = GetType().GetMethod("LoadGeneric", BindingFlags.Public | BindingFlags.Instance);
            var gm = m.MakeGenericMethod(new[] { type.ClassType });
            return gm.Invoke(this, new[] { type, identifier });
        }

        /// <summary>
        /// Generic loader.
        /// </summary>
        /// <typeparam name="TEntity">Type to return</typeparam>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public TEntity LoadGeneric<TEntity>(TypeInfo type, object identifier) where TEntity : class {
            using (var con = Connection()) {
                var gt = new QueryDocumentsCommand<TEntity>(con) {
                    FullCollectionName = type.FullCollectionName,
                    QuerySelector = " { '" + type.Id.Name + "' : '" + identifier + "' } "
                };

                gt.Execute();
                return gt.Response.Documents.FirstOrDefault();
            }
        }
    }

}