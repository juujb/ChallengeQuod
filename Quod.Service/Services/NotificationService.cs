using Quod.Domain;

namespace Quod.Service
{
    public class NotificationService : MongoEntityService<Notification, INotificationRepository>, INotificationService
    {
        public NotificationService(INotificationRepository repository) : base(repository)
        {

        }

    }
}
