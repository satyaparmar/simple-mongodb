using System;

namespace Pls.SimpleMongoDb.Operators
{
    [Serializable]
    public class SimoOperator : ISimoOperator
    {
        private string _expression;

        public string Key { get; private set; }

        public string Expression
        {
            get { return _expression; }
            set { _expression = value ?? ""; }
        }

        public SimoOperator(string key)
        {
            Key = key;
        }
    }
}