namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetUserAccount = nameof(GetUserAccount);
    public const string UpdateUserAccount = nameof(UpdateUserAccount);

    public const string GetUserFeedbacks = nameof(GetUserFeedbacks);
    public const string CreateUserFeedback = nameof(CreateUserFeedback);

    public static AuthorizationBuilder AddUsersPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetUserAccount)
            .AddSykiPolicy(UpdateUserAccount);

        builder
            .AddSykiPolicy(GetUserFeedbacks)
            .AddSykiPolicy(CreateUserFeedback);

        return builder;
    }
}
