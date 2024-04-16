using Mongo.CRUD.Attributes;
using Mongo.CRUD.Builders;
using Mongo.CRUD.Conventions;
using Mongo.CRUD.Helpers;
using Mongo.CRUD.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mongo.CRUD
{
    /// <summary>
    /// MongoCRUD class
    /// https://github.com/thiagobarradas/mongo-crud-dotnet
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public class MongoCRUD<TDocument> : IMongoCRUD<TDocument>
        where TDocument : class, new()
    {
        /// <summary>
        /// MongoClient from mongo driver
        /// </summary>
        public IMongoClient MongoClient { get; private set; }

        /// <summary>
        /// MongoDatabase from mongo driver
        /// </summary>
        public IMongoDatabase Database { get; set; }

        /// <summary>
        /// MongoCollection<T> from mongo driver
        /// </summary>
        public IMongoCollection<TDocument> Collection { get; set; }

        /// <summary>
        /// MongoCRUD configuration private object
        /// </summary>
        private MongoConfiguration _configuration { get; set; }

        /// <summary>
        /// MongoCRUD configuration with connection string and database name
        /// </summary>
        public MongoConfiguration Configuration
        {
            get { return _configuration; }
            set { this.SetupMongoConfiguration(value); }
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public MongoCRUD()
        {
        }

        /// <summary>
        /// Construct MongoCRUD with mongo client and database
        /// </summary>
        /// <param name="mongoClient"></param>
        /// <param name="database"></param>
        public MongoCRUD(IMongoClient mongoClient, IMongoDatabase database)
        {
            this.MongoClient = mongoClient;
            this.Database = database;
            this.Collection = database.GetCollection<TDocument>(typeof(TDocument).Name);
        }

        /// <summary>
        /// Contruct MongoCRUD with mongo client and database name
        /// </summary>
        public MongoCRUD(IMongoClient client, string database)
        {
            this.SetupMongoClient(client, database);
        }

        /// <summary>
        /// Construct MongoCRUD with connection string and databasse name
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="database"></param>
        public MongoCRUD(string connectionString, string database)
        {
            this.Configuration = new MongoConfiguration
            {
                ConnectionString = connectionString,
                Database = database
            };
        }

        /// <summary>
        /// Construct MongoCRUD with MongoConfiguration
        /// </summary>
        /// <param name="configuration"></param>
        public MongoCRUD(MongoConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <inheritdoc />
        public void Create(TDocument document)
            => this.CreateAsync(document).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task CreateAsync(TDocument document, CancellationToken cancellationToken = default)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            await this.Collection.InsertOneAsync(document, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public void Create(List<TDocument> documents)
            => this.CreateAsync(documents).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task CreateAsync(List<TDocument> documents, CancellationToken cancellationToken = default)
        {
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            await this.Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public bool Update(TDocument document)
            => this.UpdateAsync(document).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(TDocument document, CancellationToken cancellationToken = default)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var id = this.GetDocumentId(document);

            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            var options = new ReplaceOptions() { IsUpsert = false };

            var actionResult = await this.Collection.ReplaceOneAsync(filter, document, options, cancellationToken: cancellationToken);
            return actionResult.IsAcknowledged;
        }

        /// <inheritdoc />
        public bool UpdateByQuery(FilterDefinition<TDocument> filters, object partialDocument)
            => this.UpdateByQueryAsync(filters, partialDocument).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task<bool> UpdateByQueryAsync(FilterDefinition<TDocument> filters, object partialDocument, CancellationToken cancellationToken = default)
        {
            filters = filters ?? FilterBuilder.GetFilterBuilder<TDocument>().Empty;

            if (partialDocument == null)
            {
                throw new ArgumentNullException(nameof(partialDocument));
            }

            var updateMapped = new BsonDocument { { "$set", partialDocument.ToBsonDocument() } };

            var update = new BsonDocumentUpdateDefinition<TDocument>(updateMapped);
            return (await this.Collection.UpdateManyAsync(filters, update, cancellationToken: cancellationToken)).IsAcknowledged;
        }

        /// <inheritdoc />
        public bool Upsert(TDocument document)
            => this.UpsertAsync(document).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task<bool> UpsertAsync(TDocument document, CancellationToken cancellationToken = default)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var id = this.GetDocumentId(document);

            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            var options = new ReplaceOptions { IsUpsert = true };

            return (await this.Collection.ReplaceOneAsync(filter, document, options, cancellationToken: cancellationToken)).IsAcknowledged;
        }

        /// <inheritdoc />
        public bool Delete(object id)
            => this.DeleteAsync(id).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            return (await this.Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken)).IsAcknowledged;
        }

        /// <inheritdoc />
        public bool DeleteByQuery(FilterDefinition<TDocument> filters)
            => this.DeleteByQueryAsync(filters).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task<bool> DeleteByQueryAsync(FilterDefinition<TDocument> filters, CancellationToken cancellationToken = default)
        {
            filters = filters ?? FilterBuilder.GetFilterBuilder<TDocument>().Empty;

            return (await this.Collection.DeleteManyAsync(filters, cancellationToken: cancellationToken)).IsAcknowledged;
        }

        /// <inheritdoc />
        public TDocument Get(object id)
            => this.GetAsync(id).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task<TDocument> GetAsync(object id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var filter = Builders<TDocument>.Filter.Eq("_id", id);
            return (await this.Collection.FindAsync(filter, cancellationToken: cancellationToken)).FirstOrDefault();
        }

        /// <inheritdoc />
        public SearchResult<TDocument> Search(Expression<Func<TDocument, bool>> filters, SearchOptions options = null, ProjectionOptions projectionOptions = null)
            => this.SearchAsync(filters, options, projectionOptions).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task<SearchResult<TDocument>> SearchAsync(Expression<Func<TDocument, bool>> filters, SearchOptions options = null, ProjectionOptions projectionOptions = null, CancellationToken cancellationToken = default)
        {
            if (options == null)
            {
                options = new SearchOptions();
            }

            var findOptions = FilterBuilder.GetFindOptions<TDocument>().WithSorting(options);

            if (options.EnablePagination)
            {
                findOptions.WithPaging(options);
            }

            if (projectionOptions != null)
            {
                findOptions.Projection = projectionOptions.BuildProjectionByFields<TDocument>();
            }

            var cursor = await this.Collection.FindAsync(filters, findOptions, cancellationToken: cancellationToken);
            var documents = await cursor.ToListAsync(cancellationToken);

            var count = (documents.Count < options.PageSize && options.PageNumber <= 1) || !options.EnablePagination ?
                documents.Count :
                await this.Collection.CountDocumentsAsync(filters, cancellationToken: cancellationToken);

            return new SearchResult<TDocument>
            {
                Count = count,
                Documents = documents
            };
        }

        /// <inheritdoc />
        public SearchResult<TDocument> Search(FilterDefinition<TDocument> filters, SearchOptions options = null, ProjectionOptions projectionOptions = null)
            => this.SearchAsync(filters, options, projectionOptions).GetAwaiter().GetResult();

        /// <inheritdoc />
        public async Task<SearchResult<TDocument>> SearchAsync(FilterDefinition<TDocument> filters, SearchOptions options = null, ProjectionOptions projectionOptions = null, CancellationToken cancellationToken = default)
        {
            if (options == null)
            {
                options = new SearchOptions();
            }

            filters = filters ?? FilterBuilder.GetFilterBuilder<TDocument>().Empty;

            var findOptions = FilterBuilder.GetFindOptions<TDocument>().WithSorting(options);

            if (options.EnablePagination)
            {
                findOptions.WithPaging(options);
            }

            if (projectionOptions != null)
            {
                findOptions.Projection = projectionOptions.BuildProjectionByFields<TDocument>();
            }

            var cursor = await this.Collection.FindAsync(filters, findOptions, cancellationToken: cancellationToken);
            var documents = await cursor.ToListAsync(cancellationToken);
            
            var count = (documents.Count < options.PageSize && options.PageNumber <= 1) || !options.EnablePagination ?
                documents.Count :
                await this.Collection.CountDocumentsAsync(filters);

            return new SearchResult<TDocument>
            {
                Count = count,
                Documents = documents
            };
        }

        /// <summary>
        /// Get document id by property or annotation
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private object GetDocumentId(TDocument obj)
        {
            var resultByAttribute = obj.GetIndividualPropertyDetailsByAttribute<TDocument, BsonIdAttribute>();
            if (resultByAttribute != null)
            {
                return resultByAttribute.Value;
            }

            var resultByPropertyName = obj.GetIndividualPropertyDetailsByPropertyName<TDocument>("id");
            if (resultByPropertyName != null)
            {
                return resultByPropertyName.Value;
            }

            throw new InvalidOperationException(
                "Model must have a property called 'Id' or a property with BsonId attribute");
        }


        /// <summary>
        /// Setup instance configuration
        /// </summary>
        /// <param name="configuration"></param>
        private void SetupMongoConfiguration(MongoConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var mongoClient = new MongoClient(configuration.ConnectionString);
            this.SetupMongoClient(mongoClient, configuration.Database);
        }

        /// <summary>
        /// Setup instance configuration
        /// </summary>
        /// <param name="configuration"></param>
        private void SetupMongoClient(IMongoClient mongoClient, string database)
        {
            if (string.IsNullOrWhiteSpace(database))
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.MongoClient = mongoClient ?? throw new ArgumentNullException(nameof(mongoClient));
            this.Database = this.MongoClient.GetDatabase(database);

            var currentType = typeof(TDocument);
            var collectionNameAttribute = (CollectionNameAttribute)currentType
                .GetCustomAttributes(typeof(CollectionNameAttribute), false).FirstOrDefault();

            var collectionName = collectionNameAttribute?.CollectionName ?? currentType.Name;

            this.Collection = this.Database.GetCollection<TDocument>(collectionName);
        }
    }

    /// <summary>
    /// Static MongoCRUD without generic dependencies
    /// </summary>
    public static class MongoCRUD
    {
        /// <summary>
        /// Default convention pack name
        /// </summary>
        public static readonly string DEFAULT_CONVENTION_PACK_NAME = "Default MongoCRUD Convention";

        /// <summary>
        /// Default convention pack
        /// - Guid as string
        /// - Enum as string
        /// - CamelCase element name
        /// - Ignore extra elements
        /// Please, call this convention pack registration only one time in your application.
        /// </summary>
        public static void RegisterDefaultConventionPack(Func<Type, bool> filter)
        {
            ConventionRegistry.Register(DEFAULT_CONVENTION_PACK_NAME, GetDefaultConventionPack(), filter);
        }

        /// <summary>
        /// Remove default convention pack
        /// </summary>
        public static void UnregisterDefaultConventionPack()
        {
            ConventionRegistry.Remove(DEFAULT_CONVENTION_PACK_NAME);
        }

        /// <summary>
        /// Get Default convention pack
        /// - Guid as string
        /// - Enum as string
        /// - CamelCase element name
        /// - Ignore extra elements
        /// </summary>
        public static ConventionPack GetDefaultConventionPack()
        {
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new GuidAsStringRepresentationConvention()
            };

            return conventionPack;
        }
    }
}