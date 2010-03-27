using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Pls.SimpleMongoDb.Serialization
{
    public class DocumentWriter
    {
        private readonly JsonSerializer _jsonSerializer;
        private readonly BsonWriter _bsonWriter;

        public DocumentWriter(Stream documentStream)
        {
            _jsonSerializer = new JsonSerializerFactory().Create();
            _bsonWriter = new BsonWriter(documentStream) { Formatting = Formatting.None };
        }

        public void WriteDocument(object document)
        {
            _jsonSerializer.Serialize(_bsonWriter, document);
            _bsonWriter.Flush();
        }
    }
}