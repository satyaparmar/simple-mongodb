using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Operators;
using Pls.SimpleMongoDb.Tests.IntegrationTests.TestModel;

namespace Pls.SimpleMongoDb.Tests.IntegrationTests.Session
{
    [TestClass]
    public class SessionFindTests
        : DbTestBase
    {
        private const string DbName = Constants.TestDbName;
        private const string PersonsCollectionName = Constants.Collections.PersonsCollectionName;

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
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, documents);

            using (var session = new SimoSession(CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(new { WorkDays = new[] { 1, 1, 1, 1, 1, 1, 1 } });

                Assert.AreEqual("Nobel Adam", persons[0].Name);
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
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, documents);

            using (var session = new SimoSession(CreateConnection()))
            {
                var persons = session[DbName][PersonsCollectionName].Find<Person>(new WhereOperator("this.Age > 20 && this.Age < 65"));

                Assert.AreEqual("Daniel", persons[0].Name);
                Assert.AreEqual("Lazy John", persons[1].Name);
            }
        }
    }
}