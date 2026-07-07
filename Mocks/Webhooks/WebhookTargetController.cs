using Microsoft.AspNetCore.Mvc;

namespace Syki.Mocks.Webhooks;

/// <summary>
/// Simula o sistema target de um cliente que configurou um webhook no Syki.
/// Ecoa de volta os headers e o corpo recebidos para que os testes possam
/// validar a entrega ponta a ponta, incluindo os custom headers configurados
/// na assinatura do webhook.
/// </summary>
[ApiController]
public class WebhookTargetController : ControllerBase
{
    [HttpPost("webhooks/target")]
    public async Task<IActionResult> Receive()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

        return Ok(new { received = true, headers, body });
    }

    [HttpPost("webhooks/target/error")]
    public IActionResult ReceiveWithError()
    {
        return StatusCode(500, new { received = false, error = "target system unavailable" });
    }
}
