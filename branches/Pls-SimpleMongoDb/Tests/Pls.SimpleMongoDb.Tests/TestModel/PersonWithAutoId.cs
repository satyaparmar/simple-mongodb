using System;

namespace Pls.SimpleMongoDb.Tests.TestModel
{
    [Serializable]
    public class PersonWithAutoId
        : Person
    {
        private SimoAutoObjectId _simoAutoObjectId = new SimoAutoObjectId();

        public virtual SimoObjectId _id
        {
            get { return _simoAutoObjectId; }
            set { _simoAutoObjectId = value; }
        }
    }
}