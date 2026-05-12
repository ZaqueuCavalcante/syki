namespace Syki.Back.Features.Cross.FinishUserRegister;

[ApiController]
[EnableRateLimiting("Small")]
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
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Finish([FromBody] FinishUserRegisterIn data)
    {
        var result = await service.Finish(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<FinishUserRegisterIn>;
internal class ResponseExamples : ExamplesProvider<UserOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    WeakPassword,
    UserAlreadyRegistered,
    InvalidRegistrationToken>;
