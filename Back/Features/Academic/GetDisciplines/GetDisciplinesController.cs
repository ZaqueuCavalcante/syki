namespace Syki.Back.Features.Academic.GetDisciplines;

/// <summary>
/// Retorna todas as Disciplinas.
/// Caso informe o "CourseId", filtra as Disciplinas vinculadas com o Curso informado.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetDisciplinesController(GetDisciplinesService service) : ControllerBase
{
    [HttpGet("academic/disciplines")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get([FromQuery] Guid? courseId)
    {
        var disciplines = await service.Get(User.InstitutionId(), courseId);

        return Ok(disciplines);
    }
}
