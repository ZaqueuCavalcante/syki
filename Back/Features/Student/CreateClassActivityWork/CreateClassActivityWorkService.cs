namespace Syki.Back.Features.Student.CreateClassActivityWork;

public class CreateClassActivityWorkService(SykiDbContext ctx) : IStudentService
{
    public async Task<OneOf<ClassActivityWorkOut, SykiError>> Create(Guid classActivityId, Guid userId, CreateClassActivityWorkIn data)
    {
        var classActivity = await ctx.ClassActivities.AsNoTracking()
            .Where(x => x.Id == classActivityId)
            .FirstOrDefaultAsync();
        if (classActivity == null) return new ClassActivityNotFound();

        var classesIds = await ctx.ClassesStudents
            .Where(x => x.SykiStudentId == userId)
            .Select(x => x.ClassId).ToListAsync();
        if (!classesIds.Contains(classActivity.ClassId)) return new ClassActivityNotFound();

        var classActivityWork = await ctx.ClassActivityWorks
            .Where(x => x.ClassActivityId == classActivityId && x.SykiStudentId == userId)
            .FirstAsync();
        
        classActivityWork.AddLink(data.Link);

        await ctx.SaveChangesAsync();

        return classActivityWork.ToOut();
    }
}
