namespace Syki.Back.Features.Teachers.AssignDisciplinesToTeacher;

[ApiController, Authorize(Policies.AssignDisciplinesToTeacher)]
public class AssignDisciplinesToTeacherController(AssignDisciplinesToTeacherService service) : ControllerBase
{
    /// <summary>
    /// Vincular disciplinas ao professor
    /// </summary>
    /// <remarks>
    /// Define as disciplinas que o professor está apto a lecionar. Substitui a lista atual.
    /// </remarks>
    [HttpPut("teachers/{id}/assign-disciplines")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Assign(int id, [FromBody] AssignDisciplinesToTeacherIn data)
    {
        var result = await service.Assign(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AssignDisciplinesToTeacherIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    TeacherNotFound,
    InvalidDisciplinesList
>;
