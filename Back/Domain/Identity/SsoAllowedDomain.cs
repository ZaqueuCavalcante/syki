namespace Estud.Back.Domain.Identity;

/// <summary>
/// Allowed email domain for SSO configuration. <br/>
/// Each domain can only be linked to one organization's SSO config.
/// </summary>
public class SsoAllowedDomain
{
    /// <summary>
    /// The email domain (e.g., "empresa.com"). <br/>
    /// Must be unique across all SSO configurations.
    /// </summary>
    public string Domain { get; set; }

    public int SsoConfigurationId { get; set; }

    public SsoAllowedDomain() { }

    public SsoAllowedDomain(string domain)
    {
        Domain = domain.ToLowerInvariant();
    }
}
