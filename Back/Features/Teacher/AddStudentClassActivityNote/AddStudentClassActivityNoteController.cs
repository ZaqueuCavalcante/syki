namespace Syki.Back.Features.Teacher.AddStudentClassActivityNote;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class AddStudentClassActivityNoteController(AddStudentClassActivityNoteService service) : ControllerBase
{
    /// <summary>
    /// Atribuir nota
    /// </summary>
    /// <remarks>
    /// Atribui a nota do aluno na atividade informada.
    /// </remarks>
    [HttpPost("teacher/class-activities/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Add([FromRoute] Guid id, [FromBody] AddStudentClassActivityNoteIn data)
    {
        var result = await service.Add(User.Id(), id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AddStudentClassActivityNoteIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ClassActivityNotFound,
    ClassNotFound,
    InvalidStudentClassNote>;
