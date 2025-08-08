namespace Syki.Back.Features.Academic.AssignDisciplinesToTeacher;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class AssignDisciplinesToTeacherController(AssignDisciplinesToTeacherService service) : ControllerBase
{
    /// <summary>
    /// Vincular disciplinas
    /// </summary>
    /// <remarks>
    /// Vincula as disciplinas que o professor está apto a lecionar.
    /// </remarks>
    [HttpPut("academic/teachers/{id}/assign-disciplines")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Assign([FromRoute] Guid id, [FromBody] AssignDisciplinesToTeacherIn data)
    {
        var result = await service.Assign(User.InstitutionId, id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AssignDisciplinesToTeacherIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    TeacherNotFound,
    InvalidDisciplinesList>;
