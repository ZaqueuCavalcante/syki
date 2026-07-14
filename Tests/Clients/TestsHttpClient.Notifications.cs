using System.Net.Http.Json;
using Estud.Back.Features.Notifications.GetNotifications;
using Estud.Back.Features.Notifications.CreateNotification;
using Estud.Back.Features.Notifications.MarkNotificationsAsViewed;
using Estud.Back.Features.Notifications.GetInstitutionNotification;
using Estud.Back.Features.Notifications.GetUnreadNotificationsCount;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateNotificationOut, ErrorOut>> CreateNotification(
        string title = "Aviso importante",
        string description = "Descrição do aviso importante.",
        UsersGroup targetUsers = UsersGroup.All
    ) {
        var data = new CreateNotificationIn(title, description, targetUsers);
        var response = await http.PostAsJsonAsync("notifications", data);
        return await response.Resolve<CreateNotificationOut>();
    }

    public async Task<OneOf<GetUnreadNotificationsCountOut, ErrorOut>> GetUnreadNotificationsCount()
    {
        var response = await http.GetAsync("notifications/unread-count");
        return await response.Resolve<GetUnreadNotificationsCountOut>();
    }

    public async Task<OneOf<GetNotificationsOut, ErrorOut>> GetNotifications(
        int page = 1,
        int pageSize = 20,
        bool unreadOnly = false
    ) {
        var data = new GetNotificationsIn
        {
            Page = page,
            PageSize = pageSize,
            UnreadOnly = unreadOnly,
        };

        var response = await http.GetAsync("notifications".AddQueryString(data));
        return await response.Resolve<GetNotificationsOut>();
    }

    public async Task<OneOf<GetInstitutionNotificationOut, ErrorOut>> GetInstitutionNotification(int id)
    {
        var response = await http.GetAsync($"notifications/institution/{id}");
        return await response.Resolve<GetInstitutionNotificationOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> MarkNotificationsAsViewed(
        bool markAll = false,
        int? notificationId = null
    ) {
        var data = new MarkNotificationsAsViewedIn { MarkAll = markAll, NotificationId = notificationId };
        var response = await http.PutAsJsonAsync("notifications/mark-as-viewed", data);
        return await response.Resolve<SuccessOut>();
    }
}
