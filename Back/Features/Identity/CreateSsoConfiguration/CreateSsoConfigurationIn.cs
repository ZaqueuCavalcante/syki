using Syki.Back.Domain.Enums;

namespace Syki.Back.Features.Identity.CreateSsoConfiguration;

public class CreateSsoConfigurationIn : IApiDto<CreateSsoConfigurationIn>
{
    public SsoProviderType ProviderType { get; set; }
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    public bool RequireSso { get; set; }

    public static IEnumerable<(string Name, CreateSsoConfigurationIn Value)> GetExamples() =>
    [
        ("Azure AD",
        new CreateSsoConfigurationIn
        {
            ProviderType = SsoProviderType.AzureAd,
            Authority = "https://login.microsoftonline.com/tenant-id/v2.0",
            ClientId = "00000000-0000-0000-0000-000000000000",
            ClientSecret = "client-secret-value",
            RequireSso = false,
        }),
    ];
}
