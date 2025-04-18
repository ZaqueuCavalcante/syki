namespace Syki.Back.Features.Student.GetStudentClassActivities;

public class GetStudentClassActivitiesService(SykiDbContext ctx) : IStudentService
{
    public async Task<OneOf<List<StudentClassActivityOut>, SykiError>> Get(Guid userId, Guid classId)
    {
        var classOk = await ctx.ClassesStudents.AnyAsync(x => x.ClassId == classId && x.SykiStudentId == userId);
        if (!classOk) return new ClassNotFound();

        var activities = await ctx.ClassActivities.AsNoTracking()
            .Where(x => x.ClassId == classId)
            .OrderBy(x => x.Note)
            .ThenBy(x => x.CreatedAt)
            .ToListAsync();
        var ids = activities.ConvertAll(x => x.Id);
        var works = await ctx.ClassActivityWorks.AsNoTracking()
            .Where(x => ids.Contains(x.ClassActivityId) && x.SykiStudentId == userId).ToListAsync();

        var result = new List<StudentClassActivityOut>();
        foreach (var activity in activities)
        {
            var work = works.First(x => x.ClassActivityId == activity.Id);
            result.Add(new()
            {
                Id = activity.Id,
                ClassId = activity.ClassId,
                Note = activity.Note,
                Title = activity.Title,
                Description = activity.Description,
                Type = activity.Type,
                Status = activity.Status,
                Weight = activity.Weight,
                WorkStatus = work.Status,
                CreatedAt = activity.CreatedAt,
                DueDate = activity.DueDate,
                DueHour = activity.DueHour,
                Value = work.Note,
                PonderedValue = work.Note * activity.Weight / 100M,
            });
        }

        return result;
    }
}
