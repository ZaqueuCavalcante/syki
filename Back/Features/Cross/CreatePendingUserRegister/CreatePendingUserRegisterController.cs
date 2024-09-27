namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

/// <summary>
/// Cria um Registro de Usuário Pendente.
/// </summary>
[ApiController]
[EnableRateLimiting("SuperVerySmall")]
[Consumes("application/json"), Produces("application/json")]
public class CreatePendingUserRegisterController(CreatePendingUserRegisterService service) : ControllerBase
{
    [HttpPost("users")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(CreatePendingUserRegisterErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreatePendingUserRegisterIn data)
    {
        var result = await service.Create(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
