using Microsoft.AspNetCore.SignalR;

namespace Syki.Back.Hubs;

[AuthBearer]
public class SykiHub : Hub
{
    public override Task OnConnectedAsync()
    {
        var userId = Context.User.Id;

        if (SykiHubUsersStore.Users.ContainsKey(userId))
        {
            SykiHubUsersStore.Users[userId].Add(Context.ConnectionId);
        }
        else
        {
            SykiHubUsersStore.Users.TryAdd(userId, [Context.ConnectionId]);
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.Id;
        
        if (SykiHubUsersStore.Users.ContainsKey(userId))
        {
            SykiHubUsersStore.Users[userId].Remove(Context.ConnectionId);
            if (SykiHubUsersStore.Users[userId].Count == 0)
            {
                SykiHubUsersStore.Users.Remove(userId, out _);
            }
        }
        
        return base.OnDisconnectedAsync(exception);
    }
}
