namespace Estud.Back.Features.Students.GetStudentAgenda;

[ApiController, Authorize(Policies.GetStudentAgenda)]
public class GetStudentAgendaController(GetStudentAgendaService service) : ControllerBase
{
    /// <summary>
    /// Agenda
    /// </summary>
    /// <remarks>
    /// Retorna a agenda do aluno.
    /// </remarks>
    [HttpGet("students/agenda")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var agenda = await service.Get();
        return Ok(agenda);
    }
}

internal class ResponseExamples : ExamplesProvider<GetStudentAgendaOut>;
