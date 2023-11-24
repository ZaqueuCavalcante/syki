using Microsoft.AspNetCore.SignalR;

namespace Syki.Back.Hubs;

public sealed class NotificationsHub : Hub
{
    public int Counter { get; set; }

    public async Task UpdateCounter()
    {
        Counter = (new Random()).Next();
        await Clients.All.SendAsync("OnCounterUpdate", Counter);
    }
}
