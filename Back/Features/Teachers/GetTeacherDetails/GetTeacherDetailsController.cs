namespace Estud.Back.Features.Teachers.GetTeacherDetails;

[ApiController, Authorize(Policies.GetTeacherDetails)]
public class GetTeacherDetailsController(GetTeacherDetailsService service) : ControllerBase
{
    /// <summary>
    /// Buscar detalhes do professor
    /// </summary>
    /// <remarks>
    /// Retorna os detalhes de um professor da instituição do usuário logado, incluindo campus, disciplinas e turmas.
    /// </remarks>
    [HttpGet("teachers/{id:int}/details")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<GetTeacherDetailsOut>;
internal class ResponseExamples : ExamplesProvider<GetTeacherDetailsOut>;
internal class ErrorsExamples : ErrorExamplesProvider<TeacherNotFound>;
