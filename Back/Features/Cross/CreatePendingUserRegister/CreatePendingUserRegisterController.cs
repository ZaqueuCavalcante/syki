namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

[ApiController]
[EnableRateLimiting("SuperVerySmall")]
public class CreatePendingUserRegisterController(CreatePendingUserRegisterService service) : ControllerBase
{
    /// <summary>
    /// Registrar 🔓
    /// </summary>
    /// <remarks>
    /// Cria um registro pendente do usuário no sistema.
    /// Um link de confirmação será enviado para o email informado.
    /// </remarks>
    [HttpPost("users")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreatePendingUserRegisterIn data)
    {
        var result = await service.Create(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IMultipleExamplesProvider<CreatePendingUserRegisterIn>
{
    public IEnumerable<SwaggerExample<CreatePendingUserRegisterIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new CreatePendingUserRegisterIn("academico@syki.com")
		);
        yield return SwaggerExample.Create(
			"Professor",
			new CreatePendingUserRegisterIn("professor@syki.com")
		);
        yield return SwaggerExample.Create(
			"Aluno",
			new CreatePendingUserRegisterIn("aluno@syki.com")
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new InvalidEmail().ToSwaggerExampleErrorOut();
        yield return new EmailAlreadyUsed().ToSwaggerExampleErrorOut();
    }
}
