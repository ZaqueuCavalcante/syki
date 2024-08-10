namespace Syki.Front.Features.Cross.ViewNotifications;

public class ViewNotificationsClient(HttpClient http) : ICrossClient
{
    public async Task View()
    {
        await http.PutAsJsonAsync("/notifications/user", new {});
    }
}
