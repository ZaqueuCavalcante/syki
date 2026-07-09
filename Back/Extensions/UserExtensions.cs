using System.Text.Json;
using Estud.Back.Auth.Claims;
using System.Security.Claims;

namespace Estud.Back.Extensions;

public static class UserExtensions
{
    extension(ClaimsPrincipal user)
    {
        public int Id => int.Parse(user.FindFirst(EstudClaims.UserId)?.Value ?? "0");
        public int RoleId => int.Parse(user.FindFirst(EstudClaims.UserRole)?.Value ?? "0");
        public int InstitutionId => int.Parse(user.FindFirst(EstudClaims.UserInstitutionId)?.Value ?? "0");
        public UserType Type => (UserType)int.Parse(user.FindFirst(EstudClaims.UserType)?.Value ?? "0");
        public bool IsAuthenticated => (user.Identity?.IsAuthenticated ?? false) && user.FindFirst(EstudClaims.UserId)?.Value != null;
        public List<int> Permissions => JsonSerializer.Deserialize<List<int>>(user.FindFirst(EstudClaims.UserPermissions)?.Value ?? "[]") ?? [];
    }
}
