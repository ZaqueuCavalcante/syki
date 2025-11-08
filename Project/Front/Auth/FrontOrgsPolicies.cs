using Exato.Shared.Auth;

namespace Exato.Front.Auth;

public static partial class Policies
{
	public static List<PolicyMetadata> Orgs = [];

	public const string ViewOrgsReportsPage = nameof(ViewOrgsReportsPage);
	public const string ViewOrgsSettingsPage = nameof(ViewOrgsSettingsPage);

	public static AuthorizationOptions AddOrgsPolicy(this AuthorizationOptions options, string name, params ExatoFeature[] features)
	{
		Office.Add(new() { Name = name, Features = features.ToList() });

		var ids = features.Select(x => x.Id).ToList();

		options.AddPolicy(name, policy => policy
			.RequireAuthenticatedUser()
			.RequireAssertion(x => x.User.Features.Any(f => ids.Contains(f))));

		return options;
	}

    public static AuthorizationOptions AddOrgsPolicies(this AuthorizationOptions options)
    {
		options
			.AddOrgsPolicy(ViewOrgsReportsPage, [])
			.AddOrgsPolicy(ViewOrgsSettingsPage, []);

		return options;
    }
}
