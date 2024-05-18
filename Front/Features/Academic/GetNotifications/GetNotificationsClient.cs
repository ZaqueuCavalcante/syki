namespace Syki.Front.Features.Academic.GetNotifications;

public class GetNotificationsClient(HttpClient http)
{
    public async Task<List<NotificationOut>> Get()
    {
        return await http.GetFromJsonAsync<List<NotificationOut>>("/academic/notifications") ?? [];
    }
}
