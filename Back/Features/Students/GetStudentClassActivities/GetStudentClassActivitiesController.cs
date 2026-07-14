namespace Estud.Back.Features.Students.GetStudentClassActivities;

[ApiController, Authorize(Policies.GetStudentClassActivities)]
public class GetStudentClassActivitiesController(GetStudentClassActivitiesService service) : ControllerBase
{
    /// <summary>
    /// Atividades da turma
    /// </summary>
    /// <remarks>
    /// Retorna as atividades de uma turma em que o aluno logado está matriculado,
    /// com o status e a nota da entrega dele em cada atividade.
    /// </remarks>
    [HttpGet("students/classes/{id:int}/activities")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetStudentClassActivitiesOut>;
internal class ErrorsExamples : ErrorExamplesProvider<ClassNotFound, StudentNotEnrolledInClass>;
