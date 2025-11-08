using Exato.Shared.Features.Office.GetAuditTrail;

namespace Exato.Back.Features.Office.GetAuditTrail;

[ApiController, Authorize(Policies.GetAuditTrail)]
public class GetAuditTrailController(GetAuditTrailService service) : ControllerBase
{
    /// <summary>
    /// Audit trail
    /// </summary>
    /// <remarks>
    /// Retorna o audit trail especificado.
    /// </remarks>
    [HttpGet("office/audit-trails/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetAuditTrailOut>;
