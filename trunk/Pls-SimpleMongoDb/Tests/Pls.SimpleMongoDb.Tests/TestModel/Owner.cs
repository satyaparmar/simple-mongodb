using System;

namespace Pls.SimpleMongoDb.Tests.TestModel
{
    [Serializable]
    public class Owner
    {
        public SimoObjectId _id { get; set; }
        public string Name { get; set; }
    }
}