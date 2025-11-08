using Exato.Shared.Features.Office.GetAuditTrails;

namespace Exato.Back.Features.Office.GetAuditTrails;

[ApiController, Authorize(Policies.GetAuditTrails)]
public class GetAuditTrailsController(GetAuditTrailsService service) : ControllerBase
{
    /// <summary>
    /// Audit trails
    /// </summary>
    /// <remarks>
    /// Retorna os dados de auditoria.
    /// </remarks>
    [HttpGet("office/audit-trails")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetAuditTrailsIn data)
    {
        var auditTrails = await service.Get(data);
        return Ok(auditTrails);
    }
}

internal class RequestExamples : ExamplesProvider<GetAuditTrailsIn>;
internal class ResponseExamples : ExamplesProvider<GetAuditTrailsOut>;
