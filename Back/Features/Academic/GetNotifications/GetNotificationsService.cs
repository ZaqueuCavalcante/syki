namespace Syki.Back.Features.Academic.GetNotifications;

public class GetNotificationsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<NotificationOut>> Get(Guid institutionId)
    {
        var notifications = await ctx.Notifications.AsNoTracking()
            .Include(x => x.Users)
            .Where(c => c.InstitutionId == institutionId)
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
