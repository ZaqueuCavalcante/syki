namespace Syki.Back.Features.Adm.GetSykiTasksSummary;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetSykiTasksSummaryController(GetSykiTasksSummaryService service) : ControllerBase
{
    /// <summary>
    /// Tarefas
    /// </summary>
    /// <remarks>
    /// Retorna os dados consolidados sobre as tarefas.
    /// </remarks>
    [HttpGet("adm/tasks-summary")]
    public async Task<IActionResult> Get()
    {
        var tasks = await service.Get();

        return Ok(tasks);
    }
}
