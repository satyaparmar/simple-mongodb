using System;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.SimpleMongoDb.Tests.IntegrationTests.TestModel
{
    [Serializable]
    public class Child
    {
        private SimoAutoId _simoAutoId = new SimoAutoId();

        public SimoId _id
        {
            get { return _simoAutoId; }
        }

        public virtual string Name { get; set; }
        public virtual SimoReference Father { get; set; }
    }
}