using Mongo.CRUD.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mongo.CRUD
{
    /// <summary>
    /// Interface for MongoCRUD 
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public interface IMongoCRUD<TDocument> where TDocument : class, new()
    {
        /// <summary>
        /// MongoDatabase
        /// </summary>
        IMongoDatabase Database { get; set; }

        /// <summary>
        /// MongoCollection<T> from mongo driver
        /// </summary>
        IMongoCollection<TDocument> Collection { get; set; }

        /// <summary>
        /// MongoCRUD configuration with connection string and database name
        /// </summary>
        MongoConfiguration Configuration { get; set; }

        /// <summary>
        /// MongoClient from mongo driver
        /// </summary>
        IMongoClient MongoClient { get; }

        /// <summary>
        /// Create new document
        /// </summary>
        /// <param name="obj"></param>
        void Create(TDocument obj);

        /// <summary>
        /// Create new document async
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        Task CreateAsync(TDocument document, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create many new documents
        /// </summary>
        /// <param name="obj"></param>
        void Create(List<TDocument> objs);

        /// <summary>
        /// Create many new documents
        /// </summary>
        /// <param name="documents"></param>
        /// <param name="cancellationToken"></param>
        Task CreateAsync(List<TDocument> documents, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update one document
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Update(TDocument document);

        /// <summary>
        /// Update one document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TDocument document, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update one or more documents partially by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="partialObject"></param>
        /// <returns></returns>
        bool UpdateByQuery(FilterDefinition<TDocument> filter, object partialObject);

        /// <summary>
        /// Update one or more documents partially by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="partialObject"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UpdateByQueryAsync(FilterDefinition<TDocument> filter, object partialObject, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update if exists or create new document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        bool Upsert(TDocument document);

        /// <summary>
        /// Update if exists or create new document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UpsertAsync(TDocument document, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(object id);

        /// <summary>
        /// Delete document by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete by query
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        bool DeleteByQuery(FilterDefinition<TDocument> filter);

        /// <summary>
        /// Delete by query
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteByQueryAsync(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Search documents by filters, with paging and sorting
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="options"></param>
        /// <param name="projectionOptions"></param>
        /// <returns></returns>
        SearchResult<TDocument> Search(FilterDefinition<TDocument> filters, SearchOptions options = null, ProjectionOptions projectionOptions = null);

        /// <summary>
        /// Search documents by filters, with paging and sorting
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="options"></param>
        /// <param name="projectionOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SearchResult<TDocument>> SearchAsync(FilterDefinition<TDocument> filters, SearchOptions options = null, ProjectionOptions projectionOptions = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TDocument Get(object id);

        /// <summary>
        /// Get document by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TDocument> GetAsync(object id, CancellationToken cancellationToken = default);


        /// <summary>
        /// Search documents by expression, with paging and sorting
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="options"></param>
        /// <param name="projectionOptions"></param>
        /// <returns></returns>
        SearchResult<TDocument> Search(Expression<Func<TDocument, bool>> filters, SearchOptions options = null, ProjectionOptions projectionOptions = null);


        /// <summary>
        /// Search documents by expression, with paging and sorting
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="options"></param>
        /// <param name="projectionOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SearchResult<TDocument>> SearchAsync(Expression<Func<TDocument, bool>> filters, SearchOptions options = null, ProjectionOptions projectionOptions = null, CancellationToken cancellationToken = default);
    }
}