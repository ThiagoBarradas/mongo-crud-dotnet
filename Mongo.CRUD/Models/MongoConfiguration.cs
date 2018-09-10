namespace Mongo.CRUD.Models
{
    /// <summary>
    /// MongoConfiguration for MongoCRUD
    /// </summary>
    public class MongoConfiguration
    {
        /// <summary>
        /// Full connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Database name
        /// </summary>
        public string Database { get; set; }
    }
}
