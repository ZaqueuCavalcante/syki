using Syki.Shared;
using System.Security.Claims;

namespace Syki.Front.Services;

public interface IAuthService
{
    Task<LoginOut> Login(string email, string password);
    Task<LoginOut> LoginMfa(string code);
    Task<ClaimsPrincipal> GetUser();
    Task Logout();
}
