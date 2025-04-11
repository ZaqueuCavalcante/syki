using Microsoft.AspNetCore.SignalR;

namespace Syki.Back.Hubs;

[AuthBearer]
public class SykiHub : Hub
{
    public override Task OnConnectedAsync()
    {
        var userId = Context.User.Id();
        SykiHubUsersStore.ConnectedIds.Add(userId);
        Log.Information("Client connected: {0} / UserId = {1}", Context.ConnectionId, userId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.Id();
        SykiHubUsersStore.ConnectedIds.Remove(userId);
        Log.Information("Client disconnected: {0} / UserId = {1}", Context.ConnectionId, userId);
        return base.OnDisconnectedAsync(exception);
    }
}
