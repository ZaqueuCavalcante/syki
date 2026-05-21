using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Teachers;

namespace Syki.Back.Features.Teachers.CreateTeacher;

public class CreateTeacherService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ISykiService
{
    public async Task<OneOf<CreateTeacherOut, SykiError>> Create(CreateTeacherIn data)
    {
        var user = new SykiUser(ctx.RequestUser.InstitutionId, data.Name, data.Email);
        var teacher = new SykiTeacher(user, ctx.RequestUser.InstitutionId, data.Name);
        ctx.Add(teacher);

        await userManager.CreateAsync(user, $"Syki@{Guid.NewGuid()}");

        return new CreateTeacherOut { Id = teacher.Id };
    }
}
