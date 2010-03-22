namespace Pls.SimpleMongoDb
{
    public interface ISimoDatabase
    {
        ISimoSession Session { get; }
        string Name { get; }

        ISimoCollection this[string name] { get; }

        void DropDatabase();
        ISimoCollection GetCollection<T>() where T : class;
    }
}