namespace Estud.Back.Features.Teachers.GetTeacherAgenda;

public class GetTeacherAgendaService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetTeacherAgendaOut> Get()
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var teacherId = await ctx.GetTeacherId(institutionId, userId);

        var classes = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .Include(x => x.Schedules).ThenInclude(s => s.Classroom)
            .Where(x => x.InstitutionId == institutionId && x.Teachers.Any(x => x.Id == teacherId) && x.Status == ClassStatus.Started)
            .ToListAsync();

        // Pra cada dia, pegar as aulas que acontecem nesse dia, ordenadas pelo horário de início
        var days = classes.Select(x => x.Schedules.Where(s => s.TeacherId == teacherId).Select(s => s.Day)).SelectMany(x => x).Distinct().OrderBy(x => x).ToList();
        var agenda = new List<GetTeacherAgendaItemOut>();

        foreach (var day in days)
        {
            var dayClasses = classes.Where(c => c.Schedules.Any(s => s.Day == day && s.TeacherId == teacherId)).ToList();
            var disciplines = dayClasses.SelectMany(c => c.Schedules.Where(s => s.Day == day && s.TeacherId == teacherId).Select(s => new GetTeacherAgendaItemDisciplineOut
            {
                ClassId = c.Id,
                Name = c.Discipline.Name,
                ClassroomName = s.Classroom != null ? s.Classroom.Name : null,
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
