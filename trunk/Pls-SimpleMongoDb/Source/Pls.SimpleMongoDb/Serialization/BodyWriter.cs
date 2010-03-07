using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pls.SimpleMongoDb.Operators;

namespace Pls.SimpleMongoDb.Serialization
{
    /// <summary>
    /// Is used to generate a body-stream.
    /// </summary>
    public class BodyWriter
        : IDisposable
    {
        private BinaryWriter _writer;
        private DocumentWriter _documentWriter;

        public Encoding Encoding
        {
            get { return SerializationConsts.DefaultEncoding; }
        }

        public BodyWriter(Stream bodyStream)
        {
            _writer = new BinaryWriter(bodyStream, Encoding);
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
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }

            if (_documentWriter != null)
            {
                _documentWriter.Dispose();
                _documentWriter = null;
            }
        }

        #endregion

        public void Write(int value)
        {
            _writer.Write(value);
        }

        public void Write(string value)
        {
            _writer.Write(Encoding.GetBytes(value));
        }

        public void WriteTerminator()
        {
            _writer.Write((byte)0);
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
            else if(value is SimoJson)
                WriteJson(value as SimoJson);
            else
            {
                var @operator = value as ISimoOperator;

                if (@operator != null)
                    WriteSelector(@operator);
                else
                    WriteAsBson(value);    
            }
        }

        protected virtual void WriteSelector(ISimoOperator @operator)
        {
            var operatorDictionary = new Dictionary<string, object>
                        {
                            {@operator.Key, @operator.Expression}
                        };
            WriteAsBson(operatorDictionary);
        }

        public void WriteJson(SimoJson json)
        {
            WriteAsBson(json.ToKeyValue());
        }

        public void WriteAsBson(object value)
        {
            _documentWriter.WriteDocument(value);
        }
    }
}