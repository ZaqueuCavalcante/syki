namespace Syki.Back.Features.Student.GetStudentCurrentClasses;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentCurrentClassesController(GetStudentCurrentClassesService service) : ControllerBase
{
    /// <summary>
    /// Turmas atuais
    /// </summary>
    /// <remarks>
    /// Retorna as turmas atualmente cursadas pelo aluno.
    /// </remarks>
    [HttpGet("student/classes/current")]
    public async Task<IActionResult> Get()
    {
        var classes = await service.Get(User.InstitutionId, User.Id);

        return Ok(classes);
    }
}
