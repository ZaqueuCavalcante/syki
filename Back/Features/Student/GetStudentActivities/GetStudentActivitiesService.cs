namespace Syki.Back.Features.Student.GetStudentActivities;

public class GetStudentActivitiesService(SykiDbContext ctx) : IStudentService
{
    public async Task<List<StudentActivityOut>> Get(Guid userId)
    {
        var classesIds = await ctx.ClassesStudents
            .Where(x => x.SykiStudentId == userId)
            .Select(x => x.ClassId).ToListAsync();

        var classes = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .Include(x => x.Activities)
            .Where(x => classesIds.Contains(x.Id))
            .ToListAsync();

        var result = new List<StudentActivityOut>();

        foreach (var @class in classes)
        {
            foreach (var activity in @class.Activities)
            {
                result.Add(activity.ToStudentActivityOut(@class.Discipline.Name));
            }
        }

        return result.OrderByDescending(x => x.CreatedAt).ToList();
    }
}
