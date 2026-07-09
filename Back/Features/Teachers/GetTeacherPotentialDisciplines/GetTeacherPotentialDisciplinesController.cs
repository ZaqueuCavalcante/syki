namespace Estud.Back.Features.Teachers.GetTeacherPotentialDisciplines;

[ApiController, Authorize(Policies.GetTeacherPotentialDisciplines)]
public class GetTeacherPotentialDisciplinesController(GetTeacherPotentialDisciplinesService service) : ControllerBase
{
    /// <summary>
    /// Disciplinas disponíveis para vincular ao professor
    /// </summary>
    /// <remarks>
    /// Retorna as disciplinas ainda não vinculadas ao professor, com suporte a pesquisa por nome.
    /// </remarks>
    [HttpGet("teachers/{id}/potential-disciplines")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id, [FromQuery] string? name)
    {
        var result = await service.Get(id, name);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTeacherPotentialDisciplinesOut>;
internal class ErrorsExamples : ErrorExamplesProvider<TeacherNotFound>;
