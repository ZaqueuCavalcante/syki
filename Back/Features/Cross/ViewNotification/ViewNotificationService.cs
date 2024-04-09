namespace Syki.Back.ViewNotification;

public class ViewNotificationService(SykiDbContext ctx)
{
    public async Task View(Guid faculdadeId, Guid userId)
    {
        var notifications = await ctx.UserNotifications
            .Include(x => x.Notification)
            .Where(c => c.Notification.FaculdadeId == faculdadeId && c.UserId == userId)
            .ToListAsync();

        notifications.ForEach(x => x.ViewedAt = DateTime.Now);

        await ctx.SaveChangesAsync();
    }
}
