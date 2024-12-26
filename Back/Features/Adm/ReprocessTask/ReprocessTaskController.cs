namespace Syki.Back.Features.Adm.ReprocessTask;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class ReprocessTaskController(ReprocessTaskService service) : ControllerBase
{
    /// <summary>
    /// Tarefa
    /// </summary>
    /// <remarks>
    /// Reprocessa a tarefa especificada.
    /// </remarks>
    [HttpPost("adm/tasks/{id:guid}/reprocess")]
    public async Task<IActionResult> Reprocess([FromRoute] Guid id)
    {
        var result = await service.Reprocess(id);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
