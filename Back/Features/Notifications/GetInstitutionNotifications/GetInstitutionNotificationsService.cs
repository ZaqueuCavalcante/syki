using Dapper;

namespace Syki.Back.Features.Notifications.GetInstitutionNotifications;

public class GetInstitutionNotificationsService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<GetInstitutionNotificationsOut, SykiError>> Get(GetInstitutionNotificationsIn data)
    {
        var connection = await ctx.GetOpenConnectionAsync();

        var sql = @"
            SELECT
                n.id,
                n.title,
                n.created_at,
                n.description,
                COUNT(un.user_id) AS recipients,
                COUNT(un.viewed_at) AS viewed,
                COUNT(*) OVER() AS total_rows
            FROM
                syki.notifications n
            LEFT JOIN
                syki.user_notifications un ON un.notification_id = n.id
            WHERE
                n.institution_id = @InstitutionId
                AND n.notification_type = @NotificationType
            GROUP BY
                n.id, n.title, n.description, n.created_at
            ORDER BY
                n.created_at DESC
            LIMIT @PageSize
            OFFSET @Offset
        ";

        var parameters = new
        {
            data.PageSize,
            ctx.RequestUser.InstitutionId,
            Offset = (data.Page - 1) * data.PageSize,
            NotificationType = (int)NotificationType.Custom,
        };

        var rows = (await connection.QueryAsync<InstitutionNotificationRow>(sql, parameters)).ToList();

        var items = rows.ConvertAll(r => new GetInstitutionNotificationsItemOut
        {
            Id = r.Id,
            Title = r.Title,
            Viewed = r.Viewed,
            CreatedAt = r.CreatedAt,
            Recipients = r.Recipients,
            Description = r.Description,
            ViewRate = r.Recipients > 0 ? Math.Round(100M * (1M * r.Viewed / (1M * r.Recipients)), 2) : 0,
        });

        return new GetInstitutionNotificationsOut
        {
            Items = items,
            Page = data.Page,
            PageSize = data.PageSize,
            Total = rows.FirstOrDefault()?.TotalRows ?? 0,
        };
    }
}
