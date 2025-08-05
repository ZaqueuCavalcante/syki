namespace Syki.Back.Features.Cross.SetupMfa;

[ApiController, AuthBearer]
[EnableRateLimiting("Small")]
public class SetupMfaController(SetupMfaService service) : ControllerBase
{
    /// <summary>
    /// Habilitar MFA
    /// </summary>
    /// <remarks>
    /// Habilita a autenticação de dois fatores.
    /// </remarks>
    [HttpPost("mfa/setup")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Setup([FromBody] SetupMfaIn data)
    {
        var result = await service.Setup(User.Id(), data.Token);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<SetupMfaIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<InvalidMfaToken>;
