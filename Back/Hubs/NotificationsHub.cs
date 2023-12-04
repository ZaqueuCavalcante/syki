using Microsoft.AspNetCore.SignalR;

namespace Syki.Back.Hubs;

public sealed class NotificationsHub : Hub
{
    public async Task UpdateNotificationsCounter()
    {
        await Clients.All.SendAsync("OnUpdateNotificationsCounter", 0);
    }
}
