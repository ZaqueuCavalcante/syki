namespace Estud.Back.Features.Teachers.CreateClassActivity;

[ApiController, Authorize(Policies.CreateClassActivity)]
public class CreateClassActivityController(CreateClassActivityService service) : ControllerBase
{
    /// <summary>
    /// Criar atividade
    /// </summary>
    /// <remarks>
    /// Cria uma atividade vinculada à uma turma lecionada pelo professor logado,
    /// gerando as entregas pendentes de cada aluno matriculado.
    /// </remarks>
    [HttpPost("teachers/classes/{id:int}/activities")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromRoute] int id, [FromBody] CreateClassActivityIn data)
    {
        var result = await service.Create(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateClassActivityIn>;
internal class ResponseExamples : ExamplesProvider<CreateClassActivityOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ClassNotFound,
    TeacherNotAssignedToClass,
    InvalidClassActivityWeight
>;
