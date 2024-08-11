namespace Syki.Back.Features.Academic.CreateLessons;

public class CreateLessonsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Create(Guid institutionId, Guid classId)
    {
        var @class = await ctx.Classes
            .Include(c => c.Period)
            .Include(t => t.Lessons)
            .Include(t => t.Schedules)
            .Where(c => c.InstitutionId == institutionId && c.Id == classId)
            .FirstOrDefaultAsync();

        if (@class == null) return new ClassNotFound();

        @class.CreateLessons();

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
