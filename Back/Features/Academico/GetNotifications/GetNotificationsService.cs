namespace Syki.Back.GetNotifications;

public class GetNotificationsService(SykiDbContext ctx)
{
    public async Task<List<NotificationOut>> Get(Guid faculdadeId)
    {
        var notifications = await ctx.Notifications.AsNoTracking()
            .Include(x => x.Users)
            .Where(c => c.FaculdadeId == faculdadeId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        notifications.ForEach(n =>
        {
            var viewed = n.Users.Count(u => u.ViewedAt != null);
            var total = n.Users.Count;
            n.Views = $"{viewed}/{total}";
        });

        return notifications.ConvertAll(p => p.ToOut());
    }
}
