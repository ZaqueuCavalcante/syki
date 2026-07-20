namespace Estud.Back.Features.Parents.GetParentDetails;

[ApiController, Authorize(Policies.GetParentDetails)]
public class GetParentDetailsController(GetParentDetailsService service) : ControllerBase
{
    /// <summary>
    /// Buscar detalhes do responsável
    /// </summary>
    /// <remarks>
    /// Retorna os detalhes de um responsável da instituição do usuário logado, incluindo todos os alunos vinculados a ele.
    /// </remarks>
    [HttpGet("parents/{parentId:int}/details")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int parentId)
    {
        var result = await service.Get(parentId);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<GetParentDetailsOut>;
internal class ResponseExamples : ExamplesProvider<GetParentDetailsOut>;
internal class ErrorsExamples : ErrorExamplesProvider<ParentNotFound>;
