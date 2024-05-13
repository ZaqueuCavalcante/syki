namespace Syki.Back.Features.Teacher.GetTeacherAgenda;

public class GetTeacherAgendaService(SykiDbContext ctx)
{
    public async Task<List<AgendaDayOut>> Get(Guid institution, Guid userId)
    {
        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Schedules)
            .Where(t => t.InstitutionId == institution && t.TeacherId == userId)
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
