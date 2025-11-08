using Exato.Shared.Auth;

namespace Exato.Back.Configs;

public static class AuthorizationConfigs
{
    public static void AddAuthorizationConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddCrossPolicies()
            .AddOfficePolicies();

		var all = new List<PolicyMetadata>();
		all.AddRange(Policies.Cross);
		all.AddRange(Policies.Office);
		if (!all.Select(x => x.Name).IsAllDistinct()) throw new Exception("Duplicated backend policies!");

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
        });
    }
}
