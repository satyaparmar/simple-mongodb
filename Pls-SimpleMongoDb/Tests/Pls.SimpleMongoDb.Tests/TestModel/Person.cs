using System;

namespace Pls.SimpleMongoDb.Tests.TestModel
{
    [Serializable]
    public class Person
    {
        public DateTime TempDate { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int[] WorkDays { get; set; }
    }
}