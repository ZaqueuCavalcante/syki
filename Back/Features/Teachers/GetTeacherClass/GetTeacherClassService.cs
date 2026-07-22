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
            .Include(c => c.Teachers)
            .Include(c => c.Period)
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        var assigned = await ctx.ClassTeachers.AnyAsync(ct => ct.ClassId == id && ct.TeacherId == teacherId);
        if (!assigned) return TeacherNotAssignedToClass.I;

        var classroomIds = @class.Schedules
            .Where(s => s.ClassroomId != null)
            .Select(s => s.ClassroomId!.Value)
            .Distinct()
            .ToList();
        var classroomNames = await ctx.Classrooms.AsNoTracking()
            .Where(c => classroomIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, c => c.Name);

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
                .Select(s => new GetTeacherClassScheduleOut(s.Day, s.Start, s.End)
                {
                    Teacher = s.TeacherId == null ? null : @class.Teachers.FirstOrDefault(t => t.Id == s.TeacherId)?.Name,
                    Classroom = s.ClassroomId != null && classroomNames.TryGetValue(s.ClassroomId.Value, out var name) ? name : null,
                })
                .ToList(),
        };
    }
}
