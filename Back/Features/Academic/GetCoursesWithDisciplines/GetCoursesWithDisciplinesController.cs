namespace Syki.Back.Features.Academic.GetCoursesWithDisciplines;

/// <summary>
/// Retorna todos os Cursos que possuem Disciplinas vinculadas.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCoursesWithDisciplinesController(GetCoursesWithDisciplinesService service) : ControllerBase
{
    [HttpGet("academic/courses/with-disciplines")]
    public async Task<IActionResult> Get()
    {
        var courses = await service.Get(User.InstitutionId());

        return Ok(courses);
    }
}
