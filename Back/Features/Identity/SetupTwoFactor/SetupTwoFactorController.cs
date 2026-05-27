namespace Syki.Back.Features.Identity.SetupTwoFactor;

[ApiController, Authorize(Policies.SetupTwoFactor)]
public class SetupTwoFactorController(SetupTwoFactorService service) : ControllerBase
{
    /// <summary>
    /// Habilitar 2FA
    /// </summary>
    /// <remarks>
    /// Habilita a autenticação de dois fatores.
    /// </remarks>
    [HttpPost("identity/2fa-setup")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Setup([FromBody] SetupTwoFactorIn data)
    {
        var result = await service.Setup(data.Token);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<SetupTwoFactorIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<InvalidTwoFactorToken>;
