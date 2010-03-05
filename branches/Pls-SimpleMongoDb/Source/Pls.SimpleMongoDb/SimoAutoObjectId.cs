using System;

namespace Pls.SimpleMongoDb
{
    [Serializable]
    public class SimoAutoObjectId
    {
        private SimoObjectId ObjectId { get; set; }

        public SimoAutoObjectId()
            : this(SimoObjectId.NewId())
        {
        }

        private SimoAutoObjectId(SimoObjectId objectId)
        {
            ObjectId = objectId;
        }

        public static implicit operator SimoObjectId(SimoAutoObjectId simoAutoObjectId)
        {
            return simoAutoObjectId.ObjectId;
        }

        public static implicit operator SimoAutoObjectId(SimoObjectId objectId)
        {
            return new SimoAutoObjectId(objectId);
        }
    }
}