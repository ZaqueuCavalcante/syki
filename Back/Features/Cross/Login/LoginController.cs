namespace Syki.Back.Features.Cross.Login;

[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class LoginController(LoginService service) : ControllerBase
{
    /// <summary>
    /// Login 🔓
    /// </summary>
    /// <remarks>
    /// Realiza login no sistema.
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginOut), 200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorExamples))]
    public async Task<IActionResult> Login([FromBody] LoginIn data)
    {
        var result = await service.Login(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<LoginIn>
{
    public IEnumerable<SwaggerExample<LoginIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new LoginIn("academico@syki.com", "M1@Str0ngP4ssword#")
		);
        yield return SwaggerExample.Create(
			"Professor",
			new LoginIn("professor@syki.com", "M1@Str0ngP4ssword#")
		);
        yield return SwaggerExample.Create(
			"Aluno",
			new LoginIn("aluno@syki.com", "M1@Str0ngP4ssword#")
		);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<LoginOut>
{
    public IEnumerable<SwaggerExample<LoginOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"JWT",
			new LoginOut
			{
				AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJkYzEyZDgzZi0zNzNjLTQzYTItOTdiMC0yMTg3NTFjMThhZTIiLCJzdWIiOiIwMTkzNWViMi1lNjA3LTc5OGMtOTlhMy04ZDdjYTQ3NzE2ZGYiLCJyb2xlIjoiQWNhZGVtaWMiLCJuYW1lIjoiYWNhZGVtaWNvQHN5a2kuY29tIiwiZW1haWwiOiJhY2FkZW1pY29Ac3lraS5jb20iLCJpbnN0aXR1dGlvbiI6ImJjZDI1YTQ3LTE3YjItNGNlYS04ZDZlLWQ1NTA0NzRkNmFlYSIsIm5iZiI6MTczMjQ2MDYxMSwiZXhwIjoxNzMyODIwNjExLCJpYXQiOjE3MzI0NjA2MTEsImlzcyI6InN5a2ktYXBpLWRldmVsb3BtZW50IiwiYXVkIjoic3lraS1hcGktZGV2ZWxvcG1lbnQifQ.MT99F1dOIAUJRNzqoF6YcQJqXfAxpS5mV-8U8JP_abE"
			}
		);
    }
}

internal class ErrorExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new LoginRequiresTwoFactor().ToSwaggerExampleErrorOut();
        yield return new LoginWrongEmailOrPassword().ToSwaggerExampleErrorOut();
    }
}
