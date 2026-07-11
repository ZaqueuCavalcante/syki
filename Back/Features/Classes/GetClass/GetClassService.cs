namespace Estud.Back.Features.Classes.GetClass;

public class GetClassService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetClassOut, EstudError>> Get(int id)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var @class = await ctx.Classes.AsNoTracking()
            .Include(c => c.Discipline)
            .Include(c => c.Teacher)
            .Include(c => c.Period)
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == id && c.InstitutionId == institutionId);

        if (@class == null) return ClassNotFound.I;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (@class.Status == ClassStatus.OnEnrollment && @class.Period?.EndAt < today)
            @class.Status = ClassStatus.AwaitingStart;

        var students = await (
            from cs in ctx.ClassStudents.AsNoTracking()
            join s in ctx.Students.AsNoTracking() on cs.StudentId equals s.Id
            where cs.ClassId == id
            orderby s.Name
            select new GetClassStudentOut
            {
                Id = s.Id,
                Name = s.Name,
                Status = cs.Status,
            }
        ).ToListAsync();

        return new GetClassOut
        {
            Id = @class.Id,
            Discipline = @class.Discipline?.Name ?? "",
            Teacher = @class.Teacher?.Name ?? "",
            Period = @class.Period?.Name ?? "",
            Vacancies = @class.Vacancies,
            Workload = @class.Workload,
            Status = @class.Status,
            Schedules = @class.Schedules
                .OrderBy(s => s.Day).ThenBy(s => s.Start)
                .Select(s => new ScheduleOut(s.Day, s.Start, s.End))
                .ToList(),
            Students = students,
        };
    }
}
