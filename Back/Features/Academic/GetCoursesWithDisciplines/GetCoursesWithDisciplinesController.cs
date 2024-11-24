namespace Syki.Back.Features.Academic.GetCoursesWithDisciplines;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCoursesWithDisciplinesController(GetCoursesWithDisciplinesService service) : ControllerBase
{
    /// <summary>
    /// Cursos com disciplinas
    /// </summary>
    /// <remarks>
    /// Retorna todos os cursos que possuem disciplinas vinculadas.
    /// </remarks>
    [HttpGet("academic/courses/with-disciplines")]
    public async Task<IActionResult> Get()
    {
        var courses = await service.Get(User.InstitutionId());

        return Ok(courses);
    }
}
