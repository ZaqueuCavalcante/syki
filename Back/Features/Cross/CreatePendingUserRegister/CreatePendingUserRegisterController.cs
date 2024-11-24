namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

[ApiController]
[EnableRateLimiting("SuperVerySmall")]
[Consumes("application/json"), Produces("application/json")]
public class CreatePendingUserRegisterController(CreatePendingUserRegisterService service) : ControllerBase
{
    /// <summary>
    /// Registrar 🔓
    /// </summary>
    /// <remarks>
    /// Cria um registro do usuário no sistema, usando o email informado.
    /// </remarks>
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
