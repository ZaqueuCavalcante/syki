using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string CreateNotification = nameof(CreateNotification);

    public const string GetInstitutionNotifications = nameof(GetInstitutionNotifications);

    public const string GetNotifications = nameof(GetNotifications);
    public const string MarkNotificationsAsViewed = nameof(MarkNotificationsAsViewed);
    public const string GetUnreadNotificationsCount = nameof(GetUnreadNotificationsCount);

    public static AuthorizationBuilder AddNotificationsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(CreateNotification, UserType.Manager, SykiPermissions.ManageNotifications)
            .AddSykiPolicy(GetInstitutionNotifications, UserType.Manager, SykiPermissions.ManageNotifications);

        builder
            .AddSykiPolicy(GetNotifications)
            .AddSykiPolicy(MarkNotificationsAsViewed)
            .AddSykiPolicy(GetUnreadNotificationsCount);

        return builder;
    }
}
