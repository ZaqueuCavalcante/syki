namespace Estud.Back.Features.Teachers.GetTeacherClassStudents;

[ApiController, Authorize(Policies.GetTeacherClassStudents)]
public class GetTeacherClassStudentsController(GetTeacherClassStudentsService service) : ControllerBase
{
    /// <summary>
    /// Alunos da turma
    /// </summary>
    /// <remarks>
    /// Retorna os alunos de uma turma lecionada pelo professor logado,
    /// com o status de cada aluno na turma.
    /// </remarks>
    [HttpGet("teachers/classes/{id:int}/students")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTeacherClassStudentsOut>;
internal class ErrorsExamples : ErrorExamplesProvider<ClassNotFound, TeacherNotAssignedToClass>;
