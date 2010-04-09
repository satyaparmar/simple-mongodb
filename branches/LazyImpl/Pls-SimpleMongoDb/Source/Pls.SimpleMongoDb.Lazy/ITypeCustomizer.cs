using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Pls.SimpleMongoDb
{
    /// <summary>
    /// Customizer contract.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ITypeCustomizer<TEntity>
    {
        /// <summary>
        /// Identifier selector.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="idProperty"></param>
        void Id<TProperty>(Expression<Func<TEntity, TProperty>> idProperty);

        /// <summary>
        /// Collection Name.
        /// </summary>
        string CollectionName { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        string Database { get; set; }
    }

    public class TypeCustomizer<TEntity> : ITypeCustomizer<TEntity>
    {
        private readonly TypeInfo _ti;

        public TypeCustomizer(TypeInfo ti) {
            _ti = ti;
        }

        public void Id<TProperty>(Expression<Func<TEntity, TProperty>> idProperty) {
            var member = TypeExtensions.DecodeMemberAccessExpression(idProperty);
            _ti.Id = (PropertyInfo)member;
        }

        public string CollectionName {
            get { return _ti.CollectionName; }
            set { _ti.CollectionName = value; }
        }

        public string Database {
            get { return _ti.Database; }
            set { _ti.Database = value; }
        }
    }
}