namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetHomeStats = nameof(GetHomeStats);

    public static AuthorizationBuilder AddCrossPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetHomeStats, UserType.Manager);

        return builder;
    }
}
