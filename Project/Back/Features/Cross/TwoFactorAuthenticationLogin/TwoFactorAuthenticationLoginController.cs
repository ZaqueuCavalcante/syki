using Exato.Shared.Features.Cross.TwoFactorAuthenticationLogin;

namespace Exato.Back.Features.Cross.TwoFactorAuthenticationLogin;

[ApiController]
public class TwoFactorAuthenticationLoginController(TwoFactorAuthenticationLoginService service, AuthSettings settings) : ControllerBase
{
    /// <summary>
    /// 2FA Login
    /// </summary>
    /// <remarks>
    /// Realiza login utilizando o token 2FA.
    /// </remarks>
    [HttpPost("2fa/login")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Login([FromBody] TwoFactorAuthenticationLoginIn data)
    {
        var result = await service.Login(data);

        if (result.IsSuccess)
        {
            Response.AppendJWTCookie(result.Success.JWT, settings);
            return Ok(result.Success.ToTwoFactorAuthenticationLoginOut());
        }

        return BadRequest(result.Error);
    }
}

internal class RequestExamples : ExamplesProvider<TwoFactorAuthenticationLoginIn>;
internal class ResponseExamples : ExamplesProvider<TwoFactorAuthenticationLoginOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    LoginRequiresTwoFactor,
    LoginWrongEmailOrPassword
>;
