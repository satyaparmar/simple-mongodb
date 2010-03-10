using System.Collections.Generic;

namespace Pls.SimpleMongoDb
{
    public interface ISimoCollection
    {
        ISimoDatabase Database { get; }
        string Name { get; }
        string FullCollectionName { get; }

        void Insert(object document);
        void Insert(IEnumerable<object> documents);
        
        void Update(object selector, object document);
        void UpdateMany(object selector, object document);

        void Delete(object selector);
        
        IList<T> Find<T>(object selector, object documentSchema = null)
            where T : class;

        T FindOne<T>(object selector, object documentSchema = null)
            where T : class;

        int Count(object selector = null);
    }
}