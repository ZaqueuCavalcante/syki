namespace Syki.Back.Features.Academic.CreateCampus;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateCampusController(CreateCampusService service) : ControllerBase
{
    /// <summary>
    /// Criar campus
    /// </summary>
    /// <remarks>
    /// Cria um novo campus.
    /// </remarks>
    [HttpPost("academic/campi")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateCampusIn data)
    {
        var campus = await service.Create(User.InstitutionId(), data);

        return Ok(campus);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<CreateCampusIn>
{
    public IEnumerable<SwaggerExample<CreateCampusIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Agreste I",
			new CreateCampusIn
            {
                Name = "Agreste I",
                State = BrazilState.PE,
                City = "Caruaru",
                Capacity = 150,
			}
		);
        yield return SwaggerExample.Create(
			"Suassuna I",
			new CreateCampusIn
            {
                Name = "Suassuna I",
                State = BrazilState.PE,
                City = "Recife",
                Capacity = 500,
			}
		);
    }
}
