namespace Syki.Back.Features.Adm.GetTasks;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetTasksController(GetTasksService service) : ControllerBase
{
    /// <summary>
    /// Tarefas
    /// </summary>
    /// <remarks>
    /// Retorna todas as tarefas.
    /// </remarks>
    [HttpGet("adm/tasks")]
    public async Task<IActionResult> Get([FromQuery] SykiTaskTableFilterIn filters)
    {
        var tasks = await service.Get(filters);

        return Ok(tasks);
    }
}
