using Estud.Back.Domain.Notifications;

namespace Estud.Back.Features.Notifications.CreateNotification;

public class CreateNotificationService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<CreateNotificationIn>
    {
        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty().WithError(InvalidNotificationTitle.I);
            RuleFor(x => x.Description).NotEmpty().WithError(InvalidNotificationDescription.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateNotificationOut, EstudError>> Create(CreateNotificationIn data)
    {
        if (V.Run(data, out var error)) return error;

        var institutionId = ctx.RequestUser.InstitutionId;

        var notification = new Notification(institutionId, NotificationType.Custom, data.Title, data.Description);

        if (data.TargetUsers is UsersGroup.All or UsersGroup.Teachers)
        {
            await CreateNotificationFor(institutionId, notification, UserType.Teacher);
        }
        if (data.TargetUsers is UsersGroup.All or UsersGroup.Students)
        {
            await CreateNotificationFor(institutionId, notification, UserType.Student);
        }

        await ctx.SaveChangesAsync(notification);

        return new CreateNotificationOut { Id = notification.Id };
    }

    private async Task CreateNotificationFor(int institutionId, Notification notification, UserType userType)
    {
        FormattableString sql = $@"
            SELECT
                u.id
            FROM
                estud.users u
            INNER JOIN
                estud.roles r ON r.base_type = {userType}
            INNER JOIN
                estud.user_roles ur ON ur.user_id = u.id AND ur.role_id = r.id
            WHERE
                u.institution_id = {institutionId}
        ";

        var usersIds = await ctx.Database.SqlQuery<int>(sql).ToListAsync();

        usersIds.ForEach(userId => ctx.UserNotifications.Add(new UserNotification(userId, notification)));
    }
}
