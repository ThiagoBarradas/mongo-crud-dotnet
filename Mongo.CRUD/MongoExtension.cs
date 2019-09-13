using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Mongo.CRUD
{
    public static class MongoExtension
    {
        public static void AddMongoDb(IServiceCollection services,string conection)
        {
            var mongoUrl = new MongoUrl(conection);
            IMongoClient client = new MongoClient(mongoUrl);
            var database = client.GetDatabase(mongoUrl.DatabaseName);

            services.AddSingleton<IMongoDatabase>(database);
            services.AddSingleton<IMongoClient>(client);
            
            MongoCRUD.RegisterDefaultConventionPack((x)=> true);
        }
    }
}