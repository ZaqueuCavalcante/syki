using Exato.Shared.Features.Cross.SetupTwoFactorAuthentication;

namespace Exato.Back.Features.Cross.SetupTwoFactorAuthentication;

[ApiController, Authorize(Policies.SetupTwoFactorAuthentication)]
public class SetupTwoFactorAuthenticationController(SetupTwoFactorAuthenticationService service) : ControllerBase
{
    /// <summary>
    /// Habilitar 2FA
    /// </summary>
    /// <remarks>
    /// Habilita a autenticação de dois fatores.
    /// </remarks>
    [HttpPost("2fa/setup")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Setup([FromBody] SetupTwoFactorAuthenticationIn data)
    {
        var result = await service.Setup(User.Id, data.Token);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<SetupTwoFactorAuthenticationIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<Invalid2faToken>;
