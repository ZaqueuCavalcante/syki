namespace Estud.Back.Features.Students.GetStudentClass;

public class GetStudentClassService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetStudentClassOut, EstudError>> Get(int id)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var studentId = await ctx.GetStudentId(institutionId, userId);

        var @class = await ctx.Classes.AsNoTracking()
            .Include(c => c.Discipline)
            .Include(c => c.Teachers)
            .Include(c => c.Period)
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        var classStudent = await ctx.ClassStudents.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ClassId == id && x.StudentId == studentId);
        if (classStudent == null) return StudentNotEnrolledInClass.I;

        return new GetStudentClassOut
        {
            Id = @class.Id,
            Discipline = @class.Discipline?.Name ?? "",
            Period = @class.Period?.Name ?? "",
            Workload = @class.Workload,
            Status = @class.Status,
            MyStatus = classStudent.Status,
            Teachers = @class.Teachers.Select(t => t.Name).Order().ToList(),
            Schedules = @class.Schedules
                .OrderBy(s => s.Day).ThenBy(s => s.Start)
                .Select(s => new ScheduleOut(s.Day, s.Start, s.End))
                .ToList(),
        };
    }
}
