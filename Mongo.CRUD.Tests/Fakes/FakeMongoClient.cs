using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mongo.CRUD.Tests.Fakes
{
    public static class FakeMongoClient
    {
        public static Mock<MongoDB.Driver.IMongoClient> GetMongoClientMock<T>(bool boolOperationsResult = true, long searchSizeResult = 10)
            where T : class, new()
        {
            Mock<MongoDB.Driver.IMongoCollection<T>> collectionMock =
              new Mock<MongoDB.Driver.IMongoCollection<T>>();

            // Create
            collectionMock
                .Setup(m => m.InsertOne(It.IsAny<T>(), null, default(CancellationToken)))
                .Returns(Task.CompletedTask); ;

            // CreateMany
            collectionMock
                .Setup(m => m.InsertManyAsync(It.IsAny<IEnumerable<T>>(), null, default(CancellationToken)))
                .Returns(Task.CompletedTask);

            // Update
            collectionMock
                .Setup(m => m.ReplaceOne(It.IsAny<MongoDB.Driver.FilterDefinition<T>>(), It.IsAny<T>(), It.IsAny<MongoDB.Driver.UpdateOptions>(), default(CancellationToken)))
                .Returns(new SuccesReplaceOneResult(boolOperationsResult));

            // UpdateMany
            collectionMock
                .Setup(m => m.UpdateMany(It.IsAny<MongoDB.Driver.FilterDefinition<T>>(), It.IsAny<MongoDB.Driver.BsonDocumentUpdateDefinition<T>>(), It.IsAny<MongoDB.Driver.UpdateOptions>(), default(CancellationToken)))
                .Returns(new SuccesUpdateResult(boolOperationsResult));

            // Delete
            collectionMock
                .Setup(m => m.DeleteOne(It.IsAny<MongoDB.Driver.FilterDefinition<T>>(), default(CancellationToken)))
                .Returns(new SuccesDeleteResult(boolOperationsResult));

            // DeleteMany
            collectionMock
                .Setup(m => m.DeleteMany(It.IsAny<MongoDB.Driver.FilterDefinition<T>>(), default(CancellationToken)))
                .Returns(new SuccesDeleteResult(boolOperationsResult));

            // CountDocuments
            collectionMock
                .Setup(m => m.CountDocuments(It.IsAny<MongoDB.Driver.FilterDefinition<T>>(), It.IsAny<MongoDB.Driver.CountOptions>(), default(CancellationToken)))
                .Returns(searchSizeResult);

            // Fake Items
            List<T> items = new List<T>();
            for (int i = 0; i < searchSizeResult; i++)
            {
                items.Add(new T());
            }

            // Cursor
            var cursorMock = new Mock<MongoDB.Driver.IAsyncCursor<T>>();
            cursorMock.Setup(m => m.Current).Returns(items);
            cursorMock
                .SetupSequence(m => m.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            cursorMock
                .SetupSequence(m => m.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            // Cursor into Collection
            collectionMock
                .Setup(m => m.FindAsync<T>(
                        It.IsAny<Expression<Func<T, bool>>>(),
                        It.IsAny<MongoDB.Driver.FindOptions<T, T>>(),
                        It.IsAny<CancellationToken>()
                    ))
                .ReturnsAsync(cursorMock.Object);
            collectionMock
                .Setup(m => m.FindAsync<T>(
                        It.IsAny<MongoDB.Driver.IClientSessionHandle>(),
                        It.IsAny<Expression<Func<T, bool>>>(),
                        It.IsAny<MongoDB.Driver.FindOptions<T, T>>(),
                        It.IsAny<CancellationToken>()
                    ))
                .ReturnsAsync(cursorMock.Object);

            collectionMock
                .Setup(m => m.FindSync<T>(
                        It.IsAny<Expression<Func<T, bool>>>(),
                        It.IsAny<MongoDB.Driver.FindOptions<T, T>>(),
                        It.IsAny<CancellationToken>()
                    ))
                .Returns(cursorMock.Object);
            collectionMock
                .Setup(m => m.FindSync<T>(
                        It.IsAny<MongoDB.Driver.IClientSessionHandle>(),
                        It.IsAny<Expression<Func<T, bool>>>(),
                        It.IsAny<MongoDB.Driver.FindOptions<T, T>>(),
                        It.IsAny<CancellationToken>()
                    ))
                .Returns(cursorMock.Object);

            // Mongo Database
            Mock<MongoDB.Driver.IMongoDatabase> databaseMock = new Mock<MongoDB.Driver.IMongoDatabase>();
            databaseMock
                .Setup(m => m.GetCollection<T>(typeof(T).Name, null))
                .Returns(collectionMock.Object);

            // Mongo Client
            Mock<MongoDB.Driver.IMongoClient> clientMock = new Mock<MongoDB.Driver.IMongoClient>();
            clientMock
                .Setup(m => m.GetDatabase(It.IsAny<string>(), null))
                .Returns(databaseMock.Object);

            return clientMock;
        }
    }

    public class SuccesDeleteResult : MongoDB.Driver.DeleteResult
    {
        public SuccesDeleteResult(bool isAcknowledged)
        {
            this._isAcknowledged = true;
        }

        private bool _isAcknowledged { get; set; }

        public override bool IsAcknowledged => this._isAcknowledged;

        public override long DeletedCount => 0;
    }

    public class SuccesUpdateResult : MongoDB.Driver.UpdateResult
    {
        public SuccesUpdateResult(bool isAcknowledged)
        {
            this._isAcknowledged = true;
        }

        private bool _isAcknowledged { get; set; }

        public override bool IsAcknowledged => this._isAcknowledged;

        public override bool IsModifiedCountAvailable => true;

        public override long MatchedCount => 0;

        public override long ModifiedCount => 0;

        public override BsonValue UpsertedId => null;
    }

    public class SuccesReplaceOneResult : MongoDB.Driver.ReplaceOneResult
    {
        public SuccesReplaceOneResult(bool isAcknowledged)
        {
            this._isAcknowledged = true;
        }

        private bool _isAcknowledged { get; set; }

        public override bool IsAcknowledged => this._isAcknowledged;

        public override bool IsModifiedCountAvailable => true;

        public override long MatchedCount => 0;

        public override long ModifiedCount => 0;

        public override BsonValue UpsertedId => null;
    }
}
