using System.Collections.Generic;

namespace Mongo.CRUD.Models
{
    /// <summary>
    /// Result from search 
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public class SearchResult<TDocument>
    {
        /// <summary>
        /// Total count
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// Documents
        /// </summary>
        public IEnumerable<TDocument> Documents { get; set; }
    }
}
