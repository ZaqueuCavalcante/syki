using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Back.Features.Academic.CreateStudent;

public class CreateStudentService(SykiDbContext ctx, CreateUserService service, SendResetPasswordTokenService sendService)
{
    public async Task<OneOf<StudentOut, SykiError>> Create(Guid institutionId, CreateStudentIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        var courseOfferingOk = await ctx.CourseOfferings
            .AnyAsync(o => o.InstitutionId == institutionId && o.Id == data.CourseOfferingId);
        if (!courseOfferingOk) return new CourseOfferingNotFound();

        var userIn = CreateUserIn.NewStudent(institutionId, data.Name, data.Email);
        var result = await service.Create(userIn);

        return await result.Match<Task<OneOf<StudentOut, SykiError>>>(
            async user =>
            {
                var student = new SykiStudent(user.Id, institutionId, data.Name, data.CourseOfferingId);
                ctx.Add(student);

                ctx.Add(SykiTask.LinkOldNotifications(user.Id));
                await ctx.SaveChangesAsync();

                await sendService.Send(new() { Email = user.Email });

                transaction.Commit();

                return student.ToOut();
            },
            error => Task.FromResult<OneOf<StudentOut, SykiError>>(error)
        );
    }
}
