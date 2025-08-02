namespace Syki.Back.Features.Academic.GetCampi;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCampiController(GetCampiService service) : ControllerBase
{
    /// <summary>
    /// Campi
    /// </summary>
    /// <remarks>
    /// Retorna todos os campus da insitituição.
    /// </remarks>
    [HttpGet("academic/campi")]
    [ProducesResponseType(typeof(CampusOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]

    public async Task<IActionResult> Get()
    {
        var campi = await service.Get(User.InstitutionId());

        return Ok(campi);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<CampusOut>
{
    public IEnumerable<SwaggerExample<CampusOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Agreste I",
			new CampusOut
            {
                Id = Guid.CreateVersion7(),
                Name = "Agreste I",
                State = BrazilState.PE,
                City = "Caruaru",
                Capacity = 150,
                Students = 120,
                FillRate = 80,
			}
		);
        yield return SwaggerExample.Create(
			"Suassuna I",
			new CampusOut
            {
                Id = Guid.CreateVersion7(),
                Name = "Suassuna I",
                State = BrazilState.PE,
                City = "Recife",
                Capacity = 500,
                Students = 234,
                FillRate = 46.80M,
			}
		);
    }
}
