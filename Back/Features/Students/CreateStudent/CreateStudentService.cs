using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Students;

namespace Syki.Back.Features.Students.CreateStudent;

public class CreateStudentService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ISykiService
{
    public async Task<OneOf<CreateStudentOut, SykiError>> Create(CreateStudentIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var courseOfferingExists = await ctx.CourseOfferings
            .AnyAsync(o => o.InstitutionId == institutionId && o.Id == data.CourseOfferingId);
        if (!courseOfferingExists) return new CourseOfferingNotFound();

        var user = new SykiUser(institutionId, data.Name, data.Email);
        var student = new SykiStudent(user, institutionId, data.Name, data.CourseOfferingId);
        ctx.Add(student);

        await userManager.CreateAsync(user, $"Syki@{Guid.NewGuid()}");

        return new CreateStudentOut { Id = student.Id };
    }
}
