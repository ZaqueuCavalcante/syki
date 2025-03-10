using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Back.Features.Cross.LinkOldNotifications;

[CommandDescription("Vincular notificações ao novo usuário")]
public record LinkOldNotificationsCommand(Guid InstitutionId, Guid UserId) : ICommand;

public class LinkOldNotificationsCommandHandler(SykiDbContext ctx) : ICommandHandler<LinkOldNotificationsCommand>
{
    public async Task Handle(Guid commandId, LinkOldNotificationsCommand command)
    {
        var isStudent = await ctx.Students.AnyAsync(s => s.Id == command.UserId);
        var group = isStudent ? UsersGroup.Students : UsersGroup.Teachers;

        var userNotifications = await ctx.UserNotifications
            .Where(x => x.UserId == command.UserId)
            .Select(x => x.NotificationId)
            .ToListAsync();

        var notifications = await ctx.Notifications.AsNoTracking()
            .Where(x => x.InstitutionId == command.InstitutionId && x.Timeless && (x.Target == group || x.Target == UsersGroup.All) && !userNotifications.Contains(x.Id))
            .ToListAsync();

        foreach (var notification in notifications)
        {
            ctx.Add(new UserNotification(command.UserId, notification.Id));
        }

        await ctx.SaveChangesAsync();
    }
}
