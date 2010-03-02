using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Commands;
using Pls.SimpleMongoDb.Tests.TestModel;

namespace Pls.SimpleMongoDb.Tests.Commands
{
    [TestClass]
    public class InsertDocumentsDbTests
        : DbTestBase
    {
        [TestMethod]
        public void InsertSingle_AnonymousDocument_IsStoredAndAssignedId()
        {
            var document2Insert = new { Name = "Daniel", Age = 29 };

            using (var cn = CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                {
                    FullCollectionName = Constants.Commands.PersonsFullCollectionName,
                    Documents = new[] { document2Insert }
                };
                insertCommand.Execute();
            }

            var inferedTemplate = new { _id = SimoId.Empty };
            var refetched = GetDocument(document2Insert, inferedTemplate, Constants.Commands.PersonsFullCollectionName);
            Assert.AreNotEqual(SimoId.Empty, refetched._id);
        }

        [TestMethod]
        public void InsertSingle_AnonymousDocumentWithArray_ArrayItemsStored()
        {
            var document2Insert = new { Name = "Daniel", Age = 29, WorkDays = new[] { 1, 1, 1, 0, 1, 0, 0 } };

            using (var cn = CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                {
                    FullCollectionName = Constants.Commands.PersonsFullCollectionName,
                    Documents = new[] { document2Insert }
                };
                insertCommand.Execute();
            }

            var inferedTemplate = new { _id = SimoId.Empty, WorkDays = new int[]{} };
            var refetched = GetDocument(document2Insert, inferedTemplate, Constants.Commands.PersonsFullCollectionName);
            CollectionAssert.AreEqual(document2Insert.WorkDays, refetched.WorkDays);
        }

        [TestMethod]
        public void InsertSingle_JsonDocument_IsStoredAndAssignedId()
        {
            var document2Insert = @"{Name : ""Daniel"", Age : 29}";

            using (var cn = CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                {
                    FullCollectionName = Constants.Commands.PersonsFullCollectionName,
                    Documents = new[] { document2Insert }
                };
                insertCommand.Execute();
            }

            var inferedTemplate = new { _id = SimoId.Empty };
            var refetched = GetDocument(document2Insert, inferedTemplate, Constants.Commands.PersonsFullCollectionName);
            Assert.AreNotEqual(SimoId.Empty, refetched._id);
        }

        [TestMethod]
        public void InsertSingle_TypedDocument_IsStoredAndAssignedId()
        {
            var person2Insert = new Person { Name = "Daniel", Age = 29 };

            using (var cn = CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                {
                    FullCollectionName = Constants.Commands.PersonsFullCollectionName,
                    Documents = new[] { person2Insert }
                };
                insertCommand.Execute();
            }

            var inferedTemplate = new { _id = SimoId.Empty };
            var refetched = GetDocument(person2Insert, inferedTemplate, Constants.Commands.PersonsFullCollectionName);
            Assert.AreNotEqual(SimoId.Empty, refetched._id);
        }

        [TestMethod]
        public void InsertSingle_TypedDocumentWithId_IsStoredAndAssignedId()
        {
            var person2Insert = new PersonWithId { Name = "Daniel", Age = 29 };

            using (var cn = CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                {
                    FullCollectionName = Constants.Commands.PersonsFullCollectionName,
                    Documents = new[] { person2Insert }
                };
                insertCommand.Execute();
            }

            var refetched = GetDocument<PersonWithId>(person2Insert, Constants.Commands.PersonsFullCollectionName);
            Assert.AreNotEqual(SimoId.Empty, refetched._id);
        }

        [TestMethod]
        public void InsertSingle_DocumentExtendsMongoDocument_IsStoredAndAssignedId()
        {
            var person2Insert = new PersonExtendingSimoDocument { Name = "Daniel", Age = 29 };

            using (var cn = CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                {
                    FullCollectionName = Constants.Commands.PersonsFullCollectionName,
                    Documents = new[] { person2Insert }
                };
                insertCommand.Execute();
            }

            var refetched = GetDocument<PersonExtendingSimoDocument>(person2Insert, Constants.Commands.PersonsFullCollectionName);
            Assert.AreNotEqual(SimoId.Empty, refetched._id);
        }

        [TestMethod]
        public void InsertMany_DocumentsExtendsMongoDocument_IsStoredAndAssignedId()
        {
            var persons2Insert = new[]
            {
                new PersonExtendingSimoDocument { Name = "Daniel", Age = 29 },
                new PersonExtendingSimoDocument { Name = "Daniel", Age = 30 }
            };

            using (var cn = CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                {
                    FullCollectionName = Constants.Commands.PersonsFullCollectionName,
                    Documents = persons2Insert
                };
                insertCommand.Execute();
            }

            var refetched = GetDocuments<PersonExtendingSimoDocument>(new { Name = "Daniel" }, Constants.Commands.PersonsFullCollectionName);
            Assert.AreEqual(2, refetched.Count);
            Assert.AreNotEqual(SimoId.Empty, refetched[0]);
            Assert.AreNotEqual(SimoId.Empty, refetched[1]);
            Assert.AreEqual("Daniel", refetched[0].Name);
            Assert.AreEqual(29, refetched[0].Age);
            Assert.AreEqual("Daniel", refetched[1].Name);
            Assert.AreEqual(30, refetched[1].Age);
        }

        [TestMethod]
        public void InsertSingle_TypedNestedDocument_NestedObjectIsStoredWithNoId()
        {
            var car2Insert = new Car
            {
                RegNo = "ABC123",
                Owner = new Owner { Name = "Daniel" }
            };

            using (var cn = CreateConnection())
            {
                var insertCommand = new InsertDocumentsCommand(cn)
                {
                    FullCollectionName = Constants.Commands.CarsFullCollectionName,
                    Documents = new[] { car2Insert }
                };
                insertCommand.Execute();
            }

            var refetched = GetDocument<Car>(car2Insert, Constants.Commands.CarsFullCollectionName);
            Assert.IsNotNull(refetched.Owner);
            Assert.IsNull(refetched.Owner._id);
        }
    }
}
