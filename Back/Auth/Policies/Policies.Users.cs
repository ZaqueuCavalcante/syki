namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetAuthStatus = nameof(GetAuthStatus);
    public const string GetUserAccount = nameof(GetUserAccount);
    public const string UpdateUserAccount = nameof(UpdateUserAccount);

    public static AuthorizationBuilder AddUsersPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetAuthStatus)
            .AddEstudPolicy(GetUserAccount)
            .AddEstudPolicy(UpdateUserAccount);

        return builder;
    }
}
