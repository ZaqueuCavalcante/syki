using Syki.Shared.Auth;

namespace Syki.Back.Auth;

public static partial class Policies
{
	public static List<PolicyMetadata> Cross = [];

	public const string GetUserAccount = nameof(GetUserAccount);
	public const string GetTwoFactorAuthenticationKey = nameof(GetTwoFactorAuthenticationKey);
	public const string SetupTwoFactorAuthentication = nameof(SetupTwoFactorAuthentication);

	public static AuthorizationBuilder AddCrossPolicy(this AuthorizationBuilder builder, string name)
	{
		Cross.Add(new() { Name = name });

		return builder.AddPolicy(name, policy => policy
			.RequireAuthenticatedUser()
			.AddAuthenticationSchemes(AuthenticationConfigs.BearerScheme));
	}

	public static AuthorizationBuilder AddCrossPolicies(this AuthorizationBuilder builder)
	{
		builder
			.AddCrossPolicy(GetUserAccount)
			.AddCrossPolicy(GetTwoFactorAuthenticationKey)
			.AddCrossPolicy(SetupTwoFactorAuthentication);

		return builder;
	}
}
