using System;

namespace Pls.SimpleMongoDb.Tests.TestModel
{
    [Serializable]
    public class Owner
    {
        public virtual SimoObjectId _id { get; set; }
        public virtual string Name { get; set; }
    }
}