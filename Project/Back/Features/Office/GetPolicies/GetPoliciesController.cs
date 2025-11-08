using Exato.Shared.Features.Office.GetPolicies;

namespace Exato.Back.Features.Office.GetPolicies;

[ApiController, Authorize(Policies.GetPolicies)]
public class GetPoliciesController(GetPoliciesService service) : ControllerBase
{
    /// <summary>
    /// Policies
    /// </summary>
    /// <remarks>
    /// Retorna as policies do backend.
    /// </remarks>
    [HttpGet("office/policies")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public IActionResult Get()
    {
        var policies = service.Get();
        return Ok(policies);
    }
}

internal class ResponseExamples : ExamplesProvider<GetPoliciesOut>;
