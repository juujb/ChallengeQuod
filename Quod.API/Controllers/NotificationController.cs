using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Quod.Domain;

namespace Quod.API.Controllers
{
    [ApiController]
    [Route("api/notificacoes")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationEntityService _notificationService;
        private readonly IMapper _mapper;

        public NotificationController(
            INotificationEntityService notificationService,
            IMapper mapper
        )
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("fraude")]
        public IActionResult NotifyFraud([FromBody] NotificationViewModel request)
        {
            if (request == null)
                return BadRequest("Dados da notificação não fornecidos");
            try
            {
                var entity = _mapper.Map<Notification>(request);
                Task.Run(async () => await _notificationService.AddAsync(entity));
                return Ok("Notificação recebida com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar notificação de fraude em background: {ex.Message}");
                return StatusCode(500, $"Erro ao processar notificação de fraude: {ex.Message}");
            }
        }
    }
}
