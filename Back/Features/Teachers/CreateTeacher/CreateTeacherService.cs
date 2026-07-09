using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Teachers;

namespace Estud.Back.Features.Teachers.CreateTeacher;

public class CreateTeacherService(EstudDbContext ctx, UserManager<EstudUser> userManager) : IEstudService
{
    public async Task<OneOf<CreateTeacherOut, EstudError>> Create(CreateTeacherIn data)
    {
        var email = data.Email.ToLowerInvariant();
        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == email);
        if (emailUsed) return EmailAlreadyUsed.I;

        var user = new EstudUser(ctx.RequestUser.InstitutionId, data.Name, email);
        var teacher = new EstudTeacher(user, ctx.RequestUser.InstitutionId, data.Name);

        var institution = await ctx.Institutions.FindAsync(ctx.RequestUser.InstitutionId);
        var teacherRole = await ctx.GetTeacherRole();
        var userRole = new EstudUserRole(institution, user, teacherRole.Id);
        ctx.AddRange(teacher, userRole);

        await userManager.CreateAsync(user, $"Estud@{Guid.NewGuid()}");

        return new CreateTeacherOut { Id = teacher.Id };
    }
}
