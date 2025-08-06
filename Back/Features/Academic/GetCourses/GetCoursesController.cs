namespace Syki.Back.Features.Academic.GetCourses;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCoursesController(GetCoursesService service) : ControllerBase
{
    /// <summary>
    /// Cursos
    /// </summary>
    /// <remarks>
    /// Retorna todos os cursos.
    /// </remarks>
    [HttpGet("academic/courses")]
    public async Task<IActionResult> Get()
    {
        var courses = await service.Get(User.InstitutionId);
        return Ok(courses);
    }
}
