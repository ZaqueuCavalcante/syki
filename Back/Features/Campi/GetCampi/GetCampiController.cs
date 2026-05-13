namespace Syki.Back.Features.Campi.GetCampi;

[ApiController, Authorize]
[EnableRateLimiting("Medium")]
public class GetCampiController(GetCampiService service) : ControllerBase
{
    /// <summary>
    /// Campi
    /// </summary>
    /// <remarks>
    /// Retorna todos os campus da insitituição.
    /// </remarks>
    [HttpGet("campi")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var campi = await service.Get();
        return Ok(campi);
    }
}

internal class ResponseExamples : ExamplesProvider<GetCampiOut>;
