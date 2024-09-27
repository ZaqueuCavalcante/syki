namespace Syki.Back.Features.Academic.ReleaseClassesForEnrollment;

/// <summary>
/// Libera as Turmas informadas para que os Alunos possam se matricular.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class ReleaseClassesForEnrollmentController(ReleaseClassesForEnrollmentService service) : ControllerBase
{
    [HttpPut("academic/classes/release-for-enrollment")]
    public async Task<IActionResult> Release([FromBody] ReleaseClassesForEnrollmentIn data)
    {
        var result = await service.Release(User.InstitutionId(), data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
