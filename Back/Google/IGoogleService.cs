namespace Estud.Back.Google;

public interface IGoogleService
{
    Task<GoogleIdTokenPayload?> ValidateIdTokenAsync(string? credential, string expectedAudience);
}
