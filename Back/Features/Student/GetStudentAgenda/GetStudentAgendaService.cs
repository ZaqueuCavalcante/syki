namespace Syki.Back.Features.Student.GetStudentAgenda;

public class GetStudentAgendaService(SykiDbContext ctx)
{
    public async Task<List<AgendaDayOut>> Get(Guid institution, Guid userId)
    {
        var ids = await ctx.ClassesStudents.AsNoTracking()
            .Where(x => x.StudentId == userId && x.StudentDisciplineStatus == StudentDisciplineStatus.Matriculado)
            .Select(x => x.ClassId)
            .ToListAsync();

        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Schedules)
            .Where(t => t.InstitutionId == institution && ids.Contains(t.Id))
            .ToListAsync();

        var response = classes.ConvertAll(t =>
        {
            return new EnrollmentClassOut
            {
                Discipline = t.Discipline.Name,
                Schedules = t.Schedules.ConvertAll(h => h.ToOut()),
            };
        });
        
        return response.ToAgendas();
    }
}
