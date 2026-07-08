using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetWebhookCalls = nameof(GetWebhookCalls);
    public const string GetWebhookSubscription = nameof(GetWebhookSubscription);
    public const string GetWebhookSubscriptions = nameof(GetWebhookSubscriptions);
    public const string CreateWebhookSubscription = nameof(CreateWebhookSubscription);
    public const string UpdateWebhookSubscription = nameof(UpdateWebhookSubscription);

    public static AuthorizationBuilder AddWebhooksPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetWebhookCalls, UserType.Manager, SykiPermissions.ManageWebhooks)
            .AddSykiPolicy(GetWebhookSubscription, UserType.Manager, SykiPermissions.ManageWebhooks)
            .AddSykiPolicy(GetWebhookSubscriptions, UserType.Manager, SykiPermissions.ManageWebhooks)
            .AddSykiPolicy(CreateWebhookSubscription, UserType.Manager, SykiPermissions.ManageWebhooks)
            .AddSykiPolicy(UpdateWebhookSubscription, UserType.Manager, SykiPermissions.ManageWebhooks);

        return builder;
    }
}
