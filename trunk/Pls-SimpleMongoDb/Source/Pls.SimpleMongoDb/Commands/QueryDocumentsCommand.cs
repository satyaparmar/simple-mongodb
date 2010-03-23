using System.Collections.Generic;
using System.IO;
using Pls.SimpleMongoDb.Resources;
using Pls.SimpleMongoDb.Serialization;

namespace Pls.SimpleMongoDb.Commands
{
    /// <summary>
    /// Command used for querying for documents.
    /// </summary>
    public class QueryDocumentsCommand<TDocument>
        : SimoCommand
        where TDocument : class
    {
        private readonly List<TDocument> _response;

        public override bool CanHandleResponses
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Defines which DB and Collection the command should be executed against.
        /// E.g <![CDATA["dbname.collectionname"]]>.
        /// </summary>
        public string FullCollectionName { get; set; }

        /// <summary>
        /// Defines how the result will be returned.
        /// </summary>
        private QueryOptions QueryOption { get; set; }

        /// <summary>
        /// Sets the number of documents to omit - starting from
        /// the first document in the resulting dataset - when
        /// returning the result of the query.
        /// </summary>
        private int NumberOfDocumentsToSkip { get; set; }

        /// <summary>
        /// Number of documents to return in the first reply.
        /// If numberToReturn is 0, the db will used the default
        /// return size. If the number is negative, then the
        /// database will return that number and close the cursor.
        /// No futher results for that query can be fetched.
        /// </summary>
        private int NumberOfDocumentsToReturn { get; set; }

        /// <summary>
        /// QuerySelector - defines the query criterias.
        /// </summary>
        public object QuerySelector { get; set; }

        /// <summary>
        /// Optional. Lets you control the fields to return.
        /// </summary>
        public object DocumentSchema { get; set; }

        /// <summary>
        /// Contains the returned documents.
        /// </summary>
        public IList<TDocument> Response
        {
            get
            {
                return _response;
            }
        }

        public QueryDocumentsCommand(ISimoConnection connection)
            : base(connection)
        {
            QueryOption = QueryOptions.None;
            NumberOfDocumentsToSkip = 0;
            NumberOfDocumentsToReturn = 0;
            _response = new List<TDocument>();
        }

        protected override void OnEnsureValidForExecution()
        {
            if (string.IsNullOrEmpty(FullCollectionName))
                throw new SimoCommandException(ExceptionMessages.Command_MissingFullCollectionName);
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

            using (var stream = new MemoryStream())
            {
                using (var writer = new BodyWriter(stream))
                {
                    writer.Write((int)QueryOption);
                    writer.Write(FullCollectionName);
                    writer.WriteTerminator();
                    writer.Write(NumberOfDocumentsToSkip);
                    writer.Write(NumberOfDocumentsToReturn);

                    writer.WriteSelector(QuerySelector ?? new object());
                    
                    if(DocumentSchema != null)
                        writer.WriteDocument(DocumentSchema);
                }

                return stream.ToArray();
            }
        }

        protected override void OnReadResponse(ResponseReader responseReader)
        {
            var response = responseReader.Read<TDocument>();

            _response.Clear();
            _response.AddRange(response.ReturnedDocuments);
        }
    }
}