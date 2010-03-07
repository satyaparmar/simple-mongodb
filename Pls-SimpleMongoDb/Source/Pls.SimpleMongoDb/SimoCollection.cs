using System;
using System.Collections.Generic;
using System.Linq;
using Pls.SimpleMongoDb.Commands;

namespace Pls.SimpleMongoDb
{
    public class SimoCollection
        : ISimoCollection
    {
        public ISimoDatabase Database { get; private set; }

        public string Name { get; private set; }

        public string FullCollectionName
        {
            get { return string.Format("{0}.{1}", Database.Name, Name); }
        }

        public SimoCollection(ISimoDatabase database, string name)
        {
            Database = database;
            Name = name;
        }
        public void Insert(object document)
        {
            Insert(new[] { document });
        }

        public void Insert(IEnumerable<object> documents)
        {
            var cmd = new InsertDocumentsCommand(Database.Connection)
                          {
                              Documents = documents.ToList(),
                              FullCollectionName = FullCollectionName
                          };
            cmd.Execute();
        }

        public void Update(object selector, object document)
        {
            var cmd = new UpdateDocumentsCommand(Database.Connection)
                      {
                          Mode = UpdateModes.Upsert,
                          FullCollectionName = FullCollectionName,
                          QuerySelector = selector,
                          Document = document
                      };
            cmd.Execute();
        }

        public void UpdateMany(object selector, object document)
        {
            var cmd = new UpdateDocumentsCommand(Database.Connection)
            {
                Mode = UpdateModes.MultiUpdate,
                FullCollectionName = FullCollectionName,
                QuerySelector = selector,
                Document = document
            };
            cmd.Execute();
        }

        public void Delete(object selector)
        {
            var cmd = new DeleteDocumentsCommand(Database.Connection)
                          {
                              Selector = selector,
                              FullCollectionName = FullCollectionName
                          };
            cmd.Execute();
        }

        public IList<T> Find<T>(object selector, object documentSchema = null)
            where T : class
        {
            var cmd = new QueryDocumentsCommand<T>(Database.Connection)
                          {
                              FullCollectionName = FullCollectionName,
                              QuerySelector = selector,
                              DocumentSchema = documentSchema
                          };
            cmd.Execute();

            return cmd.Response;
        }
    }
}