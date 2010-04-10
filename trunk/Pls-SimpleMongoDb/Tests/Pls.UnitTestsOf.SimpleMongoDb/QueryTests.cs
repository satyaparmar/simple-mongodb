using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pls.SimpleMongoDb;

namespace Pls.UnitTestsOf.SimpleMongoDb
{
    [TestClass]
    public class QueryTests
    {
        [TestMethod]
        public void In_SeveralStringOperands_BuildsCorrectFormat()
        {
            var q = new Query()["Name"].In("Daniel", "Sue");

            Assert.AreEqual("{ Name : { $in : ['Daniel','Sue'] } }", q.ToString());
        }

        [TestMethod]
        public void In_ChainedOnSameProperty_BuildsCorrectFormat()
        {
            Assert.Inconclusive("TBD");
            //var q = new Query()["Name"].In("Daniel").In("Sue");

            //Assert.AreEqual("{ Name : { $in : ['Daniel','Sue'] } }", q.ToString());
        }

        [TestMethod]
        public void In_SeveralIntOperands_BuildsCorrectFormat()
        {
            var q = new Query()["Name"].In(21, 22);

            Assert.AreEqual("{ Name : { $in : [21,22] } }", q.ToString());
        }

        [TestMethod]
        public void InAndIn_WithStringsAndInts_BuildsCorrectFormat()
        {
            var q = new Query()["Name"].In("Daniel", "Sue").And("Age").In(21, 22);

            Assert.AreEqual("{ Name : { $in : ['Daniel','Sue'] }, Age : { $in : [21,22] } }", q.ToString());
        }

        [TestMethod]
        public void Where_WithStringAndInt_BuildsCorrectFormat()
        {
            var q = new Query().Where(@"this.Name == 'Daniel' || this.Age == 21");

            Assert.AreEqual(@"{ $where : "" this.Name == 'Daniel' || this.Age == 21 "" }", q.ToString());
        }

        [TestMethod]
        public void InAndWhere_InFirstAndWhereSecond_BuildsCorrectFormat()
        {
            var q = new Query()["Name"].In("Daniel", "Sue").And().Where(@"this.Age == 21 || this.Age == 22");

            Assert.AreEqual(@"{ Name : { $in : ['Daniel','Sue'] }, $where : "" this.Age == 21 || this.Age == 22 "" }", q.ToString());
        }

        [TestMethod]
        public void InAndWhere_WhereFirstAndInSecond_BuildsCorrectFormat()
        {
            var q = new Query().Where(@"this.Age == 21 || this.Age == 22").And("Name").In("Daniel", "Sue");

            Assert.AreEqual(@"{ $where : "" this.Age == 21 || this.Age == 22 "", Name : { $in : ['Daniel','Sue'] } }", q.ToString());
        }

        [TestMethod]
        public void Lt_WhitInt_BuildsCorrectFormat()
        {
            var q = new Query()["Age"].Lt(50);

            Assert.AreEqual(@"{ Age : { $lt : 50 } }", q.ToString());
        }

        [TestMethod]
        public void LtE_WhitInt_BuildsCorrectFormat()
        {
            var q = new Query()["Age"].LtE(50);

            Assert.AreEqual(@"{ Age : { $lte : 50 } }", q.ToString());
        }

        [TestMethod]
        public void Gt_WhitInt_BuildsCorrectFormat()
        {
            var q = new Query()["Age"].Gt(50);

            Assert.AreEqual(@"{ Age : { $gt : 50 } }", q.ToString());
        }

        [TestMethod]
        public void GtE_WhitInt_BuildsCorrectFormat()
        {
            var q = new Query()["Age"].GtE(50);

            Assert.AreEqual(@"{ Age : { $gte : 50 } }", q.ToString());
        }

        [TestMethod]
        public void InAndLt_ChainedOnSameProperty_BuildsCorrectFormat()
        {
            var q = new Query()["Age"].In(21, 22, 23).Lt(23);
            
            Assert.AreEqual(@"{ Age : { $in : [21,22,23], $lt : 23 } }", q.ToString());
        }
    }
}