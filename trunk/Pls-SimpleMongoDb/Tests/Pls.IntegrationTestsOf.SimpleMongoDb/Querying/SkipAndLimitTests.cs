using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb;

namespace Pls.IntegrationTestsOf.SimpleMongoDb.Querying
{
    [TestClass]
    public class SkipAndLimitTests
        : TestBase
    {
        private const string DbName = Constants.TestDbName;

        protected override void OnTestInitialize()
        {
            var persons = new List<Person>();
            for (var i = 0; i < 100; i++)
                persons.Add(new Person {Name = "Daniel", Age = i+1});

            TestHelper.InsertDocuments(Constants.Collections.PersonsCollectionName, persons);
        }

        [TestMethod]
        public void Find_UsingSkip_SkipsTwoOf100()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.FindAll<Person>(skip: 2);

                Assert.AreEqual(98, persons.Count);
            }
        }

        [TestMethod]
        public void Find_UsingLimit_LimitsTo20Of100()
        {
            using (var session = TestHelper.CreateSession())
            {
                var entityStore = new SimoEntityStore(session, DbName);

                var persons = entityStore.FindAll<Person>(limit: -20);

                Assert.AreEqual(20, persons.Count);
            }
        }
    }
}