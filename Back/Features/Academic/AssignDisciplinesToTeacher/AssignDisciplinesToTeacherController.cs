namespace Syki.Back.Features.Academic.AssignDisciplinesToTeacher;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class AssignDisciplinesToTeacherController(AssignDisciplinesToTeacherService service) : ControllerBase
{
    /// <summary>
    /// Vincular disciplinas
    /// </summary>
    /// <remarks>
    /// Vincula disciplinas que o professor esta apto a lecionar.
    /// </remarks>
    [HttpPut("academic/teachers/{id}/assign-disciplines")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Assign([FromRoute] Guid id, [FromBody] AssignDisciplinesToTeacherIn data)
    {
        var result = await service.Assign(User.InstitutionId(), id, data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<AssignDisciplinesToTeacherIn>
{
    public IEnumerable<SwaggerExample<AssignDisciplinesToTeacherIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Disciplinas",
			new AssignDisciplinesToTeacherIn
			{
				Disciplines = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()]
			}
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new TeacherNotFound().ToSwaggerExampleErrorOut();
        yield return new InvalidDisciplinesList().ToSwaggerExampleErrorOut();
    }
}
