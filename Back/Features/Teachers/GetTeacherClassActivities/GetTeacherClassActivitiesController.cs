namespace Estud.Back.Features.Teachers.GetTeacherClassActivities;

[ApiController, Authorize(Policies.GetTeacherClassActivities)]
public class GetTeacherClassActivitiesController(GetTeacherClassActivitiesService service) : ControllerBase
{
    /// <summary>
    /// Atividades da turma
    /// </summary>
    /// <remarks>
    /// Retorna as atividades de uma turma lecionada pelo professor logado,
    /// com o total de entregas de cada atividade.
    /// </remarks>
    [HttpGet("teachers/classes/{id:int}/activities")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTeacherClassActivitiesOut>;
internal class ErrorsExamples : ErrorExamplesProvider<ClassNotFound, TeacherNotAssignedToClass>;
