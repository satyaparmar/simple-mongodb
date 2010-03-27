using Pls.SimpleMongoDb.Serialization;

namespace Pls.SimpleMongoDb.Commands
{
    public abstract class SimoResponseCommand<TDocument>
        : SimoCommand, ISimoResponseCommand
        where TDocument : class
    {
        private SimoCommandResponse<TDocument> _response;

        public SimoCommandResponse<TDocument> Response
        {
            get { return _response; }
        }

        protected SimoResponseCommand(ISimoConnection connection)
            : base(connection)
        {
            InitializeResponse();
        }

        protected override void OnExecute(ISimoConnection connection)
        {
            InitializeResponse();

            base.OnExecute(connection);
        }

        private void InitializeResponse()
        {
            _response = new SimoCommandResponse<TDocument>();
        }

        protected override void OnReadResponse(ResponseStreamReader responseStreamReader)
        {
            var requestResponse = responseStreamReader.Read<TDocument>();

            Response.SetDocuments(requestResponse.ReturnedDocuments);

            GetMoreCommand<TDocument> getMoreCommand = null;

            while (requestResponse.HasMoreResponse)
            {
                if(getMoreCommand == null)
                    getMoreCommand = new GetMoreCommand<TDocument>(Connection, requestResponse.CursorId.Value, requestResponse.NumberOfReturnedDocuments.Value) { NodeName = NodeName };

                getMoreCommand.Execute();

                Response.AddDocuments(getMoreCommand.Response.Documents);

                if(getMoreCommand.Response.NumberOfDocuments < 1 || !getMoreCommand.CursorHasMoreResult)
                    break;
            }

            //Note that if a cursor is read until exhausted
            //(read until OP_QUERY or OP_GETMORE returns zero for the cursor id),
            //there is no need to kill the cursor.
        }
    }
}