namespace Syki.Back.Features.Academic.GetCourses;

/// <summary>
/// Retorna todos os Cursos.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCoursesController(GetCoursesService service) : ControllerBase
{
    [HttpGet("academic/courses")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        var courses = await service.Get(User.InstitutionId());

        return Ok(courses);
    }
}
