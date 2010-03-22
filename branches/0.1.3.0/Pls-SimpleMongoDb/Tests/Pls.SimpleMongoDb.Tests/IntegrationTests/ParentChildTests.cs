using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb.DataTypes;
using Pls.SimpleMongoDb.Tests.IntegrationTests.TestModel;

namespace Pls.SimpleMongoDb.Tests.IntegrationTests
{
    [TestClass]
    public class ParentChildTests
        : DbTestBase
    {
        [TestMethod]
        public void NewRelation_UsingStaticTypes_ReferenceCreated()
        {
            var parent = new Parent { Name = "Daniel" };
            var fatherReference = new SimoReference { CollectionName = Constants.Collections.ParentsCollectionName, Id = parent._id };
            var child = new Child { Name = "Isabell", FatherReference = fatherReference };
            InsertDocuments(Constants.Collections.ParentsFullCollectionName, parent);
            InsertDocuments(Constants.Collections.ChildsFullCollectionName, child);

            var refetchedChild = GetDocument<Child>(new { child._id }, Constants.Collections.ChildsFullCollectionName);

            Assert.AreEqual(fatherReference.Id, refetchedChild.FatherReference.Id);
            Assert.AreEqual(fatherReference.CollectionName, refetchedChild.FatherReference.CollectionName);
        }

        [TestMethod]
        public void NewRelation_UsingAnonymousTypes_ReferenceCreated()
        {
            var parentId = SimoId.NewId();
            var parent = new { _id = parentId, Name = "Daniel" };
            var fatherReference = new SimoReference
            {
                CollectionName = Constants.Collections.ParentsCollectionName,
                Id = parent._id
            };
            var child = new { _id = SimoId.NewId(), Name = "Isabell", Father = fatherReference };
            InsertDocuments(Constants.Collections.ParentsFullCollectionName, parent);
            InsertDocuments(Constants.Collections.ChildsFullCollectionName, child);

            var refetchedChild = GetDocument(new { child._id }, child, Constants.Collections.ChildsFullCollectionName);

            Assert.AreEqual(fatherReference.Id, refetchedChild.Father.Id);
            Assert.AreEqual(fatherReference.CollectionName, refetchedChild.Father.CollectionName);
        }

        [TestMethod]
        public void FetchReference_InSameDbButDifferentCollection_Fetched()
        {
            var parent = new Parent { Name = "Daniel" };
            var fatherReference = new SimoReference
            {
                CollectionName = Constants.Collections.ParentsCollectionName,
                Id = parent._id
            };
            var child = new Child { Name = "Isabell", FatherReference = fatherReference };
            InsertDocuments(Constants.Collections.ParentsFullCollectionName, parent);
            InsertDocuments(Constants.Collections.ChildsFullCollectionName, child);

            var refetchedChild = GetDocument<Child>(new { child._id }, Constants.Collections.ChildsFullCollectionName);

            Assert.AreEqual(fatherReference.Id, refetchedChild.FatherReference.Id);
            Assert.AreEqual(fatherReference.CollectionName, refetchedChild.FatherReference.CollectionName);
        }
    }
}


