namespace Syki.Back.Features.Teacher.GetTeacherClasses;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class GetTeacherClassesController(GetTeacherClassesService service) : ControllerBase
{
    /// <summary>
    /// Turmas
    /// </summary>
    /// <remarks>
    /// Retorna as turmas tituladas pelo professor.
    /// </remarks>
    [HttpGet("teacher/classes")]
    public async Task<IActionResult> Get()
    {
        var classes = await service.Get(User.InstitutionId(), User.Id());

        return Ok(classes);
    }
}
