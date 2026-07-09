namespace Estud.Back.Features.Teachers.UpdateTeacher;

[ApiController, Authorize(Policies.UpdateTeacher)]
public class UpdateTeacherController(UpdateTeacherService service) : ControllerBase
{
    /// <summary>
    /// Atualizar professor
    /// </summary>
    /// <remarks>
    /// Atualiza o nome e email de um professor.
    /// </remarks>
    [HttpPut("teachers/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTeacherIn data)
    {
        var result = await service.Update(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateTeacherIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    TeacherNotFound,
    InvalidTeacherName,
    InvalidEmail,
    EmailAlreadyUsed
>;
