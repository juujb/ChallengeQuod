namespace Quod.Domain
{
    public interface INotificationRestService
    {
        Task PostAsync(NotificationViewModel notification);
    }
}
