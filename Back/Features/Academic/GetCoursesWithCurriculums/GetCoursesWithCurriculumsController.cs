namespace Syki.Back.Features.Academic.GetCoursesWithCurriculums;

/// <summary>
/// Retorna todos os Cursos que possuem Grades Curriculares vinculadas.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCoursesWithCurriculumsController(GetCoursesWithCurriculumsService service) : ControllerBase
{
    [HttpGet("academic/courses/with-curriculums")]
    public async Task<IActionResult> Get()
    {
        var courses = await service.Get(User.InstitutionId());

        return Ok(courses);
    }
}
