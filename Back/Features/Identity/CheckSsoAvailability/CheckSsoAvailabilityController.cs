namespace Estud.Back.Features.Identity.CheckSsoAvailability;

[ApiController]
public class CheckSsoAvailabilityController(CheckSsoAvailabilityService service) : ControllerBase
{
    /// <summary>
    /// Verificar SSO 🔓
    /// </summary>
    /// <remarks>
    /// Verifica se existe configuração de SSO para o domínio do email informado.
    /// Este endpoint é público e deve ser chamado antes do login para determinar
    /// se o usuário deve fazer login via SSO ou via email/senha.
    /// </remarks>
    [HttpPost("identity/sso/check-availability")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Check([FromBody] CheckSsoAvailabilityIn data)
    {
        var result = await service.Check(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CheckSsoAvailabilityIn>;
internal class ResponseExamples : ExamplesProvider<CheckSsoAvailabilityOut>;
internal class ErrorsExamples : ErrorExamplesProvider<InvalidEmail>;
