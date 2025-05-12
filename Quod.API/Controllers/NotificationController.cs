using Microsoft.AspNetCore.Mvc;
using Quod.Domain;

namespace Quod.API.Controllers
{
    [ApiController]
    [Route("api/notificacoes")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        [HttpPost("fraude")]
        public async Task<IActionResult> NotifyFraud([FromBody] NotificationViewModel request)
        {
            if (request == null)
                return BadRequest("Dados da notificação não fornecidos");
            try
            {
                await _notificationService.AddAsync(request);
                return Ok("Notificação recebida com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar notificação de fraude: {ex.Message}");
            }
        }
    }
}
