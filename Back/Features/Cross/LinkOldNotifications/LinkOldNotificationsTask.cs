using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Back.Features.Cross.LinkOldNotifications;

[SykiTaskDescription("Vincular notificações ao novo usuário")]
public record LinkOldNotificationsTask(Guid UserId, Guid InstitutionId) : ISykiTask;

public class LinkOldNotificationsTaskHandler(SykiDbContext ctx) : ISykiTaskHandler<LinkOldNotificationsTask>
{
    public async Task Handle(LinkOldNotificationsTask task)
    {
        var isStudent = await ctx.Students.AnyAsync(s => s.Id == task.UserId);
        var group = isStudent ? UsersGroup.Students : UsersGroup.Teachers;

        var userNotifications = await ctx.UserNotifications
            .Where(x => x.UserId == task.UserId)
            .Select(x => x.NotificationId)
            .ToListAsync();

        var notifications = await ctx.Notifications.AsNoTracking()
            .Where(x => x.InstitutionId == task.InstitutionId && (x.Target == group || x.Target == UsersGroup.All) && x.Timeless && !userNotifications.Contains(x.Id))
            .ToListAsync();

        foreach (var notification in notifications)
        {
            ctx.Add(new UserNotification(task.UserId, notification.Id));
        }

        await ctx.SaveChangesAsync();
    }
}
