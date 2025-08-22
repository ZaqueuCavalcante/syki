namespace Syki.Back.Features.Academic.GetClassroomAgenda;

public class GetClassroomAgendaService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<List<AgendaDayOut>, SykiError>> Get(Guid institution, Guid classroomId)
    {
        var classroom = await ctx.Classrooms.AsNoTracking().FirstOrDefaultAsync(x => x.InstitutionId == institution && x.Id == classroomId);
        if (classroom == null) return new ClassroomNotFound();

        var ids = await ctx.ClassroomsClasses.Where(c => c.ClassroomId == classroomId && c.IsActive).Select(x => x.ClassId).ToListAsync() ?? [];
        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Schedules)
            .Where(t => t.InstitutionId == institution && ids.Contains(t.Id))
            .ToListAsync();

        var response = classes.ConvertAll(t =>
        {
            return new EnrollmentClassOut
            {
                Id = t.Id,
                Discipline = t.Discipline.Name,
                Schedules = t.Schedules.ConvertAll(h => h.ToOut()),
            };
        });

        var agenda = response.ToAgendas();
        var result = new List<AgendaDayOut>();

        foreach (var day in Enum.GetValues<Day>())
        {
            if (day == Day.Saturday) continue;
            var item = agenda.FirstOrDefault(x => x.Day == day) ?? new AgendaDayOut() { Day = day };
            result.Add(item);
        }

        return result;
    }
}
