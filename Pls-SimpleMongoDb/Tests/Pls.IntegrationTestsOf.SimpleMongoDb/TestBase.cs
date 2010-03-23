using System.Collections.Generic;
using System.Linq;
using Pls.SimpleMongoDb;
using Pls.SimpleMongoDb.Commands;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    public abstract class TestBase
    {
        protected TestBase()
        {
            DropDatabase();
        }

        protected ISimoConnection CreateConnection()
        {
            return ObjectFactoryForTests.CreateConnection();
        }

        private void DropDatabase()
        {
            using (var cn = CreateConnection())
            {
                var cmd = new DatabaseCommand(cn)
                          {
                              DatabaseName = Constants.TestDbName,
                              Command = new { dropDatabase = 1 }
                          };
                cmd.Execute();
            }
        }

        protected void InsertDocuments<TDocument>(string fullCollectionName, params TDocument[] documents)
            where TDocument : class
        {
            using (var cn = CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                                    {
                                        FullCollectionName = fullCollectionName,
                                        Documents = documents
                                    };
                insertCommand.Execute();
            }
        }

        protected int GetDocumentCount(string fullCollectionName)
        {
            return GetDocumentCount(null, fullCollectionName);
        }

        protected int GetDocumentCount(object selector, string fullCollectionName)
        {
            using (var cn = CreateConnection())
            {

                var queryCommand = new InferedCommandFactory().CreateInfered(cn, new { _id = SimoId.Empty });
                queryCommand.FullCollectionName = fullCollectionName;
                queryCommand.QuerySelector = selector;
                queryCommand.Execute();

                return queryCommand.Response.Count();
            }
        }

        protected TDocument GetDocument<TDocument>(object selector, TDocument inferedTemplate, string fullCollectionName)
            where TDocument : class
        {
            using (var cn = CreateConnection())
            {
                var queryCommand = new InferedCommandFactory().CreateInfered(cn, inferedTemplate);
                queryCommand.FullCollectionName = fullCollectionName;
                queryCommand.QuerySelector = selector;
                queryCommand.Execute();

                return queryCommand.Response.Single();
            }
        }

        protected TDocument GetDocument<TDocument>(object selector, string fullCollectionName)
            where TDocument : class
        {
            return GetDocuments<TDocument>(selector, fullCollectionName).Single();
        }

        protected IList<TDocument> GetDocuments<TDocument>(string fullCollectionName)
            where TDocument : class
        {
            return GetDocuments<TDocument>(null, fullCollectionName);
        }

        protected IList<TDocument> GetDocuments<TDocument>(object selector, string fullCollectionName)
            where TDocument : class
        {
            using (var cn = CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<TDocument>(cn)
                                   {
                                       FullCollectionName = fullCollectionName,
                                       QuerySelector = selector
                                   };
                queryCommand.Execute();

                return queryCommand.Response.ToList();
            }
        }
    }
}