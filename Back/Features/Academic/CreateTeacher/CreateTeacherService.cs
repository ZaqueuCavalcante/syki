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

        var teacher = new SykiTeacher(user.Id, institutionId, data.Name);
        ctx.Add(teacher);

        ctx.Add(SykiTask.LinkOldNotifications(user.Id));
        await ctx.SaveChangesAsync();

        await sendService.Send(new() { Email = user.Email });

        transaction.Commit();

        return teacher.ToOut();
    }
}
