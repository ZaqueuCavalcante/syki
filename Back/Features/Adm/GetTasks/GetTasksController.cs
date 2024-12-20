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
    public async Task<IActionResult> Get()
    {
        var events = await service.Get();

        return Ok(events);
    }
}
