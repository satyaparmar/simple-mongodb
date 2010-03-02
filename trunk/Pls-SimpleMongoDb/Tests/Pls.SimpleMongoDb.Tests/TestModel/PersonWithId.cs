using System;

namespace Pls.SimpleMongoDb.Tests.TestModel
{
    [Serializable]
    public class PersonWithId
        : Person
    {
        public virtual SimoId _id { get; set; }
    }
}