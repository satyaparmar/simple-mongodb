using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb;
using Pls.SimpleMongoDb.Operators;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.EntityStore
{
    [TestClass]
    public class EntityStoreFindTests
        : TestBase
    {
        private const string DbName = Constants.TestDbName;

        [TestMethod]
        public void Find_UsingJsonWhereOperator_ReturnsTwoOfThree()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Adam", Age = 55},
                                new Person {Name = "Sue", Age = 55},
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            var cn = TestHelper.CreateConnection();
            using (var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.Find<Person>(@"{$where : ""this.Name == 'Daniel' || this.Name == 'Sue'""}");

                var danielAndSueFound = persons.Where(p => new[] { "Daniel", "Sue" }.Contains(p.Name)).Count() == 2;
                Assert.AreEqual(2, persons.Count);
                Assert.IsTrue(danielAndSueFound);
            }
        }

        [TestMethod]
        public void Find_UsingWhereOperator_ReturnsTwoOfThree()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Adam", Age = 55},
                                new Person {Name = "Sue", Age = 55},
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            var cn = TestHelper.CreateConnection();
            using (var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.Find<Person>(new WhereOp(@"this.Name == 'Daniel' || this.Name == 'Sue'"));

                var danielAndSueFound = persons.Where(p => new[] { "Daniel", "Sue" }.Contains(p.Name)).Count() == 2;
                Assert.AreEqual(2, persons.Count);
                Assert.IsTrue(danielAndSueFound);
            }
        }

        [TestMethod]
        public void Find_UsingJsonInOperator_ReturnsTwoOfThree()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Adam", Age = 55},
                                new Person {Name = "Sue", Age = 55},
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            var cn = TestHelper.CreateConnection();
            using (var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.Find<Person>(@"{Name : { $in : [""Daniel"", ""Sue""] } }");

                var danielAndSueFound = persons.Where(p => new[] { "Daniel", "Sue" }.Contains(p.Name)).Count() == 2;
                Assert.AreEqual(2, persons.Count);
                Assert.IsTrue(danielAndSueFound);
            }
        }

        [TestMethod]
        public void Find_UsingInOperator_ReturnsTwoOfThree()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Adam", Age = 55},
                                new Person {Name = "Sue", Age = 55},
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            var cn = TestHelper.CreateConnection();
            using(var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.Find<Person>(new InOp("Name", "Daniel", "Sue"));

                var danielAndSueFound = persons.Where(p => new[] { "Daniel", "Sue" }.Contains(p.Name)).Count() == 2;
                Assert.AreEqual(2, persons.Count);
                Assert.IsTrue(danielAndSueFound);
            }
        }

        [TestMethod]
        public void Find_UsingChainedQuery_ReturnsTwoOfThree()
        {
            var documents = new[]
                            {
                                new Person {Name = "Daniel", Age = 21},
                                new Person {Name = "Daniel", Age = 23},
                                new Person {Name = "Daniel", Age = 29},
                                new Person {Name = "Adam", Age = 21},
                                new Person {Name = "Adam", Age = 23},
                                new Person {Name = "Adam", Age = 29},
                                new Person {Name = "Sue", Age = 21},
                                new Person {Name = "Sue", Age = 23},
                                new Person {Name = "Sue", Age = 29}
                            };
            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);

            var cn = TestHelper.CreateConnection();
            using (var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var q = new Query()["Name"].In("Daniel", "Sue").And("Age").Gt(21).And().Lt(29);
                var persons = entityStore.Find<Person>(q.ToString());

                Assert.AreEqual(2, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }
    }
}