using Exato.Shared.Features.Cross.Login;

namespace Exato.Back.Features.Cross.Login;

[ApiController]
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

        if (result.IsSuccess)
        {
            Response.AppendJWTCookie(result.Success.JWT, settings);
            return Ok(result.Success.ToLoginOut());
        }

        return BadRequest(result.Error);
    }
}

internal class RequestExamples : ExamplesProvider<LoginIn>;
internal class ResponseExamples : ExamplesProvider<LoginOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    LoginRequiresTwoFactor,
    LoginWrongEmailOrPassword
>;
