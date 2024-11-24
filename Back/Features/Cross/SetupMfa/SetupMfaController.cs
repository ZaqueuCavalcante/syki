namespace Syki.Back.Features.Cross.SetupMfa;

[ApiController, AuthBearer]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class SetupMfaController(SetupMfaService service) : ControllerBase
{
    /// <summary>
    /// Habilitar MFA
    /// </summary>
    /// <remarks>
    /// Habilita a autenticação de dois fatores.
    /// </remarks>
    [HttpPost("mfa/setup")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(SetupMfaErrorsExamples))]
    public async Task<IActionResult> Setup([FromBody] SetupMfaIn data)
    {
        var result = await service.Setup(User.Id(), data.Token);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
