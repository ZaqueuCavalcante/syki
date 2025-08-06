namespace Syki.Back.Features.Cross.Login;

[ApiController]
[EnableRateLimiting("Small")]
public class LoginController(LoginService service, AuthSettings settings) : ControllerBase
{
    /// <summary>
    /// Login 🔓
    /// </summary>
    /// <remarks>
    /// Realiza login no sistema.
    /// </remarks>
    [HttpPost("login")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Login([FromBody] LoginIn data)
    {
        var result = await service.Login(data);

        if (result.IsSuccess())
        {
            Response.AppendSykiJwtCookie(result.GetSuccess().AccessToken, settings);
            result.GetSuccess().AccessToken = "";
        }

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<LoginIn>;
internal class ResponseExamples : ExamplesProvider<LoginOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    LoginRequiresTwoFactor,
    LoginWrongEmailOrPassword>;
