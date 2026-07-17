namespace Estud.Back.Features.Teachers.GetTeacherClass;

public class GetTeacherClassService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetTeacherClassOut, EstudError>> Get(int id)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var teacherId = await ctx.GetTeacherId(institutionId, userId);

        var @class = await ctx.Classes.AsNoTracking()
            .Include(c => c.Discipline)
            .Include(c => c.Period)
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        var assigned = await ctx.ClassTeachers.AnyAsync(ct => ct.ClassId == id && ct.TeacherId == teacherId);
        if (!assigned) return TeacherNotAssignedToClass.I;

        var students = await (
            from cs in ctx.ClassStudents.AsNoTracking()
            join s in ctx.Students.AsNoTracking() on cs.StudentId equals s.Id
            where cs.ClassId == id
            orderby s.Name
            select new GetTeacherClassStudentOut
            {
                Id = s.Id,
                Name = s.Name,
                Status = cs.Status,
            }
        ).ToListAsync();

        return new GetTeacherClassOut
        {
            Id = @class.Id,
            Discipline = @class.Discipline?.Name ?? "",
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
