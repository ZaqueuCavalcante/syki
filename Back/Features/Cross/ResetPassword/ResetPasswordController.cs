namespace Syki.Back.Features.Cross.ResetPassword;

/// <summary>
/// Redefine a senha do Usuário.
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
        var result = await service.Reset(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
