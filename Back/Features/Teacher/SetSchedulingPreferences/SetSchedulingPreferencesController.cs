namespace Syki.Back.Features.Teacher.SetSchedulingPreferences;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class SetSchedulingPreferencesController(SetSchedulingPreferencesService service) : ControllerBase
{
    /// <summary>
    /// Definir horários
    /// </summary>
    /// <remarks>
    /// Define as preferências de horários do professor no semestre atual.
    /// </remarks>
    [HttpPut("teacher/scheduling-preferences")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Set([FromBody] SetSchedulingPreferencesIn data)
    {
        var result = await service.Set(User.InstitutionId(), User.Id(), data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<SetSchedulingPreferencesIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidDay,
    InvalidHour,
    InvalidSchedule,
    ConflictingSchedules>;
