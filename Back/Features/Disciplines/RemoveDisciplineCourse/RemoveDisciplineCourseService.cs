namespace Syki.Back.Features.Disciplines.RemoveDisciplineCourse;

public class RemoveDisciplineCourseService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Remove(RemoveDisciplineCourseIn data)
    {
        var disciplineOk = await ctx.Disciplines.AnyAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == data.DisciplineId);
        if (!disciplineOk) return DisciplineNotFound.I;

        var link = await ctx.CoursesDisciplines.FirstOrDefaultAsync(l => l.DisciplineId == data.DisciplineId && l.CourseId == data.CourseId);
        if (link == null) return CourseDisciplineNotFound.I;

        ctx.Remove(link);
        await ctx.SaveChangesAsync();

        return SykiSuccess.I;
    }
}
