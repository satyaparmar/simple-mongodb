using System;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.SimpleMongoDb.Tests.IntegrationTests.TestModel
{
    [Serializable]
    public class PersonWithAutoId
        : Person
    {
        private SimoAutoId _simoAutoId = new SimoAutoId();

        public SimoId _id
        {
            get { return _simoAutoId; }
            set { _simoAutoId = value; }
        }
    }
}