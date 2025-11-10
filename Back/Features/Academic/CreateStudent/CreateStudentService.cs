using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Academic.CreateStudent;

public class CreateStudentService(SykiDbContext ctx, CreateUserService createService, HybridCache cache) : IAcademicService
{
    public async Task<OneOf<StudentOut, SykiError>> Create(Guid institutionId, CreateStudentIn data)
    {
        var courseOfferingExists = await ctx.CourseOfferings
            .AnyAsync(o => o.InstitutionId == institutionId && o.Id == data.CourseOfferingId);
        if (!courseOfferingExists) return new CourseOfferingNotFound();

        var userIn = CreateUserIn.NewStudent(institutionId, data.Name, data.Email, data.PhoneNumber, data.BirthDate);
        var result = await createService.Create(userIn);

        if (result.IsError) return result.Error;

        var userOut = result.Success;
        var user = await ctx.Users.FindAsync(userOut.Id);

        var student = new SykiStudent(user.Id, institutionId, data.Name, data.CourseOfferingId, data.BirthDate)
            {
                User = user,
            };

        ctx.Add(student);

        await ctx.SaveChangesAsync();

        await cache.RemoveAsync($"students:{institutionId}");

        return student.ToOut();
    }
}
