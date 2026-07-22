using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Teachers;

namespace Estud.Back.Features.Teachers.CreateTeacher;

public class CreateTeacherService(EstudDbContext ctx, UserManager<EstudUser> userManager) : IEstudService
{
    public async Task<OneOf<CreateTeacherOut, EstudError>> Create(CreateTeacherIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var email = data.Email.ToLowerInvariant();
        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == email);
        if (emailUsed) return EmailAlreadyUsed.I;

        var user = new EstudUser(institutionId, data.Name, email);
        var teacher = new EstudTeacher(user, institutionId, data.Name);

        var institution = await ctx.Institutions.FindAsync(institutionId);
        var teacherRole = await ctx.Roles.Where(x => x.InstitutionId == institutionId && x.BaseType == UserType.Teacher).FirstAsync();
        var userRole = new EstudUserRole(institution, user, teacherRole.Id);
        ctx.AddRange(teacher, userRole);

        await userManager.CreateAsync(user, $"Estud@{Guid.NewGuid()}");

        return new CreateTeacherOut { Id = teacher.Id };
    }
}
