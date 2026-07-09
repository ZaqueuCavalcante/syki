using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

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
            .AddEstudPolicy(CreateNotification, UserType.Manager, EstudPermissions.ManageNotifications)
            .AddEstudPolicy(GetInstitutionNotifications, UserType.Manager, EstudPermissions.ManageNotifications);

        builder
            .AddEstudPolicy(GetNotifications)
            .AddEstudPolicy(MarkNotificationsAsViewed)
            .AddEstudPolicy(GetUnreadNotificationsCount);

        return builder;
    }
}
