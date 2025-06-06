namespace Syki.Back.Features.Cross.FinishUserRegister;

[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class FinishUserRegisterController(FinishUserRegisterService service) : ControllerBase
{
    /// <summary>
    /// Finalizar registro 🔓
    /// </summary>
    /// <remarks>
    /// Finaliza o registro do usuário no sistema.
    /// </remarks>
    [HttpPut("users")]
    [DbContextTransactionFilter]
    [ProducesResponseType(typeof(UserOut), 200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Finish([FromBody] FinishUserRegisterIn data)
    {
        var result = await service.Finish(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IExamplesProvider<FinishUserRegisterIn>
{
	public FinishUserRegisterIn GetExamples()
	{
		return new FinishUserRegisterIn(
			Guid.CreateVersion7().ToString(),
			"M1@Str0ngP4ssword#"
		);
	}
}

internal class ResponseExamples : IMultipleExamplesProvider<UserOut>
{
    public IEnumerable<SwaggerExample<UserOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"User",
			new UserOut
			{
				Id = Guid.CreateVersion7(),
				Name = "Zaqueu Cavalcante",
				Email = "zaqueu.cavalcante@gmail.com",
				Password = "M1@Str0ngP4ssword#",
				InstitutionId = Guid.CreateVersion7(),
				Institution = "Universidade Federal Caruaruense",
				Role = UserRole.Student.ToString()
			}
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new WeakPassword().ToSwaggerExampleErrorOut();
        yield return new UserAlreadyRegistered().ToSwaggerExampleErrorOut();
        yield return new InvalidRegistrationToken().ToSwaggerExampleErrorOut();
    }
}
