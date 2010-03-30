using System.Collections.Generic;

namespace Pls.SimpleMongoDb
{
    public interface ISimoCollection
    {
        ISimoDatabase Database { get; }
        string Name { get; }
        string FullCollectionName { get; }

        void Drop();

        void Insert(object document);
        void Insert(IEnumerable<object> documents);
        
        void Update(object selector, object document);
        void UpdateMany(object selector, object document);

        void Delete(object selector);
        
        IList<T> Find<T>(object selector, object documentSchema = null)
            where T : class;

        T FindOne<T>(object selector, object documentSchema = null)
            where T : class;

        IList<T> FindInfered<T>(T inferedTemplate, object selector)
            where T : class;

        T FindOneInfered<T>(T inferedTemplate, object selector)
            where T : class;

        int Count(object selector = null);
    }
}