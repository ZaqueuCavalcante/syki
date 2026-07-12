using Dapper;

namespace Estud.Back.Features.Notifications.GetInstitutionNotification;

public class GetInstitutionNotificationService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetInstitutionNotificationOut, EstudError>> Get(int id)
    {
        var connection = await ctx.GetOpenConnectionAsync();

        var sql = @"
            SELECT
                n.id,
                n.title,
                n.created_at,
                n.description,
                COUNT(un.user_id) AS recipients,
                COUNT(un.viewed_at) AS viewed
            FROM
                estud.notifications n
            LEFT JOIN
                estud.user_notifications un ON un.notification_id = n.id
            WHERE
                n.id = @Id
                AND n.institution_id = @InstitutionId
            GROUP BY
                n.id, n.title, n.description, n.created_at;

            SELECT
                DATE(un.viewed_at) AS day,
                COUNT(*) AS views
            FROM
                estud.user_notifications un
            JOIN
                estud.notifications n ON n.id = un.notification_id
            WHERE
                un.notification_id = @Id
                AND n.institution_id = @InstitutionId
                AND un.viewed_at IS NOT NULL
            GROUP BY
                DATE(un.viewed_at)
            ORDER BY
                day;
        ";

        var parameters = new { Id = id, ctx.RequestUser.InstitutionId };

        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var notification = await multi.ReadFirstOrDefaultAsync<NotificationRow>();
        if (notification == null) return NotificationNotFound.I;

        var viewsByDay = (await multi.ReadAsync<GetInstitutionNotificationViewsByDayOut>()).ToList();

        return new GetInstitutionNotificationOut
        {
            Id = notification.Id,
            Title = notification.Title,
            Description = notification.Description,
            CreatedAt = notification.CreatedAt,
            Recipients = notification.Recipients,
            Viewed = notification.Viewed,
            ViewRate = notification.Recipients > 0
                ? Math.Round(100M * (1M * notification.Viewed / (1M * notification.Recipients)), 2)
                : 0,
            ViewsByDay = viewsByDay,
        };
    }
}
