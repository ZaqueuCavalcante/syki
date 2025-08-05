namespace Syki.Back.Features.Cross.ResetPassword;

[ApiController]
[EnableRateLimiting("Small")]
public class ResetPasswordController(ResetPasswordService service) : ControllerBase
{
    /// <summary>
    /// Redefinir senha 🔓
    /// </summary>
    /// <remarks>
    /// Redefine a senha do usuário.
    /// </remarks>
    [HttpPost("reset-password")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Reset([FromBody] ResetPasswordIn data)
    {
        var result = await service.Reset(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<ResetPasswordIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    UserNotFound,
    WeakPassword,
    InvalidResetToken>;
