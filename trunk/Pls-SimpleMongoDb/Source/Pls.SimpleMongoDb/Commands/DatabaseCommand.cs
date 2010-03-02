using System.IO;
using Newtonsoft.Json.Linq;
using Pls.SimpleMongoDb.Resources;
using Pls.SimpleMongoDb.Serialization;

namespace Pls.SimpleMongoDb.Commands
{
    public class DatabaseCommand
        : SimoCommand
    {
        /// <summary>
        /// Defines which DB the command should be executed against.
        /// </summary>
        public virtual string DatabaseName { get; set; }

        /// <summary>
        /// Defines the command to execute.
        /// <![CDATA[E.g. dropDatabase : 1]]>
        /// </summary>
        public virtual object Command { get; set; }

        public DatabaseCommand(ISimoConnection connection)
            : base(connection)
        {
        }

        protected override void OnEnsureValidForExecution()
        {
            if (string.IsNullOrEmpty(DatabaseName))
                throw new SimoCommandException(Exceptions.Command_MissingFullCollectionName);
        }

        protected override Request GenerateRequest()
        {
            return new Request(OpCodes.Query, GenerateBody());
        }

        protected virtual byte[] GenerateBody()
        {
            //http://www.mongodb.org/display/DOCS/Mongo+Wire+Protocol#MongoWireProtocol-OPQUERY
            //int32     ZERO;                   // 0 - reserved for future use
            //cstring   fullCollectionName;     // "dbname.collectionname"
            //int32     numberToSkip;           // number of documents to skip when returning results
            //int32     numberToReturn;         // number of documents to return in the first OP_REPLY
            //BSON      query ;                 // query object.  See below for details.
            //BSON      returnFieldSelector;    // OPTIONAL : selector indicating the fields to return.  See below for details.

            byte[] result;
            
            using (var stream = new MemoryStream())
            {
                using (var writer = new BodyWriter(stream))
                {
                    writer.Write((int)QueryOptions.None);
                    writer.Write(DatabaseName + ".$cmd");
                    writer.WriteTerminator();
                    writer.Write(0);
                    writer.Write(-1);

                    writer.WriteSelector(Command);
                }

                result = stream.ToArray();
            }

            return result;
        }

        protected override void OnReadResponse(ResponseReader responseReader)
        {
            var response = responseReader.Read<JContainer>();
            var document = response.ReturnedDocuments[0];
            var commandWasOk = document.Value<double>("ok") == 1.0;

            if(!commandWasOk)
                throw new SimoCommandException(Exceptions.DatabaseCommand_CommandWasNotOk);
        }
    }
}