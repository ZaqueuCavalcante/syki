using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Estud.Back.DomainEvents;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(int institutionId, int eventId, T evt);
}

public interface IDomainEventInvoker
{
    Task Invoke(IServiceProvider sp, int institutionId, int eventId, string json);
}

public class DomainEventInvoker<T> : IDomainEventInvoker where T : IDomainEvent
{
    private static readonly JsonSerializerSettings _settings = new()
    {
        Converters = [new StringEnumConverter()],
    };

    public async Task Invoke(IServiceProvider sp, int institutionId, int eventId, string json)
    {
        var data = JsonConvert.DeserializeObject<T>(json, _settings)!;

        foreach (var handler in sp.GetServices<IDomainEventHandler<T>>())
        {
            await handler.Handle(institutionId, eventId, data);
        }
    }
}
