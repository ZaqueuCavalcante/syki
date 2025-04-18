namespace Syki.Back.Features.Academic.GetCourses;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCoursesController(GetCoursesService service) : ControllerBase
{
    /// <summary>
    /// Cursos
    /// </summary>
    /// <remarks>
    /// Retorna todos os cursos.
    /// </remarks>
    [HttpGet("academic/courses")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        var courses = await service.Get(User.InstitutionId());

        return Ok(courses);
    }
}
