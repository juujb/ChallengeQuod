using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Quod.Infra.Mongo;

namespace Quod.API
{
    public static class MongoRegistry
    {
        public static void Load(IServiceCollection services)
        {
            var objectSerializer = new ObjectSerializer(type => ObjectSerializer.DefaultAllowedTypes(type));
            BsonSerializer.RegisterSerializer(objectSerializer);
        }

    }
}
