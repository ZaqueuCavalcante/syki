namespace Syki.Back.Features.Identity.CheckSsoAvailability;

public class CheckSsoAvailabilityOut : IApiDto<CheckSsoAvailabilityOut>
{
    /// <summary>
    /// Whether SSO is enabled for this email domain.
    /// </summary>
    public bool SsoEnabled { get; set; }

    /// <summary>
    /// Whether SSO is required (password login is not allowed).
    /// Only relevant when SsoEnabled is true.
    /// </summary>
    public bool SsoRequired { get; set; }

    /// <summary>
    /// The SSO provider type (e.g., AzureAd, GoogleWorkspace).
    /// Only present when SsoEnabled is true.
    /// </summary>
    public SsoProviderType? ProviderType { get; set; }

    public static IEnumerable<(string Name, CheckSsoAvailabilityOut Value)> GetExamples() =>
    [
        ("SSO Enabled",
        new CheckSsoAvailabilityOut
        {
            SsoEnabled = true,
            SsoRequired = false,
            ProviderType = SsoProviderType.AzureAd,
        }),
        ("SSO Required",
        new CheckSsoAvailabilityOut
        {
            SsoEnabled = true,
            SsoRequired = true,
            ProviderType = SsoProviderType.GoogleWorkspace,
        }),
        ("SSO Not Available",
        new CheckSsoAvailabilityOut
        {
            SsoEnabled = false,
            SsoRequired = false,
            ProviderType = null,
        }),
    ];
}
