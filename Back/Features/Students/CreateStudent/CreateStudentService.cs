using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Students;

namespace Syki.Back.Features.Students.CreateStudent;

public class CreateStudentService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ISykiService
{
    public async Task<OneOf<CreateStudentOut, SykiError>> Create(CreateStudentIn data)
    {
        var email = data.Email.ToLowerInvariant();
        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == email);
        if (emailUsed) return EmailAlreadyUsed.I;

        var studentRole = await ctx.GetStudentRole();
        var institutionId = ctx.RequestUser.InstitutionId;

        var user = new SykiUser(institutionId, data.Name, email);
        var student = new SykiStudent(user, institutionId, data.Name);
        var userRole = new SykiUserRole(institutionId, user, studentRole.Id);
        ctx.AddRange(student, userRole);

        await userManager.CreateAsync(user, $"Syki@{Guid.NewGuid()}");

        return new CreateStudentOut { Id = student.Id };
    }
}
