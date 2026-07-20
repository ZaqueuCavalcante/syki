using Estud.Back.Domain.Classes;

namespace Estud.Back.Features.Classrooms.GetClassroom;

public class GetClassroomService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetClassroomOut, EstudError>> Get(int id)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var classroom = await ctx.Classrooms.AsNoTracking()
            .Include(c => c.Campus)
            .FirstOrDefaultAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (classroom == null) return ClassroomNotFound.I;

        var schedules = await ctx.Schedules.AsNoTracking()
            .Where(s => s.ClassroomId == id)
            .OrderBy(s => s.Day).ThenBy(s => s.Start)
            .ToListAsync();

        var classIds = schedules
            .Where(s => s.ClassId != null)
            .Select(s => s.ClassId!.Value)
            .Distinct()
            .ToList();

        var classes = await ctx.Classes.AsNoTracking()
            .Include(c => c.Discipline)
            .Include(c => c.Period)
            .Include(c => c.Teachers)
            .Where(c => classIds.Contains(c.Id))
            .ToListAsync();
        var classById = classes.ToDictionary(c => c.Id);

        var studentCounts = await ctx.ClassStudents.AsNoTracking()
            .Where(cs => classIds.Contains(cs.ClassId))
            .GroupBy(cs => cs.ClassId)
            .Select(g => new { ClassId = g.Key, Students = g.Count() })
            .ToListAsync();
        var studentsByClass = studentCounts.ToDictionary(x => x.ClassId, x => x.Students);

        var scheduleOuts = schedules.ConvertAll(s =>
        {
            Class? @class = s.ClassId != null && classById.TryGetValue(s.ClassId.Value, out var c) ? c : null;
            return new ClassroomScheduleOut
            {
                ClassId = s.ClassId ?? 0,
                Discipline = @class?.Discipline?.Name ?? "",
                Period = @class?.Period?.Name ?? "",
                Status = @class?.Status ?? ClassStatus.OnPreEnrollment,
                Students = @class != null && studentsByClass.TryGetValue(@class.Id, out var count) ? count : 0,
                Teachers = @class?.Teachers.Select(t => t.Name).Order().ToList() ?? [],
                Day = s.Day,
                StartAt = s.Start,
                EndAt = s.End,
            };
        });

        var weeklyMinutes = schedules.Sum(s => s.End.ToMinutes() - s.Start.ToMinutes());

        return new GetClassroomOut
        {
            Id = classroom.Id,
            Name = classroom.Name,
            CampusId = classroom.CampusId,
            Campus = classroom.Campus?.Name ?? "",
            Capacity = classroom.Capacity,
            ClassesCount = classIds.Count,
            WeeklyHours = Math.Round(weeklyMinutes / 60M, 1),
            PeakStudents = studentsByClass.Count == 0 ? 0 : studentsByClass.Values.Max(),
            Schedules = scheduleOuts,
        };
    }
}
