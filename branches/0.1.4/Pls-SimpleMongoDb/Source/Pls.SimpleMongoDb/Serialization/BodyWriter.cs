using System;
using System.IO;
using System.Text;
using Pls.SimpleMongoDb.DataTypes;

namespace Pls.SimpleMongoDb.Serialization
{
    /// <summary>
    /// Is used to generate a body-stream.
    /// </summary>
    public class BodyWriter
        : IDisposable
    {
        private BinaryWriter _binaryWriter;
        private DocumentWriter _documentWriter;

        public Encoding Encoding
        {
            get { return SerializationConsts.DefaultEncoding; }
        }

        public BodyWriter(Stream bodyStream)
        {
            _binaryWriter = new BinaryWriter(bodyStream, Encoding);
            _documentWriter = new DocumentWriter(bodyStream);
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

        ~BodyWriter()
        {
            Dispose(false);
        }

        protected virtual void DisposeManagedResources()
        {
            if (_binaryWriter != null)
            {
                _binaryWriter.Dispose();
                _binaryWriter = null;
            }
        }

        #endregion

        public void Write(int value)
        {
            _binaryWriter.Write(value);
        }

        public void Write(long value)
        {
            _binaryWriter.Write(value);
        }

        public void Write(string value)
        {
            _binaryWriter.Write(Encoding.GetBytes(value));
        }

        public void WriteTerminator()
        {
            _binaryWriter.Write((byte)0);
        }

        public void WriteDocument(object value)
        {
            if (value is string)
                WriteJson(value.ToString());
            else if (value is SimoJson)
                WriteJson(value as SimoJson);
            else
                WriteAsBson(value);
        }

        public void WriteSelector(object value)
        {
            if (value is string)
                WriteJson(value.ToString());
            else if (value is SimoJson)
                WriteJson(value as SimoJson);
            else
                WriteAsBson(value);
        }

        private void WriteJson(SimoJson json)
        {
            WriteAsBson(json.ToKeyValue());
        }

        private void WriteAsBson(object value)
        {
            _documentWriter.WriteDocument(value);
        }
    }
}