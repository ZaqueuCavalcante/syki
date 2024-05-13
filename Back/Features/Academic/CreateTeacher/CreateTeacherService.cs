using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Back.Features.Academic.CreateTeacher;

public class CreateTeacherService(SykiDbContext ctx, CreateUserService service, SendResetPasswordTokenService sendService)
{
    public async Task<TeacherOut> Create(Guid institutionId, CreateTeacherIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        var userIn = CreateUserIn.NewTeacher(institutionId, data.Name, data.Email);
        var user = await service.Create(userIn);

        var teacher = new Teacher(user.Id, institutionId, data.Name);

        ctx.Add(teacher);
        await ctx.SaveChangesAsync();

        if (!data.Email.EndsWith("syki.demo.com"))
        {
            await sendService.Send(new SendResetPasswordTokenIn { Email = user.Email });
        }

        transaction.Commit();

        return teacher.ToOut();
    }
}
