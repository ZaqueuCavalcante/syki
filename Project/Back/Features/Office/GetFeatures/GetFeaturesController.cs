using Exato.Shared.Features.Office.GetFeatures;

namespace Exato.Back.Features.Office.GetFeatures;

[ApiController, Authorize(Policies.GetFeatures)]
public class GetFeaturesController(GetFeaturesService service) : ControllerBase
{
    /// <summary>
    /// Features
    /// </summary>
    /// <remarks>
    /// Retorna as features do sistema.
    /// </remarks>
    [HttpGet("office/features")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public IActionResult Get()
    {
        var features = service.Get();
        return Ok(features);
    }
}

internal class ResponseExamples : ExamplesProvider<GetFeaturesOut>;
