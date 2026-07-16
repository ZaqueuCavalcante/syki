namespace Estud.Back.Features.Campi.CreateCampus;

[ApiController, Authorize(Policies.CreateCampus)]
public class CreateCampusController(CreateCampusService service) : ControllerBase
{
    /// <summary>
    /// Criar campus
    /// </summary>
    /// <remarks>
    /// Cria um novo campus.
    /// </remarks>
    [HttpPost("campi")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateCampusIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateCampusIn>;
internal class ResponseExamples : ExamplesProvider<CreateCampusOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidCampusName,
    InvalidBrazilState,
    InvalidCampusCity
>;
