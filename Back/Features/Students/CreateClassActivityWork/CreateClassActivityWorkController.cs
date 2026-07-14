namespace Estud.Back.Features.Students.CreateClassActivityWork;

[ApiController, Authorize(Policies.CreateClassActivityWork)]
public class CreateClassActivityWorkController(CreateClassActivityWorkService service) : ControllerBase
{
    /// <summary>
    /// Entregar atividade
    /// </summary>
    /// <remarks>
    /// Registra a entrega do aluno logado para a atividade especificada,
    /// salvando o link do material entregue.
    /// </remarks>
    [HttpPost("students/activities/{id:int}/works")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromRoute] int id, [FromBody] CreateClassActivityWorkIn data)
    {
        var result = await service.Create(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateClassActivityWorkIn>;
internal class ResponseExamples : ExamplesProvider<CreateClassActivityWorkOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidClassActivityWorkLink,
    ClassActivityNotFound,
    StudentNotEnrolledInClass,
    ClassActivityWorkNotFound
>;
