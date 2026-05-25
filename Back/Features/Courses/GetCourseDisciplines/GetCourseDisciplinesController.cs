namespace Syki.Back.Features.Courses.GetCourseDisciplines;

[ApiController, Authorize(Policies.GetCourseDisciplines)]
public class GetCourseDisciplinesController(GetCourseDisciplinesService service) : ControllerBase
{
    /// <summary>
    /// Disciplinas do curso
    /// </summary>
    /// <remarks>
    /// Retorna todas as disciplinas do curso informado.
    /// </remarks>
    [HttpGet("courses/{id}/disciplines")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var disciplines = await service.Get(id);
        return Ok(disciplines);
    }
}
