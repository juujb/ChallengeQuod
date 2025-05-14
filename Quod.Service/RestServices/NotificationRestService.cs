using Quod.Domain;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;

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