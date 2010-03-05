using System;
using System.IO;
using Pls.SimpleMongoDb.Resources;
using Pls.SimpleMongoDb.Serialization;

namespace Pls.SimpleMongoDb.Commands
{
    /// <summary>
    /// Used to update documents.
    /// </summary>
    public class UpdateDocumentsCommand
        : SimoCommand
    {
        /// <summary>
        /// Defines which DB and Collection the command should be executed against.
        /// E.g <![CDATA["dbname.collectionname"]]>.
        /// </summary>
        public virtual string FullCollectionName { get; set; }

        /// <summary>
        /// Determines how the MongoDb-database will handle
        /// missing document and/or multiple matches. See
        /// <see cref="UpdateModes"/> for more info.
        /// </summary>
        public virtual UpdateModes Mode { get; set; }

        /// <summary>
        /// Defines the query object that is used to
        /// identify the document(s) to update.
        /// </summary>
        /// <remarks>Needs to be convertible to BSON.</remarks>
        public virtual object QuerySelector { get; set; }

        /// <summary>
        /// The document that will be updated or inserted depending on
        /// the <see cref="Mode"/>.
        /// </summary>
        /// <remarks>Needs to be convertible to BSON.</remarks>
        public virtual object Document { get; set; }

        public UpdateDocumentsCommand(ISimoConnection connection)
            : base(connection)
        {
            Mode = UpdateModes.Upsert;
        }

        protected override void OnEnsureValidForExecution()
        {
            if (string.IsNullOrEmpty(FullCollectionName))
                throw new SimoCommandException(Exceptions.Command_MissingFullCollectionName);
        }

        protected override Request GenerateRequest()
        {
            return new Request(OpCodes.Update, GenerateBody());
        }

        protected virtual byte[] GenerateBody()
        {
            //http://www.mongodb.org/display/DOCS/Mongo+Wire+Protocol#MongoWireProtocol-OPUPDATE
            //int32     ZERO;               // 0 - reserved for future use
            //cstring   fullCollectionName; // "dbname.collectionname"
            //int32 flags;                  // bit vector. see below
            //BSON selector;                // the query to select the document
            //BSON document;                // the document data to update with or insert

            byte[] result;

            using (var stream = new MemoryStream())
            {
                using (var writer = new BodyWriter(stream))
                {
                    writer.Write(0);
                    writer.Write(FullCollectionName);
                    writer.WriteTerminator();
                    writer.Write((int)Mode);
                    writer.WriteSelector(QuerySelector ?? new object());
                    writer.WriteDocument(Document);
                }
                result = stream.ToArray();
            }

            return result;
        }
    }
}