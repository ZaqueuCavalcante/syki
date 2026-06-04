namespace Syki.Back.Features.Identity.GetSsoConfiguration;

public class GetSsoConfigurationOut : IApiDto<GetSsoConfigurationOut>
{
    public Guid Id { get; set; }
    public SsoProviderType ProviderType { get; set; }
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public bool IsActive { get; set; }
    public bool RequireSso { get; set; }
    public DateTime CreatedAt { get; set; }

    public static IEnumerable<(string, GetSsoConfigurationOut)> GetExamples() =>
    [
        ("Exemplo", new GetSsoConfigurationOut
        {
            Id = Guid.NewGuid(),
            ProviderType = SsoProviderType.AzureAd,
            Authority = "https://login.microsoftonline.com/tenant-id/v2.0",
            ClientId = "00000000-0000-0000-0000-000000000000",
            IsActive = true,
            RequireSso = false,
            CreatedAt = DateTime.UtcNow,
        }),
    ];
}
