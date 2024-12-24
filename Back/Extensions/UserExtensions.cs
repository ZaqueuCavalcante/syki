using System.Security.Claims;
using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Extensions;

public static class UserExtensions
{
    public static Guid InstitutionId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("institution")!);
    }

    public static Guid Id(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("sub")!);
    }

    public static Guid CourseCurriculumId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("CourseCurriculumId")!);
    }

    public static bool IsAuthenticated(this ClaimsPrincipal user)
    {
        return user.Identity.IsAuthenticated && user.FindFirstValue("institution") != null;
    }

    public static async Task<bool> IsOnlyInRole(this UserManager<SykiUser> userManager, SykiUser user, UserRole role)
    {
        var adm = await userManager.IsInRoleAsync(user!, UserRole.Adm.ToString());
        var student = await userManager.IsInRoleAsync(user!, UserRole.Student.ToString());
        var teacher = await userManager.IsInRoleAsync(user!, UserRole.Teacher.ToString());
        var academic = await userManager.IsInRoleAsync(user!, UserRole.Academic.ToString());

        return role switch
        {
            UserRole.Academic => academic && !(adm || student || teacher),
            UserRole.Student => student && !(adm || academic || teacher),
            UserRole.Teacher => teacher && !(adm || student || academic),
            UserRole.Adm => adm && !(student || teacher || academic),
            _ => false
        };
    }
}
