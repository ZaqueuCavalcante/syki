namespace Syki.Back.Features.Cross.FinishUserRegister;

[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class FinishUserRegisterController(FinishUserRegisterService service) : ControllerBase
{
    /// <summary>
    /// Finalizar registro
    /// </summary>
    [HttpPut("users")]
    [ProducesResponseType(typeof(UserOut), 200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(200, typeof(FinishUserRegisterResponseExamples))]
    [SwaggerResponseExample(400, typeof(FinishUserRegisterErrorsExamples))]
    public async Task<IActionResult> Finish([FromBody] FinishUserRegisterIn data)
    {
        var result = await service.Finish(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
