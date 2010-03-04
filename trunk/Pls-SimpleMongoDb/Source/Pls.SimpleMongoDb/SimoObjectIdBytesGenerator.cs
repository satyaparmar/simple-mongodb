using System.Collections.Generic;

namespace Pls.SimpleMongoDb
{
    /// <summary>
    /// Generates a byte-array that is a valid representation of
    /// the MongoDB ObjectId, which is specified as:
    /// A BSON ObjectID is a 12-byte value consisting of 
    /// a 4-byte timestamp,
    /// a 3-byte machine id,
    /// a 2-byte process id, and
    /// a 3-byte counter.
    /// </summary>
    internal class SimoObjectIdBytesGenerator
    {
        
        private readonly SimoObjectIdMachinePart _machinePart;
        private readonly SimoObjectIdProcessPart _processPart;

        internal SimoObjectIdBytesGenerator()
        {
            _machinePart = new SimoObjectIdMachinePart();
            _processPart = new SimoObjectIdProcessPart();
        }

        internal byte[] New()
        {
            var bytes = new List<byte>(12);
            var timePart = new SimoObjectIdTimePart();
            var counterPart = new SimoObjectIdCounterPart();

            bytes.AddRange(timePart.Value);
            bytes.AddRange(_machinePart.Value);
            bytes.AddRange(_processPart.Value);
            bytes.AddRange(counterPart.Value);

            return bytes.ToArray();
        }
    }
}