using System;

namespace Pls.SimpleMongoDb.Operators
{
    [Serializable]
    public class SimoOperator : ISimoOperator
    {
        public string Key { get; private set; }

        public string Expression { get; set; }

        public SimoOperator(string key)
        {
            Key = key;
        }
    }
}