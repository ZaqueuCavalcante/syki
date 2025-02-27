namespace Syki.Back.Features.Cross.SetupMfa;

[ApiController, AuthBearer]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class SetupMfaController(SetupMfaService service) : ControllerBase
{
    /// <summary>
    /// Habilitar MFA
    /// </summary>
    /// <remarks>
    /// Habilita a autenticação de dois fatores.
    /// </remarks>
    [HttpPost("mfa/setup")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorExamples))]
    public async Task<IActionResult> Setup([FromBody] SetupMfaIn data)
    {
        var result = await service.Setup(User.Id(), data.Token);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<SetupMfaIn>
{
    public IEnumerable<SwaggerExample<SetupMfaIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"TOTP Token",
			new SetupMfaIn
			{
				Token = "843972",
			}
		);
    }
}

internal class ErrorExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new InvalidMfaToken().ToSwaggerExampleErrorOut();
    }
}
