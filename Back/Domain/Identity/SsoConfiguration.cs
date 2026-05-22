using Syki.Back.Domain.Enums;

namespace Syki.Back.Domain.Identity;

/// <summary>
/// SSO configuration for an institution.
/// </summary>
public class SsoConfiguration
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public int InstitutionId { get; set; }

    // Provider
    public SsoProviderType ProviderType { get; set; }
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    // Domains
    public List<SsoAllowedDomain> AllowedDomains { get; set; }

    // Behavior
    public bool IsActive { get; set; }
    public bool RequireSso { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public SsoConfiguration() { }

    public SsoConfiguration(
        int orgId,
        SsoProviderType providerType,
        string authority,
        string clientId,
        string clientSecret,
        List<string> allowedDomains)
    {
        InstitutionId = orgId;
        PublicId = Guid.NewGuid();
        ProviderType = providerType;
        Authority = authority;
        ClientId = clientId;
        ClientSecret = clientSecret;
        IsActive = true;
        RequireSso = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;

        AllowedDomains = allowedDomains
            .Select(d => new SsoAllowedDomain(d))
            .ToList();
    }

    public void Update(
        SsoProviderType providerType,
        string authority,
        string clientId,
        string clientSecret,
        bool isActive,
        bool requireSso)
    {
        ProviderType = providerType;
        Authority = authority;
        ClientId = clientId;
        ClientSecret = clientSecret;
        IsActive = isActive;
        RequireSso = requireSso;
        UpdatedAt = DateTime.UtcNow;
    }
}
