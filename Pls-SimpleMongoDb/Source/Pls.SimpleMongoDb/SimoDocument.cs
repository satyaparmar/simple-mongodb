using System;
using Newtonsoft.Json;

namespace Pls.SimpleMongoDb
{
    [Serializable]
    public abstract class SimoDocument
        : ISimoDocument
    {
        /// <summary>
        /// Used by MongoDB to uniqually identify a document.
        /// If no string value has been assigned (prefered by me),
        /// the server will generate one at insert. This is why we
        /// specify that null-value shoudl be ignored when
        /// serializing it.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SimoId _id { get; set; }
    }
}
