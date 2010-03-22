using System;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.SimpleMongoDb.Tests.IntegrationTests.TestModel
{
    [Serializable]
    public class Parent
    {
        private readonly SimoAutoId _simoAutoId = new SimoAutoId();

        public SimoId _id
        {
            get { return _simoAutoId; }
        }

        public virtual string Name { get; set; }
    }
}