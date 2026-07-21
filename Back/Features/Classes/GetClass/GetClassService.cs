namespace Estud.Back.Features.Classes.GetClass;

public class GetClassService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetClassOut, EstudError>> Get(int id)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var @class = await ctx.Classes.AsNoTracking()
            .Include(c => c.Discipline)
            .Include(c => c.Teachers)
            .Include(c => c.Period)
            .Include(c => c.Campus)
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (@class.Status == ClassStatus.OnEnrollment)
        {
            var hasCurrentEnrollmentPeriod = await ctx.EnrollmentPeriods.AsNoTracking()
                .AnyAsync(p => p.InstitutionId == institutionId && p.StartAt <= today && today <= p.EndAt);
            if (!hasCurrentEnrollmentPeriod) @class.Status = ClassStatus.AwaitingStart;
        }

        var classroomIds = @class.Schedules
            .Where(s => s.ClassroomId != null)
            .Select(s => s.ClassroomId!.Value)
            .Distinct()
            .ToList();
        var classroomNames = await ctx.Classrooms.AsNoTracking()
            .Where(c => classroomIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, c => c.Name);

        var classStudents = await ctx.ClassStudents.AsNoTracking()
            .Where(cs => cs.ClassId == id)
            .OrderBy(cs => cs.Student!.Name)
            .Select(cs => new
            {
                cs.Student!.Id,
                cs.Student.Name,
                cs.Status,
            })
            .ToListAsync();

        // Mock: nota e frequência médias aleatórias, porém estáveis por aluno (seed = Id).
        // TODO: calcular a partir das notas e presenças reais do aluno na turma.
        var students = classStudents
            .Select(s =>
            {
                var random = new Random(s.Id);
                return new GetClassStudentOut
                {
                    Id = s.Id,
                    Name = s.Name,
                    Status = s.Status,
                    AverageGrade = Math.Round((decimal)(random.NextDouble() * 10), 1),
                    AverageAttendance = Math.Round((decimal)(random.NextDouble() * 100), 1),
                };
            })
            .ToList();

        return new GetClassOut
        {
            Id = @class.Id,
            DisciplineId = @class.DisciplineId,
            Discipline = @class.Discipline?.Name ?? "",
            Period = @class.Period?.Name ?? "",
            CampusId = @class.CampusId,
            Campus = @class.Campus?.Name,
            Vacancies = @class.Vacancies,
            Workload = @class.Workload,
            Status = @class.Status,
            Teachers = @class.Teachers
                .OrderBy(t => t.Name)
                .Select(t => new GetClassTeacherOut { Id = t.Id, Name = t.Name })
                .ToList(),
            Schedules = @class.Schedules
                .OrderBy(s => s.Day).ThenBy(s => s.Start)
                .Select(s => new GetClassScheduleOut(s.Day, s.Start, s.End)
                {
                    TeacherId = s.TeacherId,
                    Teacher = s.TeacherId == null ? null : @class.Teachers.FirstOrDefault(t => t.Id == s.TeacherId)?.Name,
                    ClassroomId = s.ClassroomId,
                    Classroom = s.ClassroomId != null && classroomNames.TryGetValue(s.ClassroomId.Value, out var name) ? name : null,
                })
                .ToList(),
            Students = students,
        };
    }
}
