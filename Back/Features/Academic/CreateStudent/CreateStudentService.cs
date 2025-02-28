using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Academic.CreateStudent;

public class CreateStudentService(SykiDbContext ctx, CreateUserService createService, HybridCache cache) : IAcademicService
{
    public async Task<OneOf<StudentOut, SykiError>> Create(Guid institutionId, CreateStudentIn data)
    {
        await using var transaction = await ctx.Database.BeginTransactionAsync();

        var courseOfferingExists = await ctx.CourseOfferings
            .AnyAsync(o => o.InstitutionId == institutionId && o.Id == data.CourseOfferingId);
        if (!courseOfferingExists) return new CourseOfferingNotFound();

        var userIn = CreateUserIn.NewStudent(institutionId, data.Name, data.Email, data.PhoneNumber);
        var result = await createService.Create(userIn);

        if (result.IsError()) return result.GetError();

        var user = result.GetSuccess();
        var student = new SykiStudent(user.Id, institutionId, data.Name, data.CourseOfferingId);
        ctx.Add(student);

        await ctx.SaveChangesAsync();

        await transaction.CommitAsync();

        await cache.RemoveAsync($"students:{institutionId}");

        return student.ToOut();
    }
}
