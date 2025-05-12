using Microsoft.Extensions.Options;
using Quod.Domain;

namespace Quod.Infra.Mongo
{
    public class NotificationRepository : MongoRepository<Notification>, INotificationRepository
    {
        public override string CollectionName => MongoCollections.Notification;

        public NotificationRepository(IOptions<DefaultMongoDbSettings> settings) : base(settings)
        {

        }
    }
}
