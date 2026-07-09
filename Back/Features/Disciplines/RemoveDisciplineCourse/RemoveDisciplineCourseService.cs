namespace Estud.Back.Features.Disciplines.RemoveDisciplineCourse;

public class RemoveDisciplineCourseService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Remove(RemoveDisciplineCourseIn data)
    {
        var disciplineOk = await ctx.Disciplines.AnyAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == data.DisciplineId);
        if (!disciplineOk) return DisciplineNotFound.I;

        var link = await ctx.CoursesDisciplines.FirstOrDefaultAsync(l => l.DisciplineId == data.DisciplineId && l.CourseId == data.CourseId);
        if (link == null) return CourseDisciplineNotFound.I;

        ctx.Remove(link);
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
