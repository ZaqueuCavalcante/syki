namespace Syki.Back.Features.Cross.ResetPassword;

/// <summary>
/// Redefine a senha do usuário.
/// </summary>
[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class ResetPasswordController(ResetPasswordService service) : ControllerBase
{
    [HttpPost("reset-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ResetPasswordErrorsExamples))]
    public async Task<IActionResult> Reset([FromBody] ResetPasswordIn data)
    {
        await service.Reset(data);

        return Ok();
    }
}
