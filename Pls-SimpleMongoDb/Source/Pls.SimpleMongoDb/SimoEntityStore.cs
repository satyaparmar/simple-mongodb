using System.Collections.Generic;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.SimpleMongoDb
{
    /// <summary>
    /// Allows you to work on a higher level with MongoDB, so that
    /// you don't explicitly have to work with Databases and Collections.
    /// If the Session.SimoPluralizer is enabled, all entitynames will be
    /// pluralized, hence stored in a collection with a pluralized name.
    /// If you use any of the generic overloads, the typename of the
    /// passed entity will be pluralized and used as collectionname.
    /// </summary>
    public class SimoEntityStore
        : ISimoEntityStore
    {
        public ISimoDatabase Database { get; private set; }

        public ISimoSession Session { get; private set; }

        public SimoEntityStore(ISimoSession session, string databaseName)
        {
            Session = session;
            Database = Session[databaseName];
        }

        public void Drop()
        {
            Database.DropDatabase();
        }

        public void DropEntityCollection<T>()
            where T : class 
        {
            var collectionName = GetEntityCollectionName<T>();
            Database.DropCollections(collectionName);
        }

        public void DropEntityCollections()
        {
            Database.DropCollections();
        }

        public void Insert<T>(T entity)
             where T : class
        {
            GetEntityCollection<T>().Insert(entity);
        }

        public void Insert(string entityName, object entity)
        {
            GetEntityCollection(entityName).Insert(entity);
        }

        public void Insert<T>(IEnumerable<T> entities)
            where T : class
        {
            GetEntityCollection<T>().Insert(entities);
        }

        public void Insert(string entityName, IEnumerable<object> entities)
        {
            GetEntityCollection(entityName).Insert(entities);
        }

        public void Update<T>(object selector, T entity)
            where T : class
        {
            GetEntityCollection<T>().Update(selector, entity);
        }

        public void Update(string entityName, object selector, object entity)
        {
            GetEntityCollection(entityName).Update(selector, entity);
        }

        public void UpdateMany<T>(object selector, T entity)
            where T : class
        {
            GetEntityCollection<T>().UpdateMany(selector, entity);
        }

        public void UpdateMany(string entityName, object selector, object entity)
        {
            GetEntityCollection(entityName).UpdateMany(selector, entity);
        }

        public void Delete<T>(object selector)
            where T : class
        {
            GetEntityCollection<T>().Delete(selector);
        }

        public void Delete(string entityName, object selector)
        {
            GetEntityCollection(entityName).Delete(selector);
        }

        public IList<T> Find<T>(object selector, object entitySchema = null)
            where T : class
        {
            return GetEntityCollection<T>().Find<T>(selector, entitySchema);
        }

        public IList<T> Find<T>(T infered, string entityName, object selector, object entitySchema = null)
            where T : class
        {
            return GetEntityCollection(entityName).Find<T>(selector, entitySchema);
        }

        public T FindOne<T>(object selector, object entitySchema = null)
            where T : class
        {
            return GetEntityCollection<T>().FindOne<T>(selector, entitySchema);
        }

        public T FindOne<T>(T infered, string entityName, object selector, object entitySchema = null)
            where T : class
        {
            return GetEntityCollection(entityName).FindOne<T>(selector, entitySchema);
        }

        public int Count<T>(object selector = null)
            where T : class
        {
            return GetEntityCollection<T>().Count(selector);
        }

        public SimoReference Reference<T>(SimoId id) where T : class
        {
            var entityName = EntityMetadata<T>.EntityName;

            return Reference(entityName, id);
        }

        public SimoReference Reference(string entityName, SimoId id)
        {
            entityName = Session.Pluralizer.Pluralize(entityName);
            
            return new SimoReference { CollectionName = entityName, Id = id };
        }

        public SimoReference<TId> Reference<T, TId>(TId id) where T : class
        {
            var entityName = EntityMetadata<T>.EntityName;

            return Reference(entityName, id);
        }

        public SimoReference<TId> Reference<TId>(string entityName, TId id)
        {
            entityName = Session.Pluralizer.Pluralize(entityName);

            return new SimoReference<TId> { CollectionName = entityName, Id = id };
        }

        private ISimoCollection GetEntityCollection<T>()
            where T : class
        {
            return Database.GetCollection<T>();
        }

        private ISimoCollection GetEntityCollection(string entityName)
        {
            return Database[entityName];
        }

        public string GetEntityCollectionName<T>()
            where T : class
        {
            return EntityMetadata<T>.EntityName;
        }
    }
}