namespace Syki.Back.Features.Adm.GetTask;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetTaskController(GetTaskService service) : ControllerBase
{
    /// <summary>
    /// Tarefa
    /// </summary>
    /// <remarks>
    /// Retorna a tarefa especificada.
    /// </remarks>
    [HttpGet("adm/tasks/{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var task = await service.Get(id);

        return Ok(task);
    }
}
