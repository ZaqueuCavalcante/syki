namespace Syki.Back.Features.Users.RegisterUser;

[ApiController, EnableRateLimiting(RateLimitingConfigs.SensitivePolicy)]
public class RegisterUserController(RegisterUserService service) : ControllerBase
{
    /// <summary>
    /// Registrar usuário 🔓
    /// </summary>
    /// <remarks>
    /// Registra um usuário no sistema.
    /// </remarks>
    [HttpPost("users/register")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Register([FromBody] RegisterUserIn data)
    {
        var result = await service.Register(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<RegisterUserIn>;
internal class ResponseExamples : ExamplesProvider<RegisterUserOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    EmailAlreadyUsed
>;
