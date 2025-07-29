namespace Syki.Back.Features.Academic.AssignDisciplinesToTeacher;

public class AssignDisciplinesToTeacherService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Assign(Guid institutionId, Guid teacherId, AssignDisciplinesToTeacherIn data)
    {
        var teacher = await ctx.Teachers.Include(x => x.Disciplines).FirstOrDefaultAsync(p => p.InstitutionId == institutionId && p.Id == teacherId);
        if (teacher == null) return new TeacherNotFound();

        var disciplines = await ctx.Disciplines
            .Where(x => data.Disciplines.Contains(x.Id))
            .ToListAsync();

        if (disciplines.Count != data.Disciplines.Count)
            return new InvalidDisciplinesList();

        teacher.Disciplines = disciplines;

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
