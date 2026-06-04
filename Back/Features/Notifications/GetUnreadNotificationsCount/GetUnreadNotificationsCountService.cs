namespace Syki.Back.Features.Notifications.GetUnreadNotificationsCount;

public class GetUnreadNotificationsCountService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetUnreadNotificationsCountOut> Get()
    {
        var count = await ctx.UserNotifications.CountAsync(x =>
            x.UserId == ctx.RequestUser.Id && x.ViewedAt == null);

        return new GetUnreadNotificationsCountOut { Count = count };
    }
}
