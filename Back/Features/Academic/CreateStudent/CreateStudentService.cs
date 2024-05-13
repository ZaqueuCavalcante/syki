using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Back.Features.Academic.CreateStudent;

public class CreateStudentService(SykiDbContext ctx, CreateUserService service, SendResetPasswordTokenService sendService)
{
    public async Task<StudentOut> Create(Guid institutionId, CreateStudentIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        var courseOfferingOk = await ctx.CourseOfferings
            .AnyAsync(o => o.InstitutionId == institutionId && o.Id == data.CourseOfferingId);
        if (!courseOfferingOk)
            Throw.DE012.Now();

        var userIn = CreateUserIn.NewStudent(institutionId, data.Name, data.Email);
        var user = await service.Create(userIn);

        var student = new Student(user.Id, institutionId, data.Name, data.CourseOfferingId);
        ctx.Add(student);
        await ctx.SaveChangesAsync();

        await sendService.Send(new SendResetPasswordTokenIn { Email = user.Email });

        transaction.Commit();

        return student.ToOut();
    }
}
