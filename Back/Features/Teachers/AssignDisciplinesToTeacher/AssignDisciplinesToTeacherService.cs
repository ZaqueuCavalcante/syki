namespace Estud.Back.Features.Teachers.AssignDisciplinesToTeacher;

public class AssignDisciplinesToTeacherService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Assign(int id, AssignDisciplinesToTeacherIn data)
    {
        var teacher = await ctx.Teachers.Include(t => t.Disciplines)
            .FirstOrDefaultAsync(t => t.InstitutionId == ctx.RequestUser.InstitutionId && t.Id == id);
        if (teacher == null) return TeacherNotFound.I;

        var disciplines = await ctx.Disciplines
            .Where(d => d.InstitutionId == ctx.RequestUser.InstitutionId && data.Disciplines.Contains(d.Id))
            .ToListAsync();

        if (disciplines.Count != data.Disciplines.Count) return InvalidDisciplinesList.I;

        teacher.Disciplines = disciplines;
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
