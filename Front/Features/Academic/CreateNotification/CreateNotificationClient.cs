namespace Syki.Front.Features.Academic.CreateNotification;

public class CreateNotificationClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string title, string description, UsersGroup targetUsers)
    {
        var data = new CreateNotificationIn(title, description,targetUsers);
        return await http.PostAsJsonAsync("/academic/notifications", data);
    }
}
