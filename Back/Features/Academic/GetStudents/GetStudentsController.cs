namespace Syki.Back.Features.Academic.GetStudents;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetStudentsController(GetStudentsService service) : ControllerBase
{
    /// <summary>
    /// Alunos
    /// </summary>
    /// <remarks>
    /// Retorna todos os alunos.
    /// </remarks>
    [HttpGet("academic/students")]
    public async Task<IActionResult> Get()
    {
        var students = await service.Get(User.InstitutionId());
        return Ok(students);
    }
}
