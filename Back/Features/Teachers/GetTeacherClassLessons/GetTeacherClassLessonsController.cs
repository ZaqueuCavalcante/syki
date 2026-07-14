namespace Estud.Back.Features.Teachers.GetTeacherClassLessons;

[ApiController, Authorize(Policies.GetTeacherClassLessons)]
public class GetTeacherClassLessonsController(GetTeacherClassLessonsService service) : ControllerBase
{
    /// <summary>
    /// Aulas da turma
    /// </summary>
    /// <remarks>
    /// Retorna as aulas de uma turma lecionada pelo professor logado,
    /// com os alunos marcados como presentes na chamada de cada aula.
    /// </remarks>
    [HttpGet("teachers/classes/{id:int}/lessons")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTeacherClassLessonsOut>;
internal class ErrorsExamples : ErrorExamplesProvider<ClassNotFound, TeacherNotAssignedToClass>;
