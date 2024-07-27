using System.Security.Claims;
using Syki.Back.Features.Academic.StartClass;
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

    public static Guid GetCourseCurriculumId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("CourseCurriculumId")!);
    }

    public static async Task<bool> IsOnlyInRole(this UserManager<SykiUser> userManager, SykiUser user, UserRole role)
    {
        var adm = await userManager.IsInRoleAsync(user!, UserRole.Adm.ToString());
        var student = await userManager.IsInRoleAsync(user!, UserRole.Student.ToString());
        var teacher = await userManager.IsInRoleAsync(user!, UserRole.Teacher.ToString());
        var academic = await userManager.IsInRoleAsync(user!, UserRole.Academic.ToString());
        var seller = await userManager.IsInRoleAsync(user!, UserRole.Seller.ToString());

        if (role is UserRole.Academic) return academic && !(adm || student || teacher || seller);

        return false;
    }

    public static bool IsAuditable(this PathString path)
    {
        return
            path != "/login" &&
            path != "/login/mfa" &&
            path != "/reset-password" &&
            path != "/users" &&
            path != "/reset-password-token";
    }

    public static decimal GetAverageNote(this IEnumerable<ExamGrade> examGrades)
    {
        var average = examGrades.Select(x => x.Note).OrderDescending().Take(2).Average();
        return Math.Round(average, 2);
    }
}
