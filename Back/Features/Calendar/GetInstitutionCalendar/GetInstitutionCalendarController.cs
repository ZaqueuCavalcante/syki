namespace Estud.Back.Features.Calendar.GetInstitutionCalendar;

[ApiController, Authorize(Policies.GetInstitutionCalendar)]
public class GetInstitutionCalendarController(GetInstitutionCalendarService service) : ControllerBase
{
    /// <summary>
    /// Calendário acadêmico da instituição
    /// </summary>
    /// <remarks>
    /// Retorna todos os dias do ano informado, com o tipo de cada dia: dia letivo, férias, recesso ou feriado.
    /// Os feriados nacionais já vêm marcados por padrão e podem ser sobrescritos pela instituição.
    /// </remarks>
    [HttpGet("calendar/institution")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetInstitutionCalendarIn data)
    {
        var calendar = await service.Get(data);
        return Ok(calendar);
    }
}

internal class RequestExamples : ExamplesProvider<GetInstitutionCalendarIn>;
internal class ResponseExamples : ExamplesProvider<GetInstitutionCalendarOut>;
