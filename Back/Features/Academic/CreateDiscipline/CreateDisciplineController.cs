namespace Syki.Back.Features.Academic.CreateDiscipline;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateDisciplineController(CreateDisciplineService service) : ControllerBase
{
    /// <summary>
    /// Criar disciplina
    /// </summary>
    /// <remarks>
    /// Cria uma nova disciplina.
    /// </remarks>
    [HttpPost("academic/disciplines")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Create([FromBody] CreateDisciplineIn data)
    {
        var discipline = await service.Create(User.InstitutionId(), data);
        return Ok(discipline);
    }
}

internal class RequestExamples : ExamplesProvider<CreateDisciplineIn>;
internal class ResponseExamples : ExamplesProvider<DisciplineOut>;
