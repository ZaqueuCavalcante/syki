namespace Estud.Back.Features.Teachers.GetTeacherDetails;

public class GetTeacherDetailsService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetTeacherDetailsOut, EstudError>> Get(int id)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var teacher = await ctx.Teachers.AsNoTracking()
            .Include(t => t.User)
            .Include(t => t.Campi)
            .Include(t => t.Disciplines)
            .FirstOrDefaultAsync(t => t.InstitutionId == institutionId && t.Id == id);
        if (teacher == null) return TeacherNotFound.I;

        var teacherName = teacher.Name;

        var classes = await ctx.Classes.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId && c.Teachers.Any(t => t.Id == id))
            .OrderByDescending(c => c.Period.Name).ThenBy(c => c.Discipline.Name)
            .Select(c => new GetTeacherDetailsClassOut
            {
                Id = c.Id,
                Discipline = c.Discipline.Name,
                Period = c.Period.Name,
                Vacancies = c.Vacancies,
                Students = ctx.ClassStudents.Count(cs => cs.ClassId == c.Id),
                Workload = c.Workload,
                Status = c.Status,
                Schedules = c.Schedules
                    .Where(s => s.TeacherId == id)
                    .OrderBy(s => s.Day).ThenBy(s => s.Start)
                    .Select(s => new GetTeacherDetailsScheduleOut(s.Day, s.Start, s.End)
                    {
                        TeacherId = s.TeacherId,
                        Teacher = teacherName,
                    })
                    .ToList(),
            })
            .ToListAsync();

        // Mesma regra do GetClass: fora de um período de matrícula vigente, uma
        // turma liberada para matrícula é exibida como aguardando início.
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (classes.Any(c => c.Status == ClassStatus.OnEnrollment))
        {
            var hasCurrentEnrollmentPeriod = await ctx.EnrollmentPeriods.AsNoTracking()
                .AnyAsync(p => p.InstitutionId == institutionId && p.StartAt <= today && today <= p.EndAt);

            if (!hasCurrentEnrollmentPeriod)
            {
                foreach (var @class in classes.Where(c => c.Status == ClassStatus.OnEnrollment))
                    @class.Status = ClassStatus.OnReview;
            }
        }

        return new GetTeacherDetailsOut
        {
            Id = teacher.Id,
            Name = teacher.Name,
            Email = teacher.User!.Email!,
            Campi = teacher.Campi
                .OrderBy(c => c.Name)
                .Select(c => new GetTeacherDetailsCampusOut { Id = c.Id, Name = c.Name })
                .ToList(),
            Disciplines = teacher.Disciplines
                .OrderBy(d => d.Name)
                .Select(d => new GetTeacherDetailsDisciplineOut { Id = d.Id, Name = d.Name })
                .ToList(),
            Classes = classes,
        };
    }
}
