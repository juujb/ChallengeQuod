using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Quod.Domain;
using Quod.Infra.Mongo;

namespace Quod.API
{
    public static class MongoRegistry
    {
        public static void Load(IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            services.AddSingleton<INotificationRepository, NotificationRepository>();
            services.AddSingleton<IBiometryRepository, BiometryRepository>();
        }


    }
}
