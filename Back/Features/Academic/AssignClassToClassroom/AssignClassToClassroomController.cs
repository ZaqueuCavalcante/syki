namespace Syki.Back.Features.Academic.AssignClassToClassroom;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class AssignClassToClassroomController(AssignClassToClassroomService service) : ControllerBase
{
    /// <summary>
    /// Vincular turma
    /// </summary>
    /// <remarks>
    /// Vincula uma turma a uma sala
    /// </remarks>
    [HttpPut("academic/classrooms/{id}/assign-class")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Assign([FromRoute] Guid id, [FromBody] AssignClassToClassroomIn data)
    {
        var result = await service.Assign(User.InstitutionId, id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AssignClassToClassroomIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ClassroomNotFound,
    ClassNotFound>;
