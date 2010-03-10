using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Tests.IntegrationTests.TestModel;

namespace Pls.SimpleMongoDb.Tests.IntegrationTests
{
    /// <summary>
    /// This class is not intended to execute tests (event though it does this),
    /// but to ensure that some ApiExamples that are published online, are valid.
    /// </summary>
    [TestClass]
    public class ApiExamples
        : DbTestBase
    {
        private const string DbName = Constants.TestDbName;

        [TestMethod]
        public void Insert_Example()
        {
            //The hierarchy in MongoDB is:
            //- Db
            //  - Collection
            //    - Document
            //By using EntityStore this is somewhat abstracted away.

            //You do need a connection to create a session
            //The creating of Connections and Session is something you most likely
            //will put in a IoC-container or factory or something.
            var cn = CreateConnection();
            using (var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                //If you are dealing with typed documents (e.g. Person)
                //you can use the generic API.
                //Then the name of the collection is the name of the
                //type, but pluralized.
                //If you don't want pluralization, call
                //session.SimoPluralizer.Disable(), or
                //replace the implementation of the 
                //SimoIoC.Instance.PluralizerFactory
                var person = new Person { Name = "Daniel", Age = 29 };
                entityStore.Insert(person);

                //If you are using non-typed documents you have to pass
                //the entityname string to the method you are using.
                var anonymousPerson = new { Name = "Daniel" };
                entityStore.Insert("Person", anonymousPerson);

                //So the EntityStore only allows for an abstraction
                //over the Db, Collection and Document hierarchy
                //and you can of course access these to.
                var db = session[DbName];
                var persons = db["Person"];
                persons.Insert(anonymousPerson);

                //The EntityStore also holds the Database that it wraps.
                //Of course you can obtain a collection using generics against the
                //database.
                var persons2 = entityStore.Database.GetCollection<Person>();
                persons2.Insert(person);

                var numOfStoredDocuments = entityStore.Count<Person>();
                Assert.AreEqual(4, numOfStoredDocuments);
            }
        }

        [TestMethod]
        public void ParentChildReference_Example()
        {
            var cn = CreateConnection();
            using (var session = new SimoSession(cn))
            {
                var entityStore = new SimoEntityStore(session, DbName);

                //The parent generates a new _id when created.
                //That _id is then used in the reference which is attached to the child.
                //After that, you just store the items.
                var parent = new Parent { Name = "Daniel" };
                var fatherReference = entityStore.Reference<Parent>(parent._id);
                var child = new Child { Name = "Isabell", Father = fatherReference };

                //You could of course have created the reference manually, but then you loose the
                //use of the pluralizer, and have to role-this on your own.
                //new SimoReference { CollectionName = Constants.Collections.ParentsCollectionName, Id = parent._id };

                entityStore.Insert(parent);
                entityStore.Insert(child);

                var refetchedChild = entityStore.FindOne<Child>(new { child._id });

                Assert.AreEqual(fatherReference.Id, refetchedChild.Father.Id);
                Assert.AreEqual(fatherReference.CollectionName, refetchedChild.Father.CollectionName);
            }
        }

        [TestMethod]
        public void DisablePluralizer_Example()
        {
            var cn = CreateConnection();
            using (var session = new SimoSession(cn))
            {
                //Just call Disable on the Pluralizer
                //When you want to enable it again, call
                //Enable().
                var entityStore = new SimoEntityStore(session, DbName);
                entityStore.Session.Pluralizer.Disable();

                var person = new Person { Name = "Daniel" };
                entityStore.Insert(person);

                var refetched = entityStore.Database["Person"].FindOne<Person>(new { person.Name });

                Assert.IsNotNull(refetched);
            }
        }

        [TestMethod]
        public void SessionFactory_Example()
        {
            //So there's a verry simple SessionFactory inplace,
            //but you can of course make one of your own or
            //let an IoC return sessions.

            var sessionFactory = new SimoSessionFactory();
            using (var session = sessionFactory.GetSession(Constants.ConnectionStringName))
            {
                session[DbName].DropDatabase();
            }
        }
    }
}