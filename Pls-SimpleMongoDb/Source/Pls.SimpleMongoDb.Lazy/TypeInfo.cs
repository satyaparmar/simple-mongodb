using System;
using System.Reflection;

namespace Pls.SimpleMongoDb
{

    /// <summary>
    /// Simple class customization holder.
    /// </summary>
    public class TypeInfo
    {
        /// <summary>
        /// Type of class.
        /// </summary>
        public Type ClassType { get; set; }

        /// <summary>
        /// Identifier property.
        /// </summary>
        public PropertyInfo Id { get; set; }

        /// <summary>
        /// Collection name.
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Returns the Database/collection in the format db.collection
        /// </summary>
        public string FullCollectionName {
            get { return string.Format("{0}.{1}", Database, CollectionName); }
        }
    }
}