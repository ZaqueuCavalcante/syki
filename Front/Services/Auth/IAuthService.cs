using Syki.Shared.Login;
using Syki.Shared.LoginMfa;
using System.Security.Claims;

namespace Syki.Front.Services;

public interface IAuthService
{
    Task<LoginOut> Login(string email, string password);
    Task<LoginMfaOut> LoginMfa(string code);
    Task<ClaimsPrincipal> GetUser();
    Task Logout();
}
