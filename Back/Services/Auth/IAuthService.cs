using Syki.Shared;

namespace Syki.Back.Services;

public interface IAuthService
{
    Task Register(RegisterIn body);
    Task<string> GetMfaKey(Guid userId);
    Task<bool> SetupMfa(Guid userId, string token);
    Task<string> GenerateAccessToken(string email);
}
