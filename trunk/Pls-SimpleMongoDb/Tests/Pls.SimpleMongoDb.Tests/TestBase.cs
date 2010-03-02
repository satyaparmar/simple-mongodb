namespace Pls.SimpleMongoDb.Tests
{
    public abstract class TestBase
        
    {
        protected virtual ISimoConnection CreateConnection()
        {
            return ObjectFactoryForTests.CreateConnection();
        }
    }
}