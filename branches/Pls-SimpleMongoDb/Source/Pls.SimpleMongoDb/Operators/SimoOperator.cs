using System;

namespace Pls.SimpleMongoDb.Operators
{
    [Serializable]
    public class SimoOperator : ISimoOperator
    {
        public virtual string Key { get; private set; }

        public virtual string Expression { get; set; }

        public SimoOperator(string key)
        {
            Key = key;
        }
    }
}