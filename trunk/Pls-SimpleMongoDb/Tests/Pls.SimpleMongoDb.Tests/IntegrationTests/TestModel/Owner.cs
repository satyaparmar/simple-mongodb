using System;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.SimpleMongoDb.Tests.IntegrationTests.TestModel
{
    [Serializable]
    public class Owner
    {
        public SimoId _id { get; set; }
        public string Name { get; set; }
    }
}