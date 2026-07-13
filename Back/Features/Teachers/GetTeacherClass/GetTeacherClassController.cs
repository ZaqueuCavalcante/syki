namespace Estud.Back.Features.Teachers.GetTeacherClass;

[ApiController, Authorize(Policies.GetTeacherClass)]
public class GetTeacherClassController(GetTeacherClassService service) : ControllerBase
{
    /// <summary>
    /// Buscar turma do professor
    /// </summary>
    /// <remarks>
    /// Retorna os detalhes de uma turma lecionada pelo professor logado,
    /// incluindo horários e alunos matriculados.
    /// </remarks>
    [HttpGet("teachers/classes/{id:int}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTeacherClassOut>;
internal class ErrorsExamples : ErrorExamplesProvider<ClassNotFound, TeacherNotAssignedToClass>;
