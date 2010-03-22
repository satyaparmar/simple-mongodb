﻿using System.Collections.Generic;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.SimpleMongoDb
{
    public interface ISimoEntityStore
    {
        ISimoSession Session { get; }
        ISimoDatabase Database { get; }

        void Drop();

        void Insert<T>(T entity) where T : class;
        void Insert(string entityName, object entity);

        void Insert<T>(IEnumerable<T> documents) where T : class;
        void Insert(string entityName, IEnumerable<object> entities);

        void Update<T>(object selector, T entity) where T : class;
        void Update(string entityName, object selector, object entity);

        void UpdateMany<T>(object selector, T document) where T : class;
        void UpdateMany(string entityName, object selector, object entity);

        void Delete<T>(object selector) where T : class;
        void Delete(string entityName, object selector);

        IList<T> Find<T>(object selector, object entitySchema = null) where T : class;

        T FindOne<T>(object selector, object entitySchema = null) where T : class;

        int Count<T>(object selector = null) where T : class;

        SimoReference Reference<T>(SimoId id) where T : class;
        SimoReference Reference(string entityName, SimoId id);
    }
}