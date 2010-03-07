using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Operators;
using Pls.SimpleMongoDb.Tests.TestModel;

namespace Pls.SimpleMongoDb.Tests.Session
{
    [TestClass]
    public class SessionTests
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
        public void Delete_UsingJsonSelector_TwoOfFourDocumentsAreDeleted()
        {
            var documents = new[]
                {
                    new Person {Name = "Daniel", Age = 29, WorkDays = new[] {1, 1, 1, 0, 1, 0, 0}},
                    new Person {Name = "Lazy John", Age = 55, WorkDays = new[] {0, 1, 0, 1, 0, 0, 0}},
                    new Person {Name = "Nobel Adam", Age = 65, WorkDays = new[] {1, 1, 1, 1, 1, 1, 1}},
                    new Person {Name = "Sue", Age = 20, WorkDays = new[] {1, 1, 1, 1, 1, 0, 0}}
                };
            InsertDocuments(Constants.Commands.PersonsFullCollectionName, documents);

            using (var session = new SimoSession(CreateConnection()))
            {
                session[DbName][PersonsCollectionName].Delete(@"{$where : ""this.Name == 'Sue' || this.WorkDays[0] == 0""}");
            }

            var storedDocuments = GetDocuments<Person>(Constants.Commands.PersonsFullCollectionName);
            Assert.AreEqual("Daniel", storedDocuments[0].Name);
            Assert.AreEqual("Nobel Adam", storedDocuments[1].Name);
        }

        [TestMethod]
        public void Find_UsingDocumentSelector_SingleItemReturned()
        {
            var documents = new[]
                {
                    new Person {Name = "Daniel", Age = 29, WorkDays = new[] {1, 1, 1, 0, 1, 0, 0}},
                    new Person {Name = "Lazy John", Age = 55, WorkDays = new[] {0, 1, 0, 1, 0, 0, 0}},
                    new Person {Name = "Nobel Adam", Age = 65, WorkDays = new[] {1, 1, 1, 1, 1, 1, 1}},
                    new Person {Name = "Sue", Age = 20, WorkDays = new[] {1, 1, 1, 1, 1, 0, 0}}
                };
            InsertDocuments(Constants.Commands.PersonsFullCollectionName, documents);

            using (var session = new SimoSession(CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(new { WorkDays = new[] { 1, 1, 1, 1, 1, 1, 1 } });
            }
        }

        [TestMethod]
        public void Find_UsingWhereOperator_SingleItemReturned()
        {
            var documents = new[]
                {
                    new Person {Name = "Daniel", Age = 29},
                    new Person {Name = "Lazy John", Age = 55},
                    new Person {Name = "Nobel Adam", Age = 65},
                    new Person {Name = "Sue", Age = 20}
                };
            InsertDocuments(Constants.Commands.PersonsFullCollectionName, documents);

            using (var session = new SimoSession(CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(new WhereOperator("this.Age > 20 && this.Age < 65"));

                Assert.AreEqual("Daniel", persons[0].Name);
                Assert.AreEqual("Lazy John", persons[1].Name);
            }
        }

        [TestMethod]
        public void Update_EmptyDb_UpsertsDocument()
        {
            var document = new { Name = "Daniel" };

            using (var session = new SimoSession(CreateConnection()))
            {
                session[DbName][PersonsCollectionName].Update(document, document);
            }

            var numOfStoredDocuments = GetDocumentCount(Constants.Commands.PersonsFullCollectionName);
            Assert.AreEqual(1, numOfStoredDocuments);
        }

        [TestMethod]
        public void Update_UsingAnonymousDocument_TwoOfFourItemsUpdated()
        {
            var documents = new[]
                {
                    new Person {Name = "Daniel1", Age = 29},
                    new Person {Name = "Daniel2", Age = 30},
                    new Person {Name = "Daniel3", Age = 31},
                    new Person {Name = "Daniel4", Age = 32}
                };
            InsertDocuments(Constants.Commands.PersonsFullCollectionName, documents);

            using (var session = new SimoSession(CreateConnection()))
            {
                session[DbName][PersonsCollectionName].UpdateMany(@"{$where : ""this.Age < 31""}", @"{$inc : {Age : 1}}");
            }

            var refetched = GetDocuments<Person>(Constants.Commands.PersonsFullCollectionName);
            Assert.AreEqual(30, refetched[0].Age);
            Assert.AreEqual(31, refetched[1].Age);
            Assert.AreEqual(31, refetched[2].Age);
            Assert.AreEqual(32, refetched[3].Age);
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
