namespace Syki.Back.Features.Academic.GetCampiEnrollment;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCampiEnrollmentController(GetCampiEnrollmentService service) : ControllerBase
{
    /// <summary>
    /// Alunos por Campus
    /// </summary>
    /// <remarks>
    /// Retorna a quantidade de alunos por campus.
    /// </remarks>
    [HttpGet("academic/campi/enrollment")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        var campi = await service.Get(User.InstitutionId());

        return Ok(campi);
    }
}
