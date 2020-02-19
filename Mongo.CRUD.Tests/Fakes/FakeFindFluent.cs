using Mongo.CRUD.Enums;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mongo.CRUD.Tests.Fakes
{
    public class FakeFindFluent<TDocument> : IFindFluent<TDocument, TDocument>
    {
        public int? LimitValue { get; set; }

        public int? SkipValue { get; set; }

        public Dictionary<string, SortMode> SortValues { get; set; }

        private readonly IEnumerable<TDocument> _items;

        public FakeFindFluent()
        {
            _items = Enumerable.Empty<TDocument>();
        }

        public FakeFindFluent(IEnumerable<TDocument> items)
        {
            _items = items ?? Enumerable.Empty<TDocument>();
        }

        public IFindFluent<TDocument, TDocument> Limit(int? limit)
        {
            this.LimitValue = limit;
            return this;
        }

        public IFindFluent<TDocument, TDocument> Skip(int? skip)
        {
            this.SkipValue = skip;
            return this;
        }

        public IFindFluent<TDocument, TDocument> Sort(SortDefinition<TDocument> sort)
        {
            if (this.SortValues == null)
            {
                this.SortValues = new Dictionary<string, SortMode>();
            }

            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<TDocument>();
            var sortRendered = sort.Render(documentSerializer, serializerRegistry);

            foreach (var item in sortRendered)
            {
                this.SortValues.Add(item.Name, (item.Value > 0) ? SortMode.Asc : SortMode.Desc);
            }

            return this;
        }

        public long Count()
        {
            return _items.LongCount();
        }

        public long Count(CancellationToken cancellationToken)
        {
            return _items.LongCount();
        }

        public IFindFluent<TDocument, TResult> As<TResult>()
        {
            return (IFindFluent<TDocument, TResult>)this;
        }

        public IFindFluent<TDocument, TResult> As<TResult>(MongoDB.Bson.Serialization.IBsonSerializer<TResult> resultSerializer)
        {
            return (IFindFluent<TDocument, TResult>)this;
        }

        public FilterDefinition<TDocument> Filter
        {
            get => Builders<TDocument>.Filter.Empty;
            set { }
        }

        public FindOptions<TDocument, TDocument> Options => new FindOptions<TDocument, TDocument>();

        public Task<long> CountAsync()
        {
            return new Task<long>(() =>
            {
                return this.Count();
            });
        }

        public Task<long> CountAsync(CancellationToken cancellationToken)
        {
            return new Task<long>(() =>
            {
                return this.Count();
            });
        }

        public long CountDocuments()
        {
            return this.Count();
        }

        public long CountDocuments(CancellationToken cancellationToken)
        {
            return this.Count();
        }

        public Task<long> CountDocumentsAsync()
        {
            return new Task<long>(() =>
            {
                return this.Count();
            });
        }

        public Task<long> CountDocumentsAsync(CancellationToken cancellationToken)
        {
            return new Task<long>(() =>
            {
                return this.Count();
            });
        }

        public IFindFluent<TDocument, TNewProjection> Project<TNewProjection>(ProjectionDefinition<TDocument, TNewProjection> projection)
        {
            throw new NotImplementedException();
        }

        public IAsyncCursor<TDocument> ToCursor()
        {
            throw new NotImplementedException();
        }

        public IAsyncCursor<TDocument> ToCursor(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncCursor<TDocument>> ToCursorAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncCursor<TDocument>> ToCursorAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
