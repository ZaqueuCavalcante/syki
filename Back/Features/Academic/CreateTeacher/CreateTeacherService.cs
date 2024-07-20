using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Back.Features.Academic.CreateTeacher;

public class CreateTeacherService(SykiDbContext ctx, CreateUserService service, SendResetPasswordTokenService sendService)
{
    public async Task<OneOf<TeacherOut, SykiError>> Create(Guid institutionId, CreateTeacherIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        var userIn = CreateUserIn.NewTeacher(institutionId, data.Name, data.Email);
        var result = await service.Create(userIn);

        return await result.Match<Task<OneOf<TeacherOut, SykiError>>>(
            async user =>
            {
                var teacher = new SykiTeacher(user.Id, institutionId, data.Name);

                ctx.Add(teacher);
                ctx.Add(SykiTask.LinkOldNotifications(user.Id, institutionId));

                await sendService.Send(new(user.Email));

                transaction.Commit();

                return teacher.ToOut();
            },
            error => Task.FromResult<OneOf<TeacherOut, SykiError>>(error)
        );
    }
}
