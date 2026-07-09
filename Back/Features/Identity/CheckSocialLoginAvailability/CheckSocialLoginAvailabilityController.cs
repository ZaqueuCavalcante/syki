namespace Estud.Back.Features.Identity.CheckSocialLoginAvailability;

[ApiController]
public class CheckSocialLoginAvailabilityController(CheckSocialLoginAvailabilityService service) : ControllerBase
{
    /// <summary>
    /// Verificar Social Login 🔓
    /// </summary>
    /// <remarks>
    /// Retorna quais provedores de login social estão disponíveis.
    /// </remarks>
    [HttpGet("identity/social-login/check-availability")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public IActionResult Get()
    {
        return Ok(service.Get());
    }
}

internal class ResponseExamples : ExamplesProvider<CheckSocialLoginAvailabilityOut>;
