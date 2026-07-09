namespace Estud.Back.Features.Teachers.GetTeacherAgenda;

[ApiController, Authorize(Policies.GetTeacherAgenda)]
public class GetTeacherAgendaController(GetTeacherAgendaService service) : ControllerBase
{
    /// <summary>
    /// Agenda
    /// </summary>
    /// <remarks>
    /// Retorna a agenda do professor.
    /// </remarks>
    [HttpGet("teachers/agenda")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get();
        return Ok(data);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTeacherAgendaOut>;
