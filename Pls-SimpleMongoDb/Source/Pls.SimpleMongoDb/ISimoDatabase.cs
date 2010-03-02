namespace Pls.SimpleMongoDb
{
    public interface ISimoDatabase
    {
        ISimoConnection Connection { get; }
        string Name { get; }
        ISimoCollection this[string name] { get; }
    }
}