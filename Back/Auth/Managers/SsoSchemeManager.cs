using Syki.Back.Auth.Schemes;
using Syki.Back.Domain.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Syki.Back.Auth.Managers;

public class SsoSchemeManager(
    SsoEncryptionManager encryption,
    IAuthenticationSchemeProvider schemeProvider,
    IOptionsMonitorCache<OpenIdConnectOptions> optionsCache,
    IEnumerable<IPostConfigureOptions<OpenIdConnectOptions>> postConfigureOptions)
{
    private readonly ConcurrentDictionary<string, DateTime> _schemeTimestamps = new();

    public void RegisterScheme(SsoConfiguration config)
    {
        RemoveScheme(config.PublicId);
        var schemeName = $"{SsoOidcScheme.Prefix}{config.PublicId}";

        config.ClientSecret = encryption.Decrypt(config.ClientSecret);

        var options = new OpenIdConnectOptions();
        SsoOidcScheme.ConfigureSsoSchemeOptions(options, config);

        foreach (var postConfigure in postConfigureOptions)
        {
            postConfigure.PostConfigure(schemeName, options);
        }

        // Options must be cached BEFORE the scheme is registered.
        // Otherwise, concurrent requests can trigger the auth middleware to resolve
        // default (empty) options via the factory, which TryAdd won't overwrite.
        optionsCache.TryAdd(schemeName, options);
        schemeProvider.AddScheme(new AuthenticationScheme(schemeName, schemeName, typeof(OpenIdConnectHandler)));
        _schemeTimestamps[schemeName] = config.UpdatedAt;
    }

    public void RemoveScheme(Guid configExternalId)
    {
        var schemeName = $"{SsoOidcScheme.Prefix}{configExternalId}";
        schemeProvider.RemoveScheme(schemeName);
        optionsCache.TryRemove(schemeName);
        _schemeTimestamps.TryRemove(schemeName, out _);
    }

    public void UpdateScheme(SsoConfiguration config)
    {
        RemoveScheme(config.PublicId);
        RegisterScheme(config);
    }

    public bool IsStale(string schemeName, DateTime dbUpdatedAt)
    {
        return !_schemeTimestamps.TryGetValue(schemeName, out var cached) || cached < dbUpdatedAt;
    }
}
