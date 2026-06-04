namespace Syki.Back.Features.Courses.RemoveCourseDiscipline;

public class RemoveCourseDisciplineService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Remove(RemoveCourseDisciplineIn data)
    {
        var courseOk = await ctx.Courses.AnyAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == data.CourseId);
        if (!courseOk) return CourseNotFound.I;

        var link = await ctx.CoursesDisciplines.FirstOrDefaultAsync(l => l.CourseId == data.CourseId && l.DisciplineId == data.DisciplineId);
        if (link == null) return CourseDisciplineNotFound.I;

        ctx.Remove(link);
        await ctx.SaveChangesAsync();

        return SykiSuccess.I;
    }
}
