using Microsoft.AspNetCore.SignalR;

namespace Estud.Back.Hubs;

[Authorize]
public class EstudHub : Hub
{
    public override Task OnConnectedAsync()
    {
        var userId = Context.User.Id;

        if (EstudHubUsersStore.Users.ContainsKey(userId))
        {
            EstudHubUsersStore.Users[userId].Add(Context.ConnectionId);
        }
        else
        {
            EstudHubUsersStore.Users.TryAdd(userId, [Context.ConnectionId]);
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.Id;
        
        if (EstudHubUsersStore.Users.ContainsKey(userId))
        {
            EstudHubUsersStore.Users[userId].Remove(Context.ConnectionId);
            if (EstudHubUsersStore.Users[userId].Count == 0)
            {
                EstudHubUsersStore.Users.Remove(userId, out _);
            }
        }
        
        return base.OnDisconnectedAsync(exception);
    }
}
