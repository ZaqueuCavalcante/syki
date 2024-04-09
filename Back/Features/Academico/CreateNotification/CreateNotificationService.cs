namespace Syki.Back.CreateNotification;

public class CreateNotificationService(SykiDbContext ctx)
{
    public async Task<NotificationOut> Create(Guid faculdadeId, NotificationIn data)
    {
        var notification = new Notification(faculdadeId, data.Title, data.Description);

        var roleName = data.UsersGroup == "Alunos" ? "Aluno" : "Professor";
        FormattableString sql = $@"
            SELECT
                u.id
            FROM
                syki.users u
            INNER JOIN
                syki.roles r ON r.name = {roleName}
            INNER JOIN
                syki.user_roles ur ON ur.user_id = u.id AND ur.role_id = r.id
            WHERE
                u.institution_id = {faculdadeId}
        ";

        var roleId = await ctx.Roles.Where(r => r.Name == roleName).Select(r => r.Id).FirstAsync();
        var users = await ctx.Database.SqlQuery<Guid>(sql).ToListAsync();

        users.ForEach(u => ctx.UserNotifications.Add(new UserNotification(u, notification.Id)));

        ctx.Add(notification);
        await ctx.SaveChangesAsync();

        return notification.ToOut();
    }
}
