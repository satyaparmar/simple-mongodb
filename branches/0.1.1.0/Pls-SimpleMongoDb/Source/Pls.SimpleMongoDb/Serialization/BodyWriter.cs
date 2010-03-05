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

        public virtual Encoding Encoding
        {
            get { return SerializationConsts.DefaultEncoding; }
        }

        public BodyWriter(Stream bodyStream)
        {
            _writer = new BinaryWriter(bodyStream, Encoding);
            _documentWriter = new DocumentWriter(bodyStream);
        }

        public virtual void Dispose()
        {
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }

            if(_documentWriter != null)
            {
                _documentWriter.Dispose();
                _documentWriter = null;
            }
        }

        public virtual void Write(int value)
        {
            _writer.Write(value);
        }

        public virtual void Write(string value)
        {
            _writer.Write(Encoding.GetBytes(value));
        }

        public virtual void WriteTerminator()
        {
            _writer.Write((byte)0);
        }

        public virtual void WriteDocument(object value)
        {
            if (value is string)
                WriteJson(value as string);
            else
                WriteAsBson(value);
        }

        public virtual void WriteSelector(object value)
        {
            if(value is string)
                WriteJson(value as string);
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

        public virtual void WriteJson(string json)
        {
            WriteAsBson(new SimoJson(json));
        }

        public virtual void WriteAsBson(object value)
        {
            _documentWriter.WriteDocument(value);
        }
    }
}