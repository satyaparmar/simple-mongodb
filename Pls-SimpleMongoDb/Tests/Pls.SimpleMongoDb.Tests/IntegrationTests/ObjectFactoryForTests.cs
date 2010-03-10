namespace Pls.SimpleMongoDb.Tests.IntegrationTests
{
    internal static class ObjectFactoryForTests
    {
        internal static ISimoConnection CreateConnection()
        {
            return new SimoConnection(new SimoConnectionInfo(Constants.ConnectionStringName));
        }
    }
}