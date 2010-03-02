using System;

namespace Pls.SimpleMongoDb.Tests.TestModel
{
    [Serializable]
    public class Car
        : SimoDocument
    {
        public virtual string RegNo { get; set; }
        public virtual Owner Owner { get; set; }
    }
}