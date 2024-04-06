namespace Syki.Back.Features.Cross.FinishUserRegister;

/// <summary>
/// Finaliza o registro de um usuário.
/// </summary>
[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class FinishUserRegisterController(FinishUserRegisterService service) : ControllerBase
{
    [HttpPut("users")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(FinishUserRegisterErrorsExamples))]
    public async Task<IActionResult> Finish([FromBody] FinishUserRegisterIn data)
    {
        await service.Finish(data);

        return Ok();
    }
}
