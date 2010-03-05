namespace Pls.SimpleMongoDb.Tests
{
    internal static class ObjectFactoryForTests
    {
        internal static ISimoConnection CreateConnection()
        {
            return new SimoConnection(new SimoConnectionInfo(Constants.ConnectionStringName));
        }
    }
}