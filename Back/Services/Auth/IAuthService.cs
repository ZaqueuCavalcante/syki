using Syki.Shared;
using Syki.Shared.CreateUser;

namespace Syki.Back.Services;

public interface IAuthService
{
    Task<CreateUserOut> RegisterUser(CreateUserIn body);
    Task<CreateUserOut> Register(CreateUserIn body);
    Task<string> GetMfaKey(Guid userId);
    Task<MfaSetupOut> SetupMfa(Guid userId, string token);
    Task<string> GenerateAccessToken(string email);
    Task<ResetPasswordTokenOut> GetResetPasswordToken(Guid userId);
    Task<ResetPasswordOut> ResetPassword(ResetPasswordIn body);
    Task<List<CreateUserOut>> GetAllUsers();
}
