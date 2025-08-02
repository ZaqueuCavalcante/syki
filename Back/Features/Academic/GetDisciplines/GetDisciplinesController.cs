namespace Syki.Back.Features.Academic.GetDisciplines;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetDisciplinesController(GetDisciplinesService service) : ControllerBase
{
    /// <summary>
    /// Disciplinas
    /// </summary>
    /// <remarks>
    /// Retorna todas as disciplinas. <br/>
    /// Caso informe o "CourseId", filtra as disciplinas vinculadas com o curso informado.
    /// </remarks>
    [HttpGet("academic/disciplines")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get([FromQuery] Guid? courseId)
    {
        var disciplines = await service.Get(User.InstitutionId(), courseId);

        return Ok(disciplines);
    }
}
