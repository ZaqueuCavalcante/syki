namespace Estud.Back.Features.Classes.UpdateClassClassrooms;

[ApiController, Authorize(Policies.UpdateClassClassrooms)]
public class UpdateClassClassroomsController(UpdateClassClassroomsService service) : ControllerBase
{
    /// <summary>
    /// Definir salas da turma
    /// </summary>
    /// <remarks>
    /// Aloca uma sala para cada horário da turma. Substitui a alocação atual (replace-all).
    /// Só é possível antes de a turma ser iniciada. Valida que o horário existe na turma, que a
    /// sala e a turma estão no mesmo campus, que a sala comporta as vagas e que não há choque
    /// com outra turma já alocada na mesma sala.
    /// </remarks>
    [HttpPut("classes/{classId}/classrooms")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update(int classId, [FromBody] UpdateClassClassroomsIn data)
    {
        var result = await service.Update(classId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateClassClassroomsIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ClassNotFound,
    ClassAlreadyStarted,
    ScheduleNotFound,
    ClassroomNotFound,
    ClassAndClassroomDifferentCampus,
    ClassroomCapacityExceeded,
    ClassroomScheduleConflict
>;
