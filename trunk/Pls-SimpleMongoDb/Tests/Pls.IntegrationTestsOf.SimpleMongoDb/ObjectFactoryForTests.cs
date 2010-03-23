using Pls.SimpleMongoDb;

namespace Pls.IntegrationTestsOf.SimpleMongoDb
{
    internal static class ObjectFactoryForTests
    {
        internal static ISimoConnection CreateConnection()
        {
            return new SimoConnection(new SimoConnectionInfo(Constants.ConnectionStringName));
        }
    }
}