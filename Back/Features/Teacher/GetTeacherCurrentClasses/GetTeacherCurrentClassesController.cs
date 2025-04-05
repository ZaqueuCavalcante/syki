namespace Syki.Back.Features.Teacher.GetTeacherCurrentClasses;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeacherCurrentClassesController(GetTeacherCurrentClassesService service) : ControllerBase
{
    /// <summary>
    /// Turmas atuais
    /// </summary>
    /// <remarks>
    /// Retorna as turmas atualmente tituladas pelo professor.
    /// </remarks>
    [HttpGet("teacher/classes/current")]
    public async Task<IActionResult> Get()
    {
        var classes = await service.Get(User.InstitutionId(), User.Id());

        return Ok(classes);
    }
}
