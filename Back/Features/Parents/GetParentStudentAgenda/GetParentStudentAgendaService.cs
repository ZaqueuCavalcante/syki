namespace Estud.Back.Features.Parents.GetParentStudentAgenda;

public class GetParentStudentAgendaService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetParentStudentAgendaOut, EstudError>> Get(int studentId)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var parentId = await ctx.GetParentId(institutionId, userId);

        var hasActiveLink = await ctx.ParentStudents.AnyAsync(x =>
            x.ParentId == parentId &&
            x.StudentId == studentId &&
            x.Status == ParentStudentStatus.Active &&
            !x.RevokedByStudent);
        if (!hasActiveLink) return StudentNotFound.I;

        var ids = await ctx.ClassStudents.Where(x => x.StudentId == studentId && x.Status == StudentClassStatus.Matriculado)
            .Select(x => x.ClassId).ToListAsync();

        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Schedules).ThenInclude(s => s.Classroom)
            .Where(t => t.InstitutionId == institutionId && ids.Contains(t.Id))
            .ToListAsync();

        // Pra cada dia, pegar as aulas que acontecem nesse dia, ordenadas pelo horário de início
        var days = classes.Select(x => x.Schedules.Select(s => s.Day)).SelectMany(x => x).Distinct().OrderBy(x => x).ToList();
        var agenda = new List<GetParentStudentAgendaItemOut>();

        foreach (var day in days)
        {
            var dayClasses = classes.Where(c => c.Schedules.Any(s => s.Day == day)).ToList();
            var disciplines = dayClasses.SelectMany(c => c.Schedules.Where(s => s.Day == day).Select(s => new GetParentStudentAgendaItemDisciplineOut
            {
                ClassId = c.Id,
                Name = c.Discipline.Name,
                ClassroomName = s.Classroom != null ? s.Classroom.Name : null,
                Start = s.Start,
                End = s.End
            })).OrderBy(d => d.Start).ToList();

            agenda.Add(new GetParentStudentAgendaItemOut
            {
                Day = day,
                Disciplines = disciplines
            });
        }

        return new GetParentStudentAgendaOut { Days = agenda };
    }
}
