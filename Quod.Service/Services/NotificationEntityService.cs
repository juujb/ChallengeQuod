using Quod.Domain;

namespace Quod.Service
{
    public class NotificationEntityService : MongoEntityService<Notification, INotificationRepository>, INotificationEntityService
    {
        public NotificationEntityService(INotificationRepository repository) : base(repository)
        {

        }

    }
}
