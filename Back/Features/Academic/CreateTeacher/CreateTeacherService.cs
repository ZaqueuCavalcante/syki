using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Academic.CreateTeacher;

public class CreateTeacherService(SykiDbContext ctx, CreateUserService service) : IAcademicService
{
    public async Task<OneOf<TeacherOut, SykiError>> Create(Guid institutionId, CreateTeacherIn data)
    {
        await using var transaction = await ctx.Database.BeginTransactionAsync();

        var userIn = CreateUserIn.NewTeacher(institutionId, data.Name, data.Email);
        var result = await service.Create(userIn);

        if (result.IsError()) return result.GetError();

        var user = result.GetSuccess();

        var teacher = new SykiTeacher(user.Id, institutionId, data.Name);
        ctx.Add(teacher);

        await ctx.SaveChangesAsync();

        await transaction.CommitAsync();

        return teacher.ToOut();
    }
}
