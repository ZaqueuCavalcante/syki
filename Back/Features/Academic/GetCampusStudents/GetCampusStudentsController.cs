namespace Syki.Back.Features.Academic.GetCampusStudents;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCampusStudentsController(GetCampusStudentsService service) : ControllerBase
{
    /// <summary>
    /// Alunos do campus
    /// </summary>
    /// <remarks>
    /// Retorna todos os alunos do campus especificado.
    /// </remarks>
    [HttpGet("academic/campi/{id}/students")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var students = await service.Get(id);
        return Ok(students);
    }
}
