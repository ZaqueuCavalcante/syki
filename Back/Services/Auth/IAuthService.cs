using Syki.Shared;

namespace Syki.Back.Services;

public interface IAuthService
{
    Task<UserOut> RegisterUser(UserIn body);
    Task<UserOut> Register(UserIn body);
    Task<string> GetMfaKey(Guid userId);
    Task<MfaSetupOut> SetupMfa(Guid userId, string token);
    Task<string> GenerateAccessToken(string email);
    Task<ResetPasswordTokenOut> GetResetPasswordToken(Guid userId);
    Task<ResetPasswordOut> ResetPassword(ResetPasswordIn body);
    Task<List<UserOut>> GetAllUsers();
}
