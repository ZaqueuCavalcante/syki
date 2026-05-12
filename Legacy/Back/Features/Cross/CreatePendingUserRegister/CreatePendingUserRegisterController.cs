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
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreatePendingUserRegisterIn data)
    {
        var result = await service.Create(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreatePendingUserRegisterIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidEmail,
    EmailAlreadyUsed>;
