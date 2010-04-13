using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.IntegrationTestsOf.SimpleMongoDb.TestModel;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    [TestClass]
    public class ParentChildTests
        : TestBase
    {
        [TestMethod]
        public void NewRelation_UsingStaticTypes_ReferenceCreated()
        {
            var parent = new Parent { Name = "Daniel" };
            var fatherReference = new SimoReference { CollectionName = Constants.Collections.ParentsCollectionName, Id = parent._id };
            var child = new Child { Name = "Isabell", FatherReference = fatherReference };
            TestHelper.InsertDocument(Constants.Collections.ParentsFullCollectionName, parent);
            TestHelper.InsertDocument(Constants.Collections.ChildsFullCollectionName, child);

            var refetchedChild = TestHelper.GetDocument<Child>(new { child._id }, Constants.Collections.ChildsFullCollectionName);

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
            TestHelper.InsertDocument(Constants.Collections.ParentsFullCollectionName, parent);
            TestHelper.InsertDocument(Constants.Collections.ChildsFullCollectionName, child);

            var refetchedChild = TestHelper.GetDocument(new { child._id }, child, Constants.Collections.ChildsFullCollectionName);

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
            TestHelper.InsertDocument(Constants.Collections.ParentsFullCollectionName, parent);
            TestHelper.InsertDocument(Constants.Collections.ChildsFullCollectionName, child);

            var refetchedChild = TestHelper.GetDocument<Child>(new { child._id }, Constants.Collections.ChildsFullCollectionName);

            Assert.AreEqual(fatherReference.Id, refetchedChild.FatherReference.Id);
            Assert.AreEqual(fatherReference.CollectionName, refetchedChild.FatherReference.CollectionName);
        }
    }
}