using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Tests.TestModel;

namespace Pls.SimpleMongoDb.Tests.Session
{
    [TestClass]
    public class SessionDeleteTests
        : DbTestBase
    {
        private const string DbName = Constants.TestDbName;
        private const string PersonsCollectionName = Constants.Commands.PersonsCollectionName;

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
    }
}
