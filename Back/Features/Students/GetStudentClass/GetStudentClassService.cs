namespace Estud.Back.Features.Students.GetStudentClass;

public class GetStudentClassService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetStudentClassOut, EstudError>> Get(int id)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;

        var @class = await ctx.Classes.AsNoTracking()
            .Include(c => c.Discipline)
            .Include(c => c.Teacher)
            .Include(c => c.Period)
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        var studentId = await ctx.Students.AsNoTracking()
            .Where(x => x.UserId == userId && x.InstitutionId == institutionId)
            .Select(x => x.Id).FirstOrDefaultAsync();

        var classStudent = await ctx.ClassStudents.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ClassId == id && x.StudentId == studentId);
        if (classStudent == null) return StudentNotEnrolledInClass.I;

        return new GetStudentClassOut
        {
            Id = @class.Id,
            Discipline = @class.Discipline?.Name ?? "",
            Teacher = @class.Teacher?.Name ?? "",
            Period = @class.Period?.Name ?? "",
            Workload = @class.Workload,
            Status = @class.Status,
            MyStatus = classStudent.Status,
            Schedules = @class.Schedules
                .OrderBy(s => s.Day).ThenBy(s => s.Start)
                .Select(s => new ScheduleOut(s.Day, s.Start, s.End))
                .ToList(),
        };
    }
}
