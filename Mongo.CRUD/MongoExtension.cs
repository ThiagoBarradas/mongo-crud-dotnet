using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Mongo.CRUD
{
    public static class MongoExtension
    {
        public static void AddMongoDb(this IServiceCollection services, string connection)
        {
            var mongoUrl = new MongoUrl(connection);
            IMongoClient client = new MongoClient(mongoUrl);
            var database = client.GetDatabase(mongoUrl.DatabaseName);

            services.AddSingleton<IMongoDatabase>(database);
            services.AddSingleton<IMongoClient>(client);

            MongoCRUD.RegisterDefaultConventionPack((x) => true);
        }
    }
}