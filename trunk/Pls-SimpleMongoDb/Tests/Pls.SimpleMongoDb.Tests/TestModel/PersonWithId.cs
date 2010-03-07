using System;

namespace Pls.SimpleMongoDb.Tests.TestModel
{
    [Serializable]
    public class PersonWithId
        : Person
    {
        public SimoObjectId _id { get; set; }
    }
}