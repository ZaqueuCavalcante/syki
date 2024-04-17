namespace Syki.Back.GetUserNotifications;

public class GetUserNotificationsService(SykiDbContext ctx)
{
    public async Task<List<UserNotificationOut>> Get(Guid institutionId, Guid userId)
    {
        var notifications = await ctx.UserNotifications.AsNoTracking()
            .Include(x => x.Notification)
            .Where(c => c.Notification.InstitutionId == institutionId && c.UserId == userId)
            .ToListAsync();
        
        return notifications.ConvertAll(p => p.ToOut());
    }
}
