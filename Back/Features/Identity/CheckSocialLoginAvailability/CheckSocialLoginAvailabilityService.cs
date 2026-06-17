namespace Syki.Back.Features.Identity.CheckSocialLoginAvailability;

public class CheckSocialLoginAvailabilityService(SocialLoginSettings settings) : ISykiService
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
