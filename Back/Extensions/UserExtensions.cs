using System.Security.Claims;

namespace Syki.Back.Extensions;

public static class UserExtensions
{
    extension(ClaimsPrincipal user)
    {
        public Guid Id => Guid.Parse(user.FindFirstValue("sub")!);
        public Guid InstitutionId => Guid.Parse(user.FindFirstValue("institution")!);
        public Guid CourseCurriculumId => Guid.Parse(user.FindFirstValue("CourseCurriculumId")!);
        public bool IsAuthenticated => user.Identity.IsAuthenticated && user.FindFirstValue("institution") != null;
    }
}
