namespace Estud.Back.Features.Notifications.GetUnreadNotificationsCount;

public class GetUnreadNotificationsCountService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetUnreadNotificationsCountOut> Get()
    {
        var count = await ctx.UserNotifications.CountAsync(x =>
            x.UserId == ctx.RequestUser.Id && x.ViewedAt == null);

        return new GetUnreadNotificationsCountOut { Count = count };
    }
}
