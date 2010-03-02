using System;

namespace Pls.SimpleMongoDb.Tests.TestModel
{
    [Serializable]
    public class Owner
        : SimoDocument
    {
        public virtual string Name { get; set; }
    }
}