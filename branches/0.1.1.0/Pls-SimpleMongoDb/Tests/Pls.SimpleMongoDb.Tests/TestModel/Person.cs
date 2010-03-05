using System;

namespace Pls.SimpleMongoDb.Tests.TestModel
{
    [Serializable]
    public class Person
    {
        public virtual DateTime TempDate { get; set; }
        public virtual string Name { get; set; }
        public virtual int Age { get; set; }
        public virtual int[] WorkDays { get; set; }
    }
}