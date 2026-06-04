using Dapper;
using System.Text.Json;

namespace Syki.Back.Features.Notifications.GetNotifications;

public class GetNotificationsService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<GetNotificationsOut, SykiError>> Get(GetNotificationsIn data)
    {
        var connection = await ctx.GetOpenConnectionAsync();

        var unreadFilter = data.UnreadOnly ? "AND un.viewed_at IS NULL" : "";

        var sql = $@"
            SELECT
                n.id,
                n.notification_type,
                n.title,
                n.description,
                n.created_at,
                un.viewed_at,
                n.metadata::text AS metadata,
                COUNT(*) OVER() AS total_rows
            FROM
                syki.user_notifications un
            JOIN
                syki.notifications n ON n.id = un.notification_id
            WHERE
                un.user_id = @UserId
                {unreadFilter}
            ORDER BY n.created_at DESC
            LIMIT @PageSize
            OFFSET @Offset
        ";

        var offset = (data.Page - 1) * data.PageSize;
        var parameters = new { UserId = ctx.RequestUser.Id, data.PageSize, Offset = offset };

        var rows = (await connection.QueryAsync<NotificationRow>(sql, parameters)).ToList();

        var items = rows.Select(r => new GetNotificationsItemOut
        {
            Id = r.Id,
            NotificationType = (NotificationType)r.NotificationType,
            Title = r.Title,
            Description = r.Description,
            CreatedAt = r.CreatedAt,
            ViewedAt = r.ViewedAt,
            Metadata = r.Metadata != null ? JsonDocument.Parse(r.Metadata) : null,
        }).ToList();

        return new GetNotificationsOut
        {
            Total = rows.FirstOrDefault()?.TotalRows ?? 0,
            Page = data.Page,
            PageSize = data.PageSize,
            Items = items,
        };
    }
}
