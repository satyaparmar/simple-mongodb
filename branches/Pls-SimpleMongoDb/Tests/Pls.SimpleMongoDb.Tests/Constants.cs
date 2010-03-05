namespace Pls.SimpleMongoDb.Tests
{
    internal static class Constants
    {
        internal const string ConnectionStringName = "SimpleMongoDbTests";
        internal const string TestDbName = "TestDb";

        internal static class Commands
        {
            internal const string PersonsCollectionName = "Persons";
            internal const string CarsCollectionName = "Cars";

            internal const string PersonsFullCollectionName = TestDbName + "." + PersonsCollectionName;
            internal const string CarsFullCollectionName = TestDbName + "." + CarsCollectionName;
        }
    }
}
