namespace Syki.Back.Features.Academic.CreateNotification;

public class CreateNotificationService(SykiDbContext ctx)
{
    public async Task<NotificationOut> Create(Guid institutionId, CreateNotificationIn data)
    {
        var notification = new Notification(institutionId, data.Title, data.Description);

        if (data.TargetUsers is UsersGroup.All or UsersGroup.Teachers)
        {
            await CreateNotificationFor(institutionId, notification.Id, UserRole.Teacher);
        }
        if (data.TargetUsers is UsersGroup.All or UsersGroup.Students)
        {
            await CreateNotificationFor(institutionId, notification.Id, UserRole.Student);
        }

        ctx.Add(notification);
        await ctx.SaveChangesAsync();

        return notification.ToOut();
    }

    private async Task CreateNotificationFor(Guid institutionId, Guid notificationId, UserRole userRole)
    {
        FormattableString sql = $@"
            SELECT
                u.id
            FROM
                syki.users u
            INNER JOIN
                syki.roles r ON r.name = {userRole.ToString()}
            INNER JOIN
                syki.user_roles ur ON ur.user_id = u.id AND ur.role_id = r.id
            WHERE
                u.institution_id = {institutionId}
        ";

        var usersIds = await ctx.Database.SqlQuery<Guid>(sql).ToListAsync();

        usersIds.ForEach(userId => ctx.UserNotifications.Add(new UserNotification(userId, notificationId)));
    }
}
