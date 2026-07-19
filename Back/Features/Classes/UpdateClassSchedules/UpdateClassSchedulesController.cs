namespace Estud.Back.Features.Classes.UpdateClassSchedules;

[ApiController, Authorize(Policies.UpdateClassSchedules)]
public class UpdateClassSchedulesController(UpdateClassSchedulesService service) : ControllerBase
{
    /// <summary>
    /// Definir horários da turma
    /// </summary>
    /// <remarks>
    /// Define os horários semanais da turma. Substitui a lista atual (replace-all).
    /// Só é possível antes de a turma ser iniciada. Valida horários bem formados, sem
    /// choque entre si e sem conflito com outras turmas dos professores já atribuídos.
    /// </remarks>
    [HttpPut("classes/{classId}/schedules")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update(int classId, [FromBody] UpdateClassSchedulesIn data)
    {
        var result = await service.Update(classId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateClassSchedulesIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ClassNotFound,
    ClassAlreadyStarted,
    InvalidDay,
    InvalidHour,
    InvalidSchedule,
    ConflictingSchedules,
    ScheduleTeacherRequired,
    InvalidScheduleTeacher,
    TeacherScheduleConflict
>;
