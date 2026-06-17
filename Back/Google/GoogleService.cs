using Google.Apis.Auth;

namespace Syki.Back.Google;

public class GoogleService : IGoogleService
{
    public async Task<GoogleIdTokenPayload?> ValidateIdTokenAsync(string credential, string expectedAudience)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [expectedAudience],
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

            return new GoogleIdTokenPayload
            {
                Email = payload.Email,
                EmailVerified = payload.EmailVerified,
                Subject = payload.Subject,
                Name = payload.Name,
            };
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }
}
