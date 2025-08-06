namespace Syki.Back.Features.Teacher.CreateLessonAttendance;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class CreateLessonAttendanceController(CreateLessonAttendanceService service) : ControllerBase
{
    /// <summary>
    /// Realizar chamada
    /// </summary>
    /// <remarks>
    /// Realiza a chamada da aula informada.
    /// </remarks>
    [HttpPut("teacher/lessons/{id}/attendance")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] CreateLessonAttendanceIn data)
    {
        var result = await service.Create(User.Id, id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateLessonAttendanceIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    LessonNotFound,
    ClassNotFound,
    InvalidStudentsList>;
