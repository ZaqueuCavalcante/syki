using Exato.Shared.Auth;

namespace Exato.Front.Auth;

public static partial class Policies
{
	public static List<PolicyMetadata> Cross = [];

	public const string ViewCrossUserAccountPage = nameof(ViewCrossUserAccountPage);
	public const string ViewCrossSetupTwoFactorAuthenticationPage = nameof(ViewCrossSetupTwoFactorAuthenticationPage);

	public static AuthorizationOptions AddCrossPolicy(this AuthorizationOptions options, string name)
	{
		Cross.Add(new() { Name = name });

		options.AddPolicy(name, policy => policy.RequireAuthenticatedUser());

		return options;
	}

	public static AuthorizationOptions AddCrossPolicies(this AuthorizationOptions options)
	{
		options
			.AddCrossPolicy(ViewCrossUserAccountPage)
			.AddCrossPolicy(ViewCrossSetupTwoFactorAuthenticationPage);

		return options;
	}
}
