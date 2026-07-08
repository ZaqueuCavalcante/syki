namespace Syki.Back.Features.Notifications.GetInstitutionNotifications;

public class GetInstitutionNotificationsService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<GetInstitutionNotificationsOut, SykiError>> Get(GetInstitutionNotificationsIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var query = ctx.Notifications.Where(n => n.InstitutionId == institutionId && n.NotificationType == NotificationType.Custom);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((data.Page - 1) * data.PageSize)
            .Take(data.PageSize)
            .Select(n => new GetInstitutionNotificationsItemOut
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                CreatedAt = n.CreatedAt,
            })
            .ToListAsync();

        return new GetInstitutionNotificationsOut
        {
            Total = total,
            Page = data.Page,
            PageSize = data.PageSize,
            Items = items,
        };
    }
}
