using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb;
using Pls.SimpleMongoDb.Querying;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.Querying
{
    [TestClass]
    public class QueryingUsingEntityStore
        : TestBase
    {
        private const string DbName = Constants.TestDbName;

        protected override void OnTestInitialize()
        {
            var documents = new[] {
                new Person { Name = "Daniel", Age = 21, TimeCodes = new[] { 100, 200, 300 }, Tags = new [] {"T1"} },
                new Person { Name = "Daniel", Age = 23, TimeCodes = new[] { 100 }, Tags = new [] {"T1", "T2"} },
                new Person { Name = "Daniel", Age = 29 },
                new Person { Name = "Adam", Age = 21, TimeCodes = new[] { 100, 200, 300 }, Tags = new [] {"T1"} },
                new Person { Name = "Adam", Age = 23, TimeCodes = new[] { 100 }, Tags = new [] {"T1", "T2"} },
                new Person { Name = "Adam", Age = 29 },
                new Person { Name = "Sue", Age = 21, TimeCodes = new[] { 100, 200, 300 }, Tags = new [] {"T1"} },
                new Person { Name = "Sue", Age = 23, TimeCodes = new[] { 100 }, Tags = new [] {"T1", "T2"} },
                new Person { Name = "Sue", Age = 29} };

            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, documents);
        }

        [TestMethod]
        public void Find_UsingChainedQuery_ReturnsTwoOfThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => 
                    q["Name"].In("Daniel", "Sue").And("Age").Between(22, 28));

                Assert.AreEqual(2, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingLtQuery_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Age"].Lt(23));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingGtQuery_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Age"].Gt(23));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingLtEQuery_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Age"].LtEq(21));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingGtEQuery_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Age"].GtEq(29));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingBetweenQuery_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Age"].Between(22, 28));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_IntervalUsingQueryLtAndGt_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Age"].Gt(21).Lt(29));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingInQuery_ReturnsFourPersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Name"].In("Daniel", "Sue").And("Age").In(21, 23));

                Assert.AreEqual(4, persons.Count);
                Assert.AreEqual(2, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(2, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingNotInQuery_ReturnsOnePerson()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Name"].NotIn("Daniel", "Sue").And("Age").NotIn(21, 23));

                Assert.AreEqual(1, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
            }
        }

        [TestMethod]
        public void Find_UsingNotEqualQuery_ReturnsFourPersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Name"].NotEq("Adam").And("Age").NotEq(23));

                Assert.AreEqual(4, persons.Count);
                Assert.AreEqual(2, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(2, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingModQuery_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Age"].Mod(21, 0));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingAllQueryWithInts_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["TimeCodes"].HasAll(100, 200));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingAllQueryWithStrings_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Tags"].HasAll("T1", "T2"));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingSizeQueryOnArrayWithNoStrings_ReturnsNoPersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Tags"].Size(0));

                Assert.AreEqual(0, persons.Count);
            }
        }

        [TestMethod]
        public void Find_UsingSizeQueryOnArrayWithStrings_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Tags"].Size(2));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingSizeQueryOnArrayWithNoInts_ReturnsNoPersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["TimeCodes"].Size(0));

                Assert.AreEqual(0, persons.Count);
            }
        }

        [TestMethod]
        public void Find_UsingSizeQueryOnArrayWithInts_ReturnsThreePersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["TimeCodes"].Size(1));

                Assert.AreEqual(3, persons.Count);
                Assert.AreEqual(1, persons.Where(p => p.Name == "Daniel").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Adam").Count());
                Assert.AreEqual(1, persons.Where(p => p.Name == "Sue").Count());
            }
        }

        [TestMethod]
        public void Find_UsingExistsQueryWhereMemberDoesNotExist_ReturnsNoPersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["FakeMember"].Exists());

                Assert.AreEqual(0, persons.Count);
            }
        }

        [TestMethod]
        public void Find_UsingNotExistsQueryWhereMemberDoesNotExist_ReturnsAllPersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["FakeMember"].NotExists());

                Assert.AreEqual(9, persons.Count);
            }
        }

        [TestMethod]
        public void Find_UsingExistsQueryWhereMemberExists_ReturnsAllPersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Tags"].Exists());

                Assert.AreEqual(9, persons.Count);
            }
        }

        [TestMethod]
        public void Find_UsingNotExistsQueryWhereMemberExists_ReturnsNoPersons()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Tags"].NotExists());

                Assert.AreEqual(0, persons.Count);
            }
        }

        [TestMethod]
        public void FindOne_UsingQuery_ReturnsOnePerson()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.QueryFor<Person>(q => q["Name"].In("Daniel").And("Age").In(21));

                Assert.AreEqual(1, persons.Count);
            }
        }
    }
}