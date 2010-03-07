using System;
using Pls.SimpleMongoDb.Utils;

namespace Pls.SimpleMongoDb
{
    [Serializable]
    public class SimoReference
    {
        public string FullCollectionName { get; set; }

        public string DatabaseName
        {
            get
            {
                return FullCollectionName.GetPartOrDefault(".", 0);
            }
        }

        public string CollectionName
        {
            get
            {
                return FullCollectionName.GetPartOrDefault(".", 1);
            }
        }

        public object _id { get; set; }
    }
}