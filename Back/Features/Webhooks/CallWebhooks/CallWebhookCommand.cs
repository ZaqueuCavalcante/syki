using System.Text;

namespace Estud.Back.Features.Webhooks.CallWebhooks;

[CommandDescription("Chama um webhook")]
public record CallWebhookCommand(int WebhookCallId) : ICommand;

public class CallWebhookCommandHandler(EstudDbContext ctx, IHttpClientFactory factory) : ICommandHandler<CallWebhookCommand>
{
    public async Task Handle(int commandId, CallWebhookCommand command)
    {
        var call = await ctx.WebhookCalls
            .Include(x => x.Attempts)
            .FirstOrDefaultAsync(x => x.Id == command.WebhookCallId);

        var webhook = await ctx.WebhookSubscriptions.AsNoTracking()
            .Where(x => x.Id == call.WebhookSubscriptionId)
            .Select(x => new { x.Url, x.CustomHeaders })
            .FirstAsync();

        var client = factory.CreateClient();
        client.BaseAddress = new Uri(webhook.Url);
        foreach (var header in webhook.CustomHeaders)
        {
            client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        try
        {
            var payload = new StringContent(call.Payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("", payload);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                call.Success((int)response.StatusCode, responseContent);
            }
            else
            {
                call.Failed((int)response.StatusCode, responseContent);
            }
        }
        catch (Exception ex)
        {
            call.Failed(999, ex.Message);
        }
    }
}
