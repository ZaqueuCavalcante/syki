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
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var campi = await service.Get(User.InstitutionId);
        return Ok(campi);
    }
}

internal class ResponseExamples : ExamplesProvider<GetCampiOut>;
