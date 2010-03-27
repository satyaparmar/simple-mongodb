using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb.Commands;
using Pls.SimpleMongoDb.Operators;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.Commands
{
    [TestClass]
    public class DeleteDocumentsTests
        : TestBase
    {
        [TestMethod]
        public void DeleteAll_WithSelector_ZeroCountLeft()
        {
            var documents = new[]
                            {
                                new { Name = "Daniel", Age = 29 }, 
                                new { Name = "Daniel", Age = 30 }
                            };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, documents);

            using (var cn = CreateConnection())
            {
                var deleteCommand = new DeleteDocumentsCommand(cn)
                                    {
                                        NodeName = Constants.Collections.PersonsFullCollectionName,
                                        Selector = new { Name = "Daniel" }
                                    };
                deleteCommand.Execute();
            }

            Assert.AreEqual(0, GetDocumentCount(Constants.Collections.PersonsFullCollectionName));
        }

        [TestMethod]
        public void DeleteAll_NullSelector_ZeroCountLeft()
        {
            var documents = new[]
                            {
                                new { Name = "Daniel", Age = 29 },
                                new { Name = "Daniel", Age = 30 }
                            };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, documents);

            using (var cn = CreateConnection())
            {
                var deleteCommand = new DeleteDocumentsCommand(cn)
                                    {
                                        NodeName = Constants.Collections.PersonsFullCollectionName
                                    };
                deleteCommand.Execute();
            }

            Assert.AreEqual(0, GetDocumentCount(new { Name = "Daniel" }, Constants.Collections.PersonsFullCollectionName));
        }

        [TestMethod]
        public void Delete_WhereClauseViaOperator_OneItemLeft()
        {
            var persons = new[]
                          {
                              new Person { Name = "Daniel1", Age = 29 },
                              new Person { Name = "Daniel2", Age = 29 },
                              new Person { Name = "Daniel3", Age = 30 }
                          };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, persons);

            using (var cn = CreateConnection())
            {
                var deleteCommand = new DeleteDocumentsCommand(cn)
                                    {
                                        NodeName = Constants.Collections.PersonsFullCollectionName,
                                        Selector = new WhereOperator("this.Name.indexOf('Daniel') > -1 && this.Age < 30")
                                    };
                deleteCommand.Execute();
            }

            Assert.AreEqual(1, GetDocumentCount(Constants.Collections.PersonsFullCollectionName));
        }

        [TestMethod]
        public void Delete_WhereClauseViaJson_OneItemLeft()
        {
            var persons = new[]
                          {
                              new Person { Name = "Daniel1", Age = 29 },
                              new Person { Name = "Daniel2", Age = 29 },
                              new Person { Name = "Daniel3", Age = 30 }
                          };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, persons);

            using (var cn = CreateConnection())
            {
                var deleteCommand = new DeleteDocumentsCommand(cn)
                                    {
                                        NodeName = Constants.Collections.PersonsFullCollectionName,
                                        Selector = @"{$where : ""this.Name.indexOf('Daniel') > -1 && this.Age < 30""}"
                                    };
                deleteCommand.Execute();
            }

            Assert.AreEqual(1, GetDocumentCount(Constants.Collections.PersonsFullCollectionName));
        }
    }
}