using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Tests.TestModel;

namespace Pls.SimpleMongoDb.Tests.Session
{
    [TestClass]
    public class SessionInsertTests
        : DbTestBase
    {
        private const string DbName = Constants.TestDbName;
        private const string PersonsCollectionName = Constants.Commands.PersonsCollectionName;

        [TestMethod]
        public void Insert_SingleDocument_IsStored()
        {
            using (var session = new SimoSession(CreateConnection()))
            {
                var document = new
                               {
                                   Name = "Daniel",
                                   BirthDay = new DateTime(1980, 05, 03),
                                   WorkDays = new[] { 1, 1, 1, 0, 1, 0, 0 }
                               };
                session[DbName][PersonsCollectionName].Insert(document);
            }

            var numOfStoredDocuments = GetDocumentCount(Constants.Commands.PersonsFullCollectionName);
            Assert.AreEqual(1, numOfStoredDocuments);
        }

        [TestMethod]
        public void Insert_ManyDocuments_AllAreStored()
        {
            using (var session = new SimoSession(CreateConnection()))
            {
                var documents = new[]
                                {
                                    new Person {Name = "Daniel", Age = 29, WorkDays = new[] {1, 1, 1, 0, 1, 0, 0}},
                                    new Person {Name = "Lazy John", Age = 55, WorkDays = new[] {0, 1, 0, 1, 0, 0, 0}}
                                };
                session[DbName][PersonsCollectionName].Insert(documents);
            }

            var numOfStoredDocuments = GetDocumentCount(Constants.Commands.PersonsFullCollectionName);
            Assert.AreEqual(2, numOfStoredDocuments);
        }

        [TestMethod]
        public void Insert_TypedDocumentWithAssignedId_IsStored()
        {
            var document = new PersonWithId { _id = SimoObjectId.NewId(), Name = "Daniel" };

            using (var session = new SimoSession(CreateConnection()))
            {
                session[DbName][PersonsCollectionName].Insert(document);
            }

            var numOfDocs = GetDocumentCount(new { _id = document._id }, Constants.Commands.PersonsFullCollectionName);
            Assert.AreEqual(1, numOfDocs);
        }

        [TestMethod]
        public void Insert_TypedDocumentWithAutoAssignedId_IsStored()
        {
            var document = new PersonWithAutoId { Name = "Daniel" };

            using (var session = new SimoSession(CreateConnection()))
            {
                session[DbName][PersonsCollectionName].Insert(document);
            }

            var numOfDocs = GetDocumentCount(new { _id = document._id }, Constants.Commands.PersonsFullCollectionName);
            Assert.AreEqual(1, numOfDocs);
        }
    }
}