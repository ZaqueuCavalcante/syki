namespace Syki.Back.Features.Teacher.SetSchedulingPreferences;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class SetSchedulingPreferencesController(SetSchedulingPreferencesService service) : ControllerBase
{
    /// <summary>
    /// Definir horários
    /// </summary>
    /// <remarks>
    /// Define as preferências de horários do professor no semestre atual.
    /// </remarks>
    [HttpPut("teacher/scheduling-preferences")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Set([FromBody] SetSchedulingPreferencesIn data)
    {
        var result = await service.Set(User.InstitutionId(), User.Id(), data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<SetSchedulingPreferencesIn>
{
    public IEnumerable<SwaggerExample<SetSchedulingPreferencesIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Horários",
			new SetSchedulingPreferencesIn
            {
                Schedules =
				[
					new(Day.Monday, Hour.H07_00, Hour.H10_00),
					new(Day.Thursday, Hour.H08_00, Hour.H10_30),
				]
			}
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new InvalidDay().ToSwaggerExampleErrorOut();
        yield return new InvalidHour().ToSwaggerExampleErrorOut();
        yield return new InvalidSchedule().ToSwaggerExampleErrorOut();
        yield return new ConflictingSchedules().ToSwaggerExampleErrorOut();
    }
}
