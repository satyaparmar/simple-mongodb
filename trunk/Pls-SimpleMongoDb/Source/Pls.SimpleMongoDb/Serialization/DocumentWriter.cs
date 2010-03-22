using System;
using System.IO;
using Newtonsoft.Json2;
using Newtonsoft.Json2.Bson;

namespace Pls.SimpleMongoDb.Serialization
{
    public class DocumentWriter
        : IDisposable
    {
        private readonly JsonSerializer _jsonSerializer;
        private BsonWriter _bsonWriter;

        public DocumentWriter(Stream documentStream)
        {
            _jsonSerializer = _jsonSerializer = new JsonSerializerFactory().Create();
            _bsonWriter = new BsonWriter(documentStream) { Formatting = Formatting.None };
        }

        #region Object lifetime, Disposing

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// A call to Dispose(false) should only clean up native resources.
        /// A call to Dispose(true) should clean up both managed and native resources.
        /// </summary>
        /// <param name="disposeManagedResources"></param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
                DisposeManagedResources();
        }

        ~DocumentWriter()
        {
            Dispose(false);
        }

        protected virtual void DisposeManagedResources()
        {
            if (_bsonWriter != null)
            {
                _bsonWriter.Flush();
                _bsonWriter.Close();
                ((IDisposable)_bsonWriter).Dispose();
                _bsonWriter = null;
            }
        }

        #endregion

        public void WriteDocument(object document)
        {
            _jsonSerializer.Serialize(_bsonWriter, document);
            _bsonWriter.Flush();
        }
    }
}