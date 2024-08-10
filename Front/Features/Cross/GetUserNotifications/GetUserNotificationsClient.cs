namespace Syki.Front.Features.Cross.GetUserNotifications;

public class GetUserNotificationsClient(HttpClient http) : ICrossClient
{
    public async Task<List<UserNotificationOut>> Get()
    {
        return await http.GetFromJsonAsync<List<UserNotificationOut>>("/notifications/user") ?? [];
    }
}
