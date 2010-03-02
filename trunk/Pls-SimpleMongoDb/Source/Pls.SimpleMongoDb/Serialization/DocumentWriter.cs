using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Pls.SimpleMongoDb.Serialization
{
    public class DocumentWriter 
        : IDisposable
    {
        private readonly Stream _documentStream;
        private readonly JsonSerializer _jsonSerializer;
        private BsonWriter _bsonWriter;

        public DocumentWriter(Stream documentStream)
        {
            _documentStream = documentStream;
            _jsonSerializer = _jsonSerializer = new JsonSerializerFactory().Create();
            _bsonWriter = new BsonWriter(_documentStream) { Formatting = Formatting.None };
        }

        public virtual void Dispose()
        {
            if (_bsonWriter != null)
            {
                _bsonWriter.Flush();
                _bsonWriter.Close();
                ((IDisposable)_bsonWriter).Dispose();
                _bsonWriter = null;
            }
        }

        public virtual void WriteDocument(object document)
        {
            ////TODO: Ensure that BSON-writer doesn't have to be recreated as the reader.
            //using (var writer = new BsonWriter(_documentStream) { Formatting = Formatting.None })
            //{
                _jsonSerializer.Serialize(_bsonWriter, document);
            _bsonWriter.Flush();
            //}
        }
    }
}