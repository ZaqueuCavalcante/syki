namespace Syki.Back.Features.Cross.ViewNotification;

public class ViewNotificationService(SykiDbContext ctx)
{
    public async Task View(Guid institutionId, Guid userId)
    {
        var notifications = await ctx.UserNotifications
            .Include(x => x.Notification)
            .Where(c => c.Notification.InstitutionId == institutionId && c.UserId == userId)
            .ToListAsync();

        notifications.ForEach(x => x.ViewedAt = DateTime.Now);

        await ctx.SaveChangesAsync();
    }
}
