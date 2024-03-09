namespace Syki.Back.Services;

public class NotificationsService(SykiDbContext ctx) : INotificationsService
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

    public async Task<List<NotificationOut>> GetAll(Guid faculdadeId)
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

    public async Task<List<UserNotificationOut>> GetByUserId(Guid faculdadeId, Guid userId)
    {
        var notifications = await ctx.UserNotifications.AsNoTracking()
            .Include(x => x.Notification)
            .Where(c => c.Notification.FaculdadeId == faculdadeId && c.UserId == userId)
            .ToListAsync();
        
        return notifications.ConvertAll(p => p.ToOut());
    }

    public async Task ViewByUserId(Guid faculdadeId, Guid userId)
    {
        var notifications = await ctx.UserNotifications
            .Include(x => x.Notification)
            .Where(c => c.Notification.FaculdadeId == faculdadeId && c.UserId == userId)
            .ToListAsync();

        notifications.ForEach(x => x.ViewedAt = DateTime.Now);

        await ctx.SaveChangesAsync();
    }
}
