namespace Estud.Back.Features.Calendar.UpdateCalendarDay;

[ApiController, Authorize(Policies.UpdateCalendarDay)]
public class UpdateCalendarDayController(UpdateCalendarDayService service) : ControllerBase
{
    /// <summary>
    /// Editar dia do calendário
    /// </summary>
    /// <remarks>
    /// Edita o tipo e a descrição de um dia já customizado do calendário acadêmico da instituição.
    /// </remarks>
    [HttpPut("calendar/days")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateCalendarDayIn data)
    {
        var result = await service.Update(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateCalendarDayIn>;
internal class ResponseExamples : ExamplesProvider<UpdateCalendarDayOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidCalendarDayType,
    InvalidCalendarDayDescription,
    CalendarDayNotFound
>;
