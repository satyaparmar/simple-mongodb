using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.Serialization;

namespace Pls.SimpleMongoDb.Tests.Lazy
{
    [TestClass]
    public class ProxyTests
    {

        [TestMethod]
        public void NewRelation_Using_Converter() {
            var father = new TempPerson { Name = "The Dad" };
            var child = new TempPerson { Name = "The Son", Father = father };

            var tr = new TypeRegistry(CreateConnection);

            tr.Class<TempPerson>(x => {
                x.Database = "TestDb";
                x.CollectionName = "Persons";
                x.Id(id => id._id);
            });

            var meta = tr.GetMeta(typeof(TempPerson));

            JsonSerializerFactory.ContractResolver = new ProxyContractResolver(tr);

            Insert(meta.Database, meta.CollectionName, father);
            Insert(meta.Database, meta.CollectionName, child);

            var refetchedChild = GetDocuments<TempPerson>(meta.Database, meta.CollectionName, new { child._id }).First();

            Assert.AreEqual(refetchedChild.Father._id, father._id);
            Assert.AreEqual(refetchedChild.Father.Name, father.Name);
        }

        #region Utils

        // TODO: These should all be in a base test class.


        private static ISimoSession CreateSession() {
            return new SimoSession(CreateConnection());
        }

        private static void Insert<TItem>(string db, string collectionName, TItem item) {
            using (var session = CreateSession()) {
                session[db][collectionName].Insert(item);
            }
        }

        private static IList<TItem> GetDocuments<TItem>(string db, string collectionName, object selector) where TItem : class {
            using (var session = CreateSession()) {
                return session[db][collectionName].Find<TItem>(selector);
            }
        }

        private static ISimoConnection CreateConnection() {
            return new SimoConnection(new SimoConnectionInfo("SimpleMongoDbTests"));
        }

        #endregion

    }

    #region Test Types

    public class TempPerson
    {
        public TempPerson() {
            _id = Guid.NewGuid();
        }

        public virtual Guid _id { get; set; }

        public virtual string Name { get; set; }

        public virtual TempPerson Father { get; set; }
    }

    #endregion

}