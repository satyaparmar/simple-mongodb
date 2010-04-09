namespace Pls.SimpleMongoDb
{
    /// <summary>
    /// Simple class to represent the reference between a lazy relation.
    /// NOTE: A SimoReference can be used instead.
    /// </summary>
    internal class EntityReference
    {
        public string C { get; set; }
        public object I { get; set; }
    }
}