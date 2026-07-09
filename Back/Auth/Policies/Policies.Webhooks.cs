using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

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
            .AddEstudPolicy(GetWebhookCalls, UserType.Manager, EstudPermissions.ManageWebhooks)
            .AddEstudPolicy(GetWebhookSubscription, UserType.Manager, EstudPermissions.ManageWebhooks)
            .AddEstudPolicy(GetWebhookSubscriptions, UserType.Manager, EstudPermissions.ManageWebhooks)
            .AddEstudPolicy(CreateWebhookSubscription, UserType.Manager, EstudPermissions.ManageWebhooks)
            .AddEstudPolicy(UpdateWebhookSubscription, UserType.Manager, EstudPermissions.ManageWebhooks);

        return builder;
    }
}
