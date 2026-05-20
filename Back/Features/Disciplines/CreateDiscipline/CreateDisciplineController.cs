namespace Syki.Back.Features.Disciplines.CreateDiscipline;

[ApiController, Authorize(Policies.CreateDiscipline)]
public class CreateDisciplineController(CreateDisciplineService service) : ControllerBase
{
    /// <summary>
    /// Criar disciplina
    /// </summary>
    /// <remarks>
    /// Cria uma nova disciplina.
    /// </remarks>
    [HttpPost("disciplines")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateDisciplineIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateDisciplineIn>;
internal class ResponseExamples : ExamplesProvider<CreateDisciplineOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidDisciplineName
>;
