using Exato.Shared.Features.Cross.SendResetPasswordToken;

namespace Exato.Back.Features.Cross.SendResetPasswordToken;

[ApiController]
public class SendResetPasswordTokenController(SendResetPasswordTokenService service) : ControllerBase
{
    /// <summary>
    /// Esqueci minha senha 🔓
    /// </summary>
    /// <remarks>
    /// Envia um link de redefinição de senha para o email informado.
    /// </remarks>
    [HttpPost("reset-password-token")]
    [DbContextTransactionFilter]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Send([FromBody] SendResetPasswordTokenIn data)
    {
        var result = await service.Send(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<SendResetPasswordTokenIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<UserNotFound>;
