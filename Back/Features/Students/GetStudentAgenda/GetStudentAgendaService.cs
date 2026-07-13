namespace Estud.Back.Features.Students.GetStudentAgenda;

public class GetStudentAgendaService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetStudentAgendaOut> Get()
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var studentId = await ctx.Students.Where(x => x.UserId == userId && x.InstitutionId == institutionId)
            .Select(x => x.Id).FirstOrDefaultAsync();

        var ids = await ctx.ClassStudents.Where(x => x.StudentId == studentId && x.Status == StudentClassStatus.Matriculado)
            .Select(x => x.ClassId).ToListAsync();

        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Schedules)
            .Where(t => t.InstitutionId == institutionId && ids.Contains(t.Id))
            .ToListAsync();

        // Pra cada dia, pegar as aulas que acontecem nesse dia, ordenadas pelo horário de início
        var days = classes.Select(x => x.Schedules.Select(s => s.Day)).SelectMany(x => x).Distinct().OrderBy(x => x).ToList();
        var agenda = new List<GetStudentAgendaItemOut>();

        foreach (var day in days)
        {
            var dayClasses = classes.Where(c => c.Schedules.Any(s => s.Day == day)).ToList();
            var disciplines = dayClasses.SelectMany(c => c.Schedules.Where(s => s.Day == day).Select(s => new GetStudentAgendaItemDisciplineOut
            {
                ClassId = c.Id,
                Name = c.Discipline.Name,
                Start = s.Start,
                End = s.End
            })).OrderBy(d => d.Start).ToList();

            agenda.Add(new GetStudentAgendaItemOut
            {
                Day = day,
                Disciplines = disciplines
            });
        }

        return new GetStudentAgendaOut { Days = agenda };
    }
}
