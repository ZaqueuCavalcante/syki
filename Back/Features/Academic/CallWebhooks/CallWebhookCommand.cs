using System.Text;

namespace Syki.Back.Features.Academic.CallWebhooks;

[CommandDescription("Chama um webhook")]
public record CallWebhookCommand(Guid WebhookCallId) : ICommand;

public class CallWebhookCommandHandler(SykiDbContext ctx, IHttpClientFactory factory) : ICommandHandler<CallWebhookCommand>
{
    public async Task Handle(CommandId commandId, CallWebhookCommand command)
    {
        var call = await ctx.WebhookCalls
            .Include(x => x.Attempts)
            .FirstOrDefaultAsync(x => x.Id == command.WebhookCallId);

        var webhook = await ctx.Webhooks.AsNoTracking()
            .Include(x => x.Authentication)
            .Where(x => x.Id == call.WebhookId)
            .Select(x => new { x.Url, x.Authentication.ApiKey })
            .FirstAsync();

        var client = factory.CreateClient();
        client.BaseAddress = new Uri(webhook.Url);
        client.DefaultRequestHeaders.Add("Syki-Webhook-ApiKey", webhook.ApiKey);

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
