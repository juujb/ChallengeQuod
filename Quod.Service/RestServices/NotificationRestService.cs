using Microsoft.Extensions.Configuration;
using Quod.Domain;
using System.Net.Http.Json;

namespace Quod.Service
{
    public class NotificationRestService : INotificationRestService
    {
        private static readonly HttpClient _httpClient = new ();
        private readonly string _notificationUrl;

        public NotificationRestService(IConfiguration configuration)
        {
            _notificationUrl = configuration["NotificationServiceUrl"] ?? "https://localhost:7274/api/notificacoes/fraude";
        }

        public async Task PostAsync(NotificationViewModel notification)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_notificationUrl, notification);
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }
    }
}