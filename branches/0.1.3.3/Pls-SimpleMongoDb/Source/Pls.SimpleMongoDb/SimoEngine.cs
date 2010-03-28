using Pls.SimpleMongoDb.IoC;

namespace Pls.SimpleMongoDb
{
    public class SimoEngine : ISimoEngine
    {
        public static SimoEngine Instance { get; private set; }

        public ISimoIoC IoC { get; private set; }
        
        static SimoEngine()
        {
            Instance = new SimoEngine();
        }

        private SimoEngine()
        {
            IoC = new SimoIoC();
        }
    }
}