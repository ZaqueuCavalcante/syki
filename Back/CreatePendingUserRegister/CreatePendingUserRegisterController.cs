using Swashbuckle.AspNetCore.Filters;

namespace Syki.Back.CreatePendingUserRegister;

/// <summary>
/// Cria um registro de usuário pendente.
/// </summary>
[ApiController]
[EnableRateLimiting("VerySmall")]
[Consumes("application/json"), Produces("application/json")]
public class CreatePendingUserRegisterController(CreatePendingUserRegisterService service) : ControllerBase
{
    [HttpPost("users")]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(CreatePendingUserRegisterErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreatePendingUserRegisterIn data)
    {
        await service.Create(data);

        return Ok();
    }
}
