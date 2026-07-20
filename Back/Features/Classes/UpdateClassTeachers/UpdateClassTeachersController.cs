namespace Estud.Back.Features.Classes.UpdateClassTeachers;

[ApiController, Authorize(Policies.UpdateClassTeachers)]
public class UpdateClassTeachersController(UpdateClassTeachersService service) : ControllerBase
{
    /// <summary>
    /// Vincular professores à turma
    /// </summary>
    /// <remarks>
    /// Define os professores que lecionam na turma, no máximo 2. Substitui a lista atual.
    /// </remarks>
    [HttpPut("classes/{classId}/teachers")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update(int classId, [FromBody] UpdateClassTeachersIn data)
    {
        var result = await service.Update(classId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateClassTeachersIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ClassNotFound,
    TeacherNotFound,
    InvalidTeachersList,
    TeacherNotAssignedToDiscipline
>;
