using System;

namespace Pls.SimpleMongoDb.Tests.IntegrationTests.TestModel
{
    [Serializable]
    public class Car
    {
        public string RegNo { get; set; }
        public Owner Owner { get; set; }
    }
}