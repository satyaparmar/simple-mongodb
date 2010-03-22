using System;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.SimpleMongoDb.Tests.IntegrationTests.TestModel
{
    [Serializable]
    public class PersonWithId
        : Person
    {
        public SimoId _id { get; set; }
    }
}