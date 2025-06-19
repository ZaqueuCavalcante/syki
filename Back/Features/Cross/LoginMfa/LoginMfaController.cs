namespace Syki.Back.Features.Cross.LoginMfa;

[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class LoginMfaController(LoginMfaService service, AuthSettings settings) : ControllerBase
{
    /// <summary>
    /// Login MFA 🔓
    /// </summary>
    /// <remarks>
    /// Realiza login utilizando o token MFA.
    /// </remarks>
    [HttpPost("login/mfa")]
    [ProducesResponseType(typeof(LoginMfaOut), 200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorExamples))]
    public async Task<IActionResult> LoginMfa([FromBody] LoginMfaIn data)
    {
        var result = await service.LoginMfa(data);

        if (result.IsSuccess())
        {
            Response.Cookies.Append(
                "syki_jwt",
                result.GetSuccess().AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure = settings.CookieSecure,
                    Domain = settings.CookieDomain,
                }
            );
        }

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IExamplesProvider<LoginMfaIn>
{
    public LoginMfaIn GetExamples()
    {
		return new LoginMfaIn
		{
			Token = "853941",
		};
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<LoginMfaOut>
{
    public IEnumerable<SwaggerExample<LoginMfaOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"JWT",
			new LoginMfaOut
			{
				AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJkYzEyZDgzZi0zNzNjLTQzYTItOTdiMC0yMTg3NTFjMThhZTIiLCJzdWIiOiIwMTkzNWViMi1lNjA3LTc5OGMtOTlhMy04ZDdjYTQ3NzE2ZGYiLCJyb2xlIjoiQWNhZGVtaWMiLCJuYW1lIjoiYWNhZGVtaWNvQHN5a2kuY29tIiwiZW1haWwiOiJhY2FkZW1pY29Ac3lraS5jb20iLCJpbnN0aXR1dGlvbiI6ImJjZDI1YTQ3LTE3YjItNGNlYS04ZDZlLWQ1NTA0NzRkNmFlYSIsIm5iZiI6MTczMjQ2MDYxMSwiZXhwIjoxNzMyODIwNjExLCJpYXQiOjE3MzI0NjA2MTEsImlzcyI6InN5a2ktYXBpLWRldmVsb3BtZW50IiwiYXVkIjoic3lraS1hcGktZGV2ZWxvcG1lbnQifQ.MT99F1dOIAUJRNzqoF6YcQJqXfAxpS5mV-8U8JP_abE"
			}
		);
    }
}

public class ErrorExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new LoginRequiresTwoFactor().ToSwaggerExampleErrorOut();
        yield return new LoginWrongEmailOrPassword().ToSwaggerExampleErrorOut();
    }
}
