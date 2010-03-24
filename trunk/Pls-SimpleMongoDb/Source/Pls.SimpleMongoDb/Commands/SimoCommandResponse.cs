using System;
using System.Collections.Generic;
using Pls.SimpleMongoDb.Serialization;

namespace Pls.SimpleMongoDb.Commands
{
    [Serializable]
    public class SimoCommandResponse<TDocument>
        where TDocument : class 
    {
        private readonly List<TDocument> _documents;

        public IList<TDocument> Documents
        {
            get { return _documents; }
        }

        public int NumberOfDocuments
        {
            get { return Documents.Count; }
        }

        public SimoCommandResponse()
        {
            _documents = new List<TDocument>();
        }
        
        public void Initialize(Response<TDocument> response)
        {
            SetDocuments(response.ReturnedDocuments);
        }

        private void SetDocuments(IList<TDocument> documents)
        {
            _documents.Clear();
            
            if(documents != null && documents.Count > 0)
                _documents.AddRange(documents);
        }
    }
}