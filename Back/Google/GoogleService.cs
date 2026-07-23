using Google.Apis.Auth;

namespace Estud.Back.Google;

public class GoogleService : IGoogleService
{
    public async Task<GoogleIdTokenPayload?> ValidateIdTokenAsync(string? credential, string expectedAudience)
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
                Name = payload.Name,
                Email = payload.Email,
                Subject = payload.Subject,
                EmailVerified = payload.EmailVerified,
            };
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }
}
