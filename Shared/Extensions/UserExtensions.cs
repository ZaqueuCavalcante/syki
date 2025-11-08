using Newtonsoft.Json;
using System.Security.Claims;

namespace Syki.Shared;

public static class UserExtensions
{
    extension(ClaimsPrincipal user)
    {
        public Guid Id => Guid.Parse(user.FindFirst(Claims.UserId)?.Value ?? Guid.Empty.ToString());
        public Guid InstitutionId => Guid.Parse(user.FindFirst(Claims.InstitutionId)?.Value ?? Guid.Empty.ToString());
        public Guid CourseCurriculumId => Guid.Parse(user.FindFirst(Claims.CourseCurriculumId)?.Value ?? Guid.Empty.ToString());
        public bool IsAuthenticated => (user.Identity?.IsAuthenticated ?? false) && user.FindFirst(Claims.InstitutionId)?.Value != null;
        public List<int> Features => JsonConvert.DeserializeObject<List<int>>(user.FindFirst(Claims.UserFeatures)?.Value ?? "[]") ?? [];
    }
}
