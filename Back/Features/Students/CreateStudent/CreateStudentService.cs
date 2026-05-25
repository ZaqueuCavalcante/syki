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

        var user = new SykiUser(ctx.RequestUser.InstitutionId, data.Name, email);
        var student = new SykiStudent(user, ctx.RequestUser.InstitutionId, data.Name);
        ctx.Add(student);

        await userManager.CreateAsync(user, $"Syki@{Guid.NewGuid()}");

        return new CreateStudentOut { Id = student.Id };
    }
}
