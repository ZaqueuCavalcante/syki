namespace Syki.Back.Features.Notifications.MarkNotificationsAsViewed;

public class MarkNotificationsAsViewedService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<MarkNotificationsAsViewedIn>
    {
        public Validator()
        {
            When(x => !x.MarkAll, () =>
            {
                RuleFor(x => x.NotificationId).NotNull().WithError(InvalidNotificationId.I);
            });
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<SykiSuccess, SykiError>> MarkAsViewed(MarkNotificationsAsViewedIn data)
    {
        if (V.Run(data, out var error)) return error;

        var query = ctx.UserNotifications.Where(x => x.UserId == ctx.RequestUser.Id && x.ViewedAt == null);
        if (!data.MarkAll) query = query.Where(x => x.NotificationId == data.NotificationId);

        var notifications = await query.ToListAsync();
        foreach (var notification in notifications)
        {
            notification.ViewedAt = DateTime.UtcNow;
        }

        await ctx.SaveChangesAsync();

        return SykiSuccess.I;
    }
}
