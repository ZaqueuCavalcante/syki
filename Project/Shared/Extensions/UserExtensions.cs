using Newtonsoft.Json;
using System.Security.Claims;

namespace Exato.Shared.Extensions;

public static class UserExtensions
{
    extension(ClaimsPrincipal user)
    {
        public Guid Id => Guid.Parse(user.FindFirst(Claims.UserId)?.Value ?? Guid.Empty.ToString());
        public int OrganizationId => int.Parse(user.FindFirst(Claims.OrganizationId)?.Value ?? "0");
        public bool IsAuthenticated => (user.Identity?.IsAuthenticated ?? false) && user.FindFirst(Claims.OrganizationId)?.Value != null;
        public List<int> Features => JsonConvert.DeserializeObject<List<int>>(user.FindFirst(Claims.UserFeatures)?.Value ?? "[]") ?? [];
    }
}
