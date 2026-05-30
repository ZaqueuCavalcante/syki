using System.Text.Json;
using Syki.Back.Auth.Claims;
using System.Security.Claims;

namespace Syki.Back.Extensions;

public static class UserExtensions
{
    extension(ClaimsPrincipal user)
    {
        public int Id => int.Parse(user.FindFirst(SykiClaims.UserId)?.Value ?? "0");
        public int RoleId => int.Parse(user.FindFirst(SykiClaims.UserRole)?.Value ?? "0");
        public int InstitutionId => int.Parse(user.FindFirst(SykiClaims.UserInstitutionId)?.Value ?? "0");
        public UserType Type => (UserType)int.Parse(user.FindFirst(SykiClaims.UserType)?.Value ?? "0");
        public bool IsAuthenticated => (user.Identity?.IsAuthenticated ?? false) && user.FindFirst(SykiClaims.UserId)?.Value != null;
        public List<int> Permissions => JsonSerializer.Deserialize<List<int>>(user.FindFirst(SykiClaims.UserPermissions)?.Value ?? "[]") ?? [];
    }
}
