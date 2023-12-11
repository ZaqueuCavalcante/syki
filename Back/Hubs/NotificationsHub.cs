using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace Syki.Back.Hubs;

[Authorize]
public sealed class NotificationsHub : Hub
{
    public async Task UpdateNotificationsCounter()
    {
        await Clients.All.SendAsync("OnUpdateNotificationsCounter", 0);
    }
}
