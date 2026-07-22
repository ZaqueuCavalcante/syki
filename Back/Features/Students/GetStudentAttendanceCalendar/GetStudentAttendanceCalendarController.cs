namespace Estud.Back.Features.Students.GetStudentAttendanceCalendar;

[ApiController, Authorize(Policies.GetStudentAttendanceCalendar)]
public class GetStudentAttendanceCalendarController(GetStudentAttendanceCalendarService service) : ControllerBase
{
    /// <summary>
    /// Calendário de frequência do aluno
    /// </summary>
    /// <remarks>
    /// Retorna todos os dias do ano informado com o status de frequência do aluno logado em cada dia:
    /// sem aula (fim de semana, feriado, férias, recesso ou dia sem aula do aluno), indefinido (aula futura
    /// ou frequência ainda não lançada), presença ou falta.
    /// </remarks>
    [HttpGet("students/attendances/calendar")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetStudentAttendanceCalendarIn data)
    {
        var calendar = await service.Get(data);
        return Ok(calendar);
    }
}

internal class RequestExamples : ExamplesProvider<GetStudentAttendanceCalendarIn>;
internal class ResponseExamples : ExamplesProvider<GetStudentAttendanceCalendarOut>;
