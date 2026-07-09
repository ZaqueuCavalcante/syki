namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetUserAccount = nameof(GetUserAccount);
    public const string UpdateUserAccount = nameof(UpdateUserAccount);

    public const string GetUserFeedbacks = nameof(GetUserFeedbacks);
    public const string CreateUserFeedback = nameof(CreateUserFeedback);

    public static AuthorizationBuilder AddUsersPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetUserAccount)
            .AddEstudPolicy(UpdateUserAccount);

        builder
            .AddEstudPolicy(GetUserFeedbacks)
            .AddEstudPolicy(CreateUserFeedback);

        return builder;
    }
}
