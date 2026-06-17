namespace Syki.Back.Features.Identity.CheckSsoAvailability;

public class SsoConfigDto
{
    public bool RequireSso { get; set; }
    public SsoProviderType ProviderType { get; set; }
}
