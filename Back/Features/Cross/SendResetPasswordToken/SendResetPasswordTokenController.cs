namespace Syki.Back.Features.Cross.SendResetPasswordToken;

[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class SendResetPasswordTokenController(SendResetPasswordTokenService service) : ControllerBase
{
    /// <summary>
    /// Esqueci minha senha 🔓
    /// </summary>
    /// <remarks>
    /// Envia para o email informado um link de redefinição de senha.
    /// </remarks>
    [HttpPost("reset-password-token")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Reset([FromBody] SendResetPasswordTokenIn data)
    {
        var result = await service.Send(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

public class RequestsExamples : IMultipleExamplesProvider<SendResetPasswordTokenIn>
{
    public IEnumerable<SwaggerExample<SendResetPasswordTokenIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new SendResetPasswordTokenIn("academico@syki.com")
		);
        yield return SwaggerExample.Create(
			"Professor",
			new SendResetPasswordTokenIn("professor@syki.com")
		);
        yield return SwaggerExample.Create(
			"Aluno",
			new SendResetPasswordTokenIn("aluno@syki.com")
		);
    }
}

public class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new UserNotFound().ToSwaggerExampleErrorOut();
    }
}
