namespace Estud.Back.Features.Identity.CheckSocialLoginAvailability;

public class CheckSocialLoginAvailabilityService(SocialLoginSettings settings) : IEstudService
{
    public CheckSocialLoginAvailabilityOut Get()
    {
        return new CheckSocialLoginAvailabilityOut
        {
            GoogleEnabled = settings.Google.Enabled,
            GoogleClientId = settings.Google.Enabled ? settings.Google.ClientId : null,
        };
    }
}
