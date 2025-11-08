using Exato.Shared.Features.Office.GetAuditTrailOperations;

namespace Exato.Back.Features.Office.GetAuditTrailOperations;

[ApiController, Authorize(Policies.GetAuditTrailOperations)]
public class GetAuditTrailOperationsController(GetAuditTrailOperationsService service) : ControllerBase
{
    /// <summary>
    /// Audit trail operations
    /// </summary>
    /// <remarks>
    /// Retorna as operações de auditoria.
    /// </remarks>
    [HttpGet("office/audit-trails/operations")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var operations = await service.Get();
        return Ok(operations);
    }
}

internal class ResponseExamples : ExamplesProvider<GetAuditTrailOperationsOut>;
