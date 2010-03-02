using System;
using System.IO;
using System.Text;

namespace Pls.SimpleMongoDb.Serialization
{
    /// <summary>
    /// Is used to generate a Request-stream, containing both the Request-header
    /// and the body containing the specific request parameters.
    /// </summary>
    public class RequestWriter
        : IDisposable
    {
        private BinaryWriter _writer;

        public virtual Encoding Encoding
        {
            get { return SerializationConsts.DefaultEncoding; }
        }

        public RequestWriter(Stream targetStream)
        {
            _writer = new BinaryWriter(targetStream, Encoding);
        }

        public virtual void Dispose()
        {
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }
        }

        public virtual void Write(Request request)
        {
            OnWriteHeader(request);
            OnWriteBody(request.BodyBuffer);
        }

        protected virtual void OnWriteHeader(Request request)
        {
            _writer.Write(request.TotalLength);
            _writer.Write(request.RequestId);
            _writer.Write(request.ResponseTo);
            _writer.Write((int)request.OpCode);
            _writer.Flush();
        }

        protected virtual void OnWriteBody(byte[] bodyBuff)
        {
            _writer.Write(bodyBuff);
            _writer.Flush();
        }
    }
}