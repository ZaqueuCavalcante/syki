namespace Estud.Back.Features.Teachers.GetTeacherClassActivities;

public class GetTeacherClassActivitiesService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetTeacherClassActivitiesOut, EstudError>> Get(int classId)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var teacherId = await ctx.GetTeacherId(institutionId, userId);

        var @class = await ctx.Classes.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == classId && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        var assigned = await ctx.ClassTeachers.AnyAsync(ct => ct.ClassId == classId && ct.TeacherId == teacherId);
        if (!assigned) return TeacherNotAssignedToClass.I;

        var activities = await ctx.ClassActivities.AsNoTracking()
            .Where(a => a.ClassId == classId)
            .OrderBy(a => a.Note)
            .ThenBy(a => a.CreatedAt)
            .Select(a => new GetTeacherClassActivitiesItemOut
            {
                Id = a.Id,
                ClassId = a.ClassId,
                Note = a.Note,
                Title = a.Title,
                Description = a.Description,
                Type = a.ActivityType,
                Status = a.Status,
                Weight = a.Weight,
                CreatedAt = a.CreatedAt,
                DueDate = a.DueDate,
                DueHour = a.DueHour,
                DeliveredWorks = a.Works.Count(w => w.Status != ClassActivityWorkStatus.Pending),
                TotalWorks = a.Works.Count,
            })
            .ToListAsync();

        return new GetTeacherClassActivitiesOut { Activities = activities };
    }
}
