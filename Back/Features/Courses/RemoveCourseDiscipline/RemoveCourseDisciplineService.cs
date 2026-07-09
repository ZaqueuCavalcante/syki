namespace Estud.Back.Features.Courses.RemoveCourseDiscipline;

public class RemoveCourseDisciplineService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Remove(RemoveCourseDisciplineIn data)
    {
        var courseOk = await ctx.Courses.AnyAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == data.CourseId);
        if (!courseOk) return CourseNotFound.I;

        var link = await ctx.CoursesDisciplines.FirstOrDefaultAsync(l => l.CourseId == data.CourseId && l.DisciplineId == data.DisciplineId);
        if (link == null) return CourseDisciplineNotFound.I;

        ctx.Remove(link);
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
