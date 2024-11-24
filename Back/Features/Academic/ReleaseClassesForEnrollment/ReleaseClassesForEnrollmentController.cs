namespace Syki.Back.Features.Academic.ReleaseClassesForEnrollment;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class ReleaseClassesForEnrollmentController(ReleaseClassesForEnrollmentService service) : ControllerBase
{
    /// <summary>
    /// Liberar turmas
    /// </summary>
    /// <remarks>
    /// Libera as turmas informadas para que os alunos possam se matricular.
    /// </remarks>
    [HttpPut("academic/classes/release-for-enrollment")]
    public async Task<IActionResult> Release([FromBody] ReleaseClassesForEnrollmentIn data)
    {
        var result = await service.Release(User.InstitutionId(), data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
