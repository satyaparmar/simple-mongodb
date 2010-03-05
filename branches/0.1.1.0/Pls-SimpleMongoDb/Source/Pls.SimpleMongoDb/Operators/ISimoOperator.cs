namespace Pls.SimpleMongoDb.Operators
{
    public interface ISimoOperator
    {
        string Key { get; }
        string Expression { get; set; }
    }
}