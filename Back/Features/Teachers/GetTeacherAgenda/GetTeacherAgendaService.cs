namespace Estud.Back.Features.Teachers.GetTeacherAgenda;

public class GetTeacherAgendaService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetTeacherAgendaOut> Get()
    {
        var teacherId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;

        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Schedules)
            .Where(t => t.InstitutionId == institutionId && t.TeacherId == teacherId && t.Status == ClassStatus.Started)
            .ToListAsync();

        // Pra cada dia, pegar as aulas que acontecem nesse dia, ordenadas pelo horário de início
        var days = classes.Select(x => x.Schedules.Select(s => s.Day)).SelectMany(x => x).Distinct().OrderBy(x => x).ToList();
        var agenda = new List<GetTeacherAgendaItemOut>();

        foreach (var day in days)
        {
            var dayClasses = classes.Where(c => c.Schedules.Any(s => s.Day == day)).ToList();
            var disciplines = dayClasses.SelectMany(c => c.Schedules.Where(s => s.Day == day).Select(s => new GetTeacherAgendaItemDisciplineOut
            {
                ClassId = c.Id,
                Name = c.Discipline.Name,
                Start = s.Start,
                End = s.End
            })).OrderBy(d => d.Start).ToList();

            agenda.Add(new GetTeacherAgendaItemOut
            {
                Day = day,
                Disciplines = disciplines
            });
        }

        return new GetTeacherAgendaOut { Days = agenda };
    }
}
