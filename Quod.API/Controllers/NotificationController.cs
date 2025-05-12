using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Quod.Domain;
using SharpCompress.Common;

namespace Quod.API.Controllers
{
    [ApiController]
    [Route("api/notificacoes")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public NotificationController(
            INotificationService notificationService,
            IMapper mapper
        )
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("fraude")]
        public async Task<IActionResult> NotifyFraud([FromBody] NotificationViewModel request)
        {
            if (request == null)
                return BadRequest("Dados da notificação não fornecidos");
            try
            {
                var entity = _mapper.Map<Notification>(request);
                await _notificationService.AddAsync(entity);
                return Ok("Notificação recebida com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar notificação de fraude: {ex.Message}");
            }
        }
    }
}
