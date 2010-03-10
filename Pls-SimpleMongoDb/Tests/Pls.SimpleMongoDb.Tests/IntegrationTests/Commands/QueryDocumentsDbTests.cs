using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Commands;
using Pls.SimpleMongoDb.DataTypes;
using Pls.SimpleMongoDb.Operators;
using Pls.SimpleMongoDb.Tests.IntegrationTests.TestModel;

namespace Pls.SimpleMongoDb.Tests.IntegrationTests.Commands
{
    [TestClass]
    public class QueryDocumentsDbTests
        : DbTestBase
    {
        [TestMethod]
        public void QuerySinglePerson_AnonymousTypeQueryWithString_ItemReturned()
        {
            var person = new Person { Name = "Daniel", Age = 29 };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, person);

            using (var cn = CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                       QuerySelector = new { Name = "Daniel" }
                                   };
                queryCommand.Execute();

                Assert.AreEqual(1, queryCommand.Response.Count());
            }
        }

        [TestMethod]
        public void QuerySinglePerson_AnonymousTypeQueryWithDateTime_ItemReturned()
        {
            var tempDate = new DateTime(2010, 1, 1, 10, 02, 03, 04);
            var person = new Person { Name = "Daniel", Age = 29, TempDate = tempDate };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, person);

            using (var cn = CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                       QuerySelector = new { TempDate = tempDate }
                                   };
                queryCommand.Execute();

                Assert.AreEqual(1, queryCommand.Response.Count());
            }
        }

        [TestMethod]
        public void QueryManyPersons_NoQuery_ReturnsAll()
        {
            var tempDate = new DateTime(2010, 1, 1, 10, 02, 03, 04);
            var persons = new[]
                          {
                              new Person { Name = "Daniel", Age = 29, TempDate = tempDate },
                              new Person { Name = "Daniel", Age = 30, TempDate = tempDate }
                          };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, persons);

            using (var cn = CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName
                                   };
                queryCommand.Execute();

                Assert.AreEqual(2, queryCommand.Response.Count());
            }
        }

        [TestMethod]
        public void QueryManyPersons_WhereClauseUsingJson_ReturnsAllExceptOne()
        {
            var person = new[]
                         {
                             new Person { Name = "Daniel1", Age = 29 },
                             new Person { Name = "Daniel2", Age = 29 },
                             new Person { Name = "Daniel3", Age = 30 }
                         };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, person);

            using (var cn = CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                       QuerySelector = @"{$where : ""this.Name.indexOf('Daniel') > -1 && this.Age < 30""}"
                                   };
                queryCommand.Execute();

                Assert.AreEqual(2, queryCommand.Response.Count());
            }
        }

        [TestMethod]
        public void QueryManyPersons_WhereClauseUsingJsonObject_ReturnsAllExceptOne()
        {
            var person = new[]
                         {
                             new Person { Name = "Daniel1", Age = 29 },
                             new Person { Name = "Daniel2", Age = 29 },
                             new Person { Name = "Daniel3", Age = 30 }
                         };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, person);

            using (var cn = CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                       QuerySelector = new SimoJson(@"{$where : ""this.Name.indexOf('Daniel') > -1 && this.Age < 30""}")
                                   };
                queryCommand.Execute();

                Assert.AreEqual(2, queryCommand.Response.Count());
            }
        }

        [TestMethod]
        public void QueryManyPersons_WhereClauseUsingWhereOperator_ReturnsAllExceptOne()
        {
            var person = new[]
                         {
                             new Person { Name = "Daniel1", Age = 29 },
                             new Person { Name = "Daniel2", Age = 29 },
                             new Person { Name = "Daniel3", Age = 30 }
                         };
            InsertDocuments(Constants.Collections.PersonsFullCollectionName, person);

            using (var cn = CreateConnection())
            {
                var queryCommand = new QueryDocumentsCommand<Person>(cn)
                                   {
                                       FullCollectionName = Constants.Collections.PersonsFullCollectionName,
                                       QuerySelector = new WhereOperator("this.Name.indexOf('Daniel') > -1 && this.Age < 30")
                                   };
                queryCommand.Execute();

                Assert.AreEqual(2, queryCommand.Response.Count());
            }
        }
    }
}