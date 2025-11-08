using Exato.Front.Auth;
using Exato.Shared.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace Exato.Front.Configs;

public static class AuthConfigs
{
    public static void AddAuthConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddAuthorizationCore(options => options
            .AddCrossPolicies()
            .AddOfficePolicies()
            .AddOrgsPolicies());

		var all = new List<PolicyMetadata>();
		all.AddRange(Policies.Cross);
		all.AddRange(Policies.Office);
		all.AddRange(Policies.Orgs);
		if (!all.Select(x => x.Name).IsAllDistinct()) throw new Exception("Duplicated frontend policies!");

        builder.Services.AddScoped<AuthManager>();
        builder.Services.AddScoped<ExatoAuthStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ExatoAuthStateProvider>());
    }
}
