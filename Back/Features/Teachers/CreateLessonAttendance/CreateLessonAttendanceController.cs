namespace Estud.Back.Features.Teachers.CreateLessonAttendance;

[ApiController, Authorize(Policies.CreateLessonAttendance)]
public class CreateLessonAttendanceController(CreateLessonAttendanceService service) : ControllerBase
{
    /// <summary>
    /// Realizar chamada
    /// </summary>
    /// <remarks>
    /// Realiza a chamada da aula informada, registrando a presença de cada aluno matriculado na turma.
    /// Apenas o professor da turma pode fazer a chamada. Ao final, a aula é finalizada.
    /// </remarks>
    [HttpPut("teachers/lessons/{id:int}/attendance")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromRoute] int id, [FromBody] CreateLessonAttendanceIn data)
    {
        var result = await service.Create(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateLessonAttendanceIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ClassLessonNotFound,
    TeacherNotAssignedToClass,
    InvalidStudentsList
>;
