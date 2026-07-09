using System.Text.Json;
using Estud.Back.Converters;
using Estud.Back.Domain.Webhooks;

namespace Estud.Back.Webhooks;

public interface IWebhookEvent;

public interface IWebhookEventHandler<T> where T : IWebhookEvent
{
    Task Handle(ReceivedWebhookEvent evt, T data);
}

public interface IWebhookEventInvoker
{
    Task Invoke(IServiceProvider sp, ReceivedWebhookEvent evt);
}

public class WebhookEventInvoker<T> : IWebhookEventInvoker where T : IWebhookEvent
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new EstudStringEnumConverter() },
    };

    public async Task Invoke(IServiceProvider sp, ReceivedWebhookEvent evt)
    {
        var data = JsonSerializer.Deserialize<T>(evt.Payload, _options)!;
        var handler = sp.GetRequiredService<IWebhookEventHandler<T>>();
        await handler.Handle(evt, data);
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class WebhookEventTypeAttribute(string type) : Attribute
{
    public string Type { get; } = type;
}
