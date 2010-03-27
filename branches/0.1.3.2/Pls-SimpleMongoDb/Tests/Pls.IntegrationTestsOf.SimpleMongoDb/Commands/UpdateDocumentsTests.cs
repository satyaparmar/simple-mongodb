using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb.Commands;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.Commands
{
    [TestClass]
    public class UpdateDocumentsTests
        : TestBase
    {
        [TestMethod]
        public void UpdateSinglePerson_ManyExists_OneIsUpdated()
        {
            var persons = new[]
                          {
                              new Person {Name = "Daniel1", Age = 29},
                              new Person {Name = "Daniel2", Age = 29}
                          };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, persons);

            using (var cn = CreateConnection())
            {
                var updateCommand = new UpdateDocumentsCommand(cn)
                                    {
                                        NodeName = Constants.Collections.PersonsFullCollectionName,
                                        QuerySelector = new { Name = "Daniel2" },
                                        Document = new { Name = "Daniel3" }
                                    };
                updateCommand.Execute();
            }

            Assert.AreEqual(1, GetDocumentCount(new { Name = "Daniel3" }, Constants.Collections.PersonsFullCollectionName));
        }

        [TestMethod]
        public void UpdateSinglePerson_EmptyDb_OneIsUpsert()
        {
            using (var cn = CreateConnection())
            {
                var updateCommand = new UpdateDocumentsCommand(cn)
                                    {
                                        Mode = UpdateModes.Upsert,
                                        NodeName = Constants.Collections.PersonsFullCollectionName,
                                        QuerySelector = new { Name = "Joe"},
                                        Document = new { Name = "Joe" }
                                    };
                updateCommand.Execute();
            }

            Assert.AreEqual(1, GetDocumentCount(Constants.Collections.PersonsFullCollectionName));
        }

        [TestMethod]
        public void UpdateManyPersons_UsingJsonWhereManyExists_AllAreUpdated()
        {
            var persons = new[]
                          {
                              new {Name = "Daniel1", Age = 29},
                              new {Name = "Daniel2", Age = 29}
                          };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, persons);

            using (var cn = CreateConnection())
            {
                var updateCommand = new UpdateDocumentsCommand(cn)
                                    {
                                        Mode = UpdateModes.MultiUpdate,
                                        NodeName = Constants.Collections.PersonsFullCollectionName,
                                        QuerySelector = @"{$where : ""this.Name.indexOf('Daniel') > -1""}",
                                        Document = @"{$set:{Name : ""The Daniel""}}"
                                    };
                updateCommand.Execute();
            }

            Assert.AreEqual(2, GetDocumentCount(new { Name = "The Daniel" }, Constants.Collections.PersonsFullCollectionName));
        }

        [TestMethod]
        public void UpdateManyPersons_UsingDocumentWhereManyExists_NoneIsUpdated()
        {
            var persons = new[]
                          {
                              new {Name = "Daniel1", Age = 29},
                              new {Name = "Daniel2", Age = 29}
                          };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, persons);

            using (var cn = CreateConnection())
            {
                var updateCommand = new UpdateDocumentsCommand(cn)
                                    {
                                        Mode = UpdateModes.MultiUpdate,
                                        NodeName = Constants.Collections.PersonsFullCollectionName,
                                        QuerySelector = @"{$where : ""this.Name.indexOf('Daniel') > -1""}",
                                        Document = new {Name = "The Daniel"}
                                    };
                updateCommand.Execute();
            }

            Assert.AreEqual(0, GetDocumentCount(new { Name = "The Daniel" }, Constants.Collections.PersonsFullCollectionName));
        }
    }
}