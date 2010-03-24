using Pls.SimpleMongoDb.Serialization;

namespace Pls.SimpleMongoDb.Commands
{
    public abstract class SimoResponseCommand<TDocument>
        : SimoCommand, ISimoResponseCommand
        where TDocument : class
    {
        private readonly SimoCommandResponse<TDocument> _response;

        public SimoCommandResponse<TDocument> Response
        {
            get { return _response; }
        }

        /// <summary>
        /// Sets the number of documents to omit - starting from
        /// the first document in the resulting dataset - when
        /// returning the result of the query.
        /// </summary>
        public int NumberOfDocumentsToSkip { get; set; }

        /// <summary>
        /// Number of documents to return in the first reply.
        /// If numberToReturn is 0, the db will used the default
        /// return size. If the number is negative, then the
        /// database will return that number and close the cursor.
        /// No futher results for that query can be fetched.
        /// </summary>
        public int NumberOfDocumentsToReturn { get; set; }

        protected SimoResponseCommand(ISimoConnection connection)
            : base(connection)
        {
            _response = new SimoCommandResponse<TDocument>();
        }

        protected override void OnReadResponse(ResponseReader responseReader)
        {
            var response = responseReader.Read<TDocument>();

            Response.Initialize(response);
        }
    }
}