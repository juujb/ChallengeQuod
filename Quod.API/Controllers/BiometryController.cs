using Microsoft.AspNetCore.Mvc;
using Quod.Domain;

namespace Quod.API.Controllers
{
    [ApiController]
    [Route("api/biometry")]
    public class BiometryController : ControllerBase
    {
        private readonly IBiometryService _biometryService;

        public BiometryController(IBiometryService biometryService)
        {
            _biometryService = biometryService ?? throw new ArgumentNullException(nameof(biometryService));
        }

        [HttpPost("facial")]
        public async Task<IActionResult> ValidateFacialBiometry([FromForm] BiometryRequestViewModel request)
        {
            if (request?.Image == null)
                return BadRequest("Imagem facial não fornecida");

            try
            {
                var result = await _biometryService.CheckFacialBiometryAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar imagem facial: {ex.Message}");
            }
        }

        [HttpPost("fingerprint")]
        public async Task<IActionResult> ValidateFingerPrintBiometry([FromForm] BiometryRequestViewModel request)
        {
            if (request?.Image == null)
                return BadRequest("Imagem da impressão digital não fornecida");

            try
            {
                var result = await _biometryService.CheckFingerPrintBiometryAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar impressão digital: {ex.Message}");
            }
        }
    }
}