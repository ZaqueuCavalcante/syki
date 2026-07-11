namespace Estud.Back.Features.Classes.GetClass;

[ApiController, Authorize(Policies.GetClass)]
public class GetClassController(GetClassService service) : ControllerBase
{
    /// <summary>
    /// Buscar turma
    /// </summary>
    /// <remarks>
    /// Retorna os detalhes de uma turma da instituição do usuário logado, incluindo horários e alunos matriculados.
    /// </remarks>
    [HttpGet("classes/{id:int}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<GetClassOut>;
internal class ResponseExamples : ExamplesProvider<GetClassOut>;
internal class ErrorsExamples : ErrorExamplesProvider<ClassNotFound>;
