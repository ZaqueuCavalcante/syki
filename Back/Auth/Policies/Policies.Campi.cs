namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string CreateCampus = nameof(CreateCampus);
    public const string GetCampus = nameof(GetCampus);
    public const string UpdateCampus = nameof(UpdateCampus);

    public static AuthorizationBuilder AddCampiPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(CreateCampus)
            .AddSykiPolicy(GetCampus)
            .AddSykiPolicy(UpdateCampus);

        return builder;
    }
}
