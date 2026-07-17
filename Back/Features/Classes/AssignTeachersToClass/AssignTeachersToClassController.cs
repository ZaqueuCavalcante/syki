namespace Estud.Back.Features.Classes.AssignTeachersToClass;

[ApiController, Authorize(Policies.AssignTeachersToClass)]
public class AssignTeachersToClassController(AssignTeachersToClassService service) : ControllerBase
{
    /// <summary>
    /// Vincular professores à turma
    /// </summary>
    /// <remarks>
    /// Define os professores que lecionam na turma, no máximo 2. Substitui a lista atual.
    /// </remarks>
    [HttpPost("classes/{classId}/teachers")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Assign(int classId, [FromBody] AssignTeachersToClassIn data)
    {
        var result = await service.Assign(classId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AssignTeachersToClassIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ClassNotFound,
    TeacherNotFound,
    InvalidTeachersList,
    TeacherNotAssignedToDiscipline
>;
