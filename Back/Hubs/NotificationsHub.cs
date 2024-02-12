using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace Syki.Back.Hubs;

[Authorize]
public sealed class NotificationsHub : Hub
{
    // TODO: remove all SignalR things?
    public async Task UpdateNotificationsCounter()
    {
        await Clients.All.SendAsync("OnUpdateNotificationsCounter", 0);
    }
}
