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
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorExamples))]
    public async Task<IActionResult> Reset([FromBody] ResetPasswordIn data)
    {
        var result = await service.Reset(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<ResetPasswordIn>
{
    public IEnumerable<SwaggerExample<ResetPasswordIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new ResetPasswordIn
			{
				Token = Guid.CreateVersion7().ToString(),
				Password = "M1@Str0ngP4ssword#"
			}
		);
    }
}

internal class ErrorExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new UserNotFound().ToSwaggerExampleErrorOut();
        yield return new WeakPassword().ToSwaggerExampleErrorOut();
        yield return new InvalidResetToken().ToSwaggerExampleErrorOut();
    }
}
