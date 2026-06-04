namespace Syki.Back.Features.Identity.UpdateSsoConfiguration;

public class UpdateSsoConfigurationIn : IApiDto<UpdateSsoConfigurationIn>
{
    public SsoProviderType ProviderType { get; set; }
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public bool IsActive { get; set; }
    public bool RequireSso { get; set; }

    public static IEnumerable<(string Name, UpdateSsoConfigurationIn Value)> GetExamples() =>
    [
        ("Azure AD",
        new UpdateSsoConfigurationIn
        {
            ProviderType = SsoProviderType.AzureAd,
            Authority = "https://login.microsoftonline.com/tenant-id/v2.0",
            ClientId = "00000000-0000-0000-0000-000000000000",
            ClientSecret = null,
            IsActive = true,
            RequireSso = false,
        }),
    ];
}
