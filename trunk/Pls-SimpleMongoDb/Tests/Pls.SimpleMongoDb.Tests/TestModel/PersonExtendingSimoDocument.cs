using System;

namespace Pls.SimpleMongoDb.Tests.TestModel
{
    [Serializable]
    public class PersonExtendingSimoDocument
        : SimoDocument
    {
        public virtual string Name { get; set; }
        public virtual int Age { get; set; }
    }
}